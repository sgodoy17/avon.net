using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IdentiGo.Transversal.Utilities
{
    public static class ReadExcel
    {
        static ISheet _sheet;

        public static void InitializeSheet(Stream file, string fileExt)
        {

            if (fileExt == ".xls")
            {
                var hssfwb = new HSSFWorkbook(file);
                _sheet = hssfwb.GetSheetAt(0);
            }
            else
            {
                var hssfwb = new XSSFWorkbook(file);
                _sheet = hssfwb.GetSheetAt(0);
            }
        }

        public static List<DataTable> ReadExcelToDataTableList(List<Tuple<int, int>> range)
        {
            List<DataTable> dataTableList = new List<DataTable>();
            foreach (var item in range)
            {
                dataTableList.Add(ReadExcelToDataTable(item.Item1, item.Item2));
            }
            return dataTableList;
        }

        public static DataTable ReadExcelToDataTable(int rowFirst = 0, int? rowLast = null, List<int> excludeList = null)
        {
            var table = new DataTable();
            excludeList = excludeList ?? new List<int>();
            var headerRow = _sheet.GetRow(rowFirst);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            rowLast = rowLast ?? _sheet.LastRowNum;
            for (var i = rowFirst + 1; i <= rowLast; i++)
            {
                if (excludeList.Any(x => x == i)) break;

                var row = _sheet.GetRow(i);

                if(row == null) continue;

                var dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        dataRow[j] = GetDataRow(row.GetCell(j), row.GetCell(j).CellType);
                    }
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }

        public static dynamic GetDataRow(ICell row, CellType type)
        {
            dynamic value;
            switch (type)
            {
                case CellType.Unknown:
                    value = row.StringCellValue;
                    break;
                case CellType.Numeric:
                    value = row.NumericCellValue;
                    break;
                case CellType.String:
                    value = row.StringCellValue;
                    break;
                case CellType.Formula:
                    value = GetDataRow(row, row.CachedFormulaResultType);
                    break;
                case CellType.Blank:
                    value = row.StringCellValue;
                    break;
                case CellType.Boolean:
                    value = row.BooleanCellValue;
                    break;
                case CellType.Error:
                    value = row.ErrorCellValue;
                    break;
                default:
                    value = row.RichStringCellValue;
                    break;
            }
            return value;
        }

        public static List<T> ConvertToList<T>()
        {
            DataTable dt = ReadExcelToDataTable();
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (!columnNames.Contains(pro.Name)) continue;
                    PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                    pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));
                }
                return objT;
            }).ToList();
        }

        public static DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            DataTable newDataTable = new DataTable();
            Type impliedType = typeof(T);
            PropertyInfo[] propInfo = impliedType.GetProperties();
            foreach (PropertyInfo pi in propInfo)
                newDataTable.Columns.Add(pi.Name, pi.PropertyType);

            foreach (T item in collection)
            {
                DataRow newDataRow = newDataTable.NewRow();
                newDataRow.BeginEdit();
                foreach (var pi in propInfo)
                    newDataRow[pi.Name] = pi.GetValue(item, null);
                newDataRow.EndEdit();
                newDataTable.Rows.Add(newDataRow);
            }
            return newDataTable;
        }

        //public static void TransferXLToTable()
        //{
        //    DataTable dt = new DataTable();


        //    using (FileStream stream = new FileStream(new SafeFileHandle(IntPtr., FileAccess.ReadWrite))
        //    {
        //        IWorkbook wb = new XSSFWorkbook();
        //        ISheet sheet = wb.CreateSheet("Prueba");
        //        ICreationHelper cH = wb.GetCreationHelper();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            IRow row = sheet.CreateRow(i);
        //            for (int j = 0; j < 3; j++)
        //            {
        //                ICell cell = row.CreateCell(j);
        //                cell.SetCellValue(cH.CreateRichTextString(dt.Rows[i].ItemArray[j].ToString()));
        //            }
        //        }
        //        wb.Write(stream);
        //    }
        //}

        //public static DataTable ConvertToDataTable<T>(IList<T> data)
        //{
        //    PropertyDescriptorCollection properties =
        //       TypeDescriptor.GetProperties(typeof(T));
        //    DataTable table = new DataTable();
        //    foreach (PropertyDescriptor prop in properties)
        //        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        //    foreach (T item in data)
        //    {
        //        DataRow row = table.NewRow();
        //        foreach (PropertyDescriptor prop in properties)
        //            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        //        table.Rows.Add(row);
        //    }
        //    return table;
        //}
    }
}
