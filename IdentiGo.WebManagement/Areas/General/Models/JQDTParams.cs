using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentiGo.WebManagement.Areas.General.Models
{
    public class JQDTParams
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }        
        public JQDTColumnSearch Search { get; set; }
        public List<JQDTColumnOrder> Order { get; set; }
        public List<JQDTColumn> Columns { get; set; }
    }

    public enum JQDTColumnOrderDirection
    {
        asc,
        desc
    }

    public class JQDTColumnOrder
    {
        public int Column { get; set; }
        public JQDTColumnOrderDirection Direction { get; set; }
    }

    public class JQDTColumnSearch
    {
        public string Value { get; set; }
        public string Regex { get; set; }
    }

    public class JQDTColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public Boolean Searchable { get; set; }
        public Boolean Orderable { get; set; }
        public JQDTColumnSearch Search { get; set; }
    }
}