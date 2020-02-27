using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Component.Transversal.Extensions;

namespace Component.Transversal.Utilities
{
    public static class Utilities
    {
        public const int NumberQuestion = 4;

        public static string OptionTelefono = "6103872";

        public static string DisplayFormatDate = "MMMM dd, yyyy";

        public static string OptionLabelInvalidQuestion = "ninguna de las Anteriores";

        private static readonly string[] AdrresStrings = { "Calle", "Avenidad", "Carrera", "Transversal", "Diagonal", "Circular" };

        private static readonly string[] TypeAffiliation = { "COTIZANTE;COTIZANTE PRINCIPAL", "BENEFICIARIO", "OTRO" };

        public static readonly string[] DeathOptions = { "AFILIADO FALLECIDO" };

        public static readonly string DeathFieldRuaf = "RuafHealth.HealthStateAffiliate";

        public static readonly string DeathFieldFosyga = "StateAffiliation";

        public static string DefaulTypeAffiliation = "Ninguna";

        public static string GenerateAddress()
        {
            var random = new Random();
            return string.Format("{0} {1} #{2}-{3}", AdrresStrings[random.Next(AdrresStrings.Length)], random.Next(1, 100), random.Next(1, 100), random.Next(10, 100));
        }

        public static List<string> GetAllExceptByNameTypeAffiliation(string name)
        {
            List<string> list = new List<string>();
            foreach (var type in TypeAffiliation)
            {
                string[] names = type.Split(';');
                if(names.All(x => x.ToUpper() != name.ToUpper()))
                    list.Add(names[0]);
            }
            return list;
        }

        public static DateTime GenerateDateAffiliation()
        {
            Random random = new Random();
            return new DateTime(DateTime.Now.Year - random.Next(0, 15), random.Next(1, 12), random.Next(1, 28));
        }
    }
}
