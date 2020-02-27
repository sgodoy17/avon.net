using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Transversal.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> Random<T>(this IEnumerable<T> source, int count = 0)
        {
            List<int> randomList = new List<int>();
            List<T> questionDtoList = new List<T>();
            Random random = new Random();
            var numberList = count == 0 || count > source.Count() ? source.Count() : count;
            for (int i = 0; i < numberList; i++)
            {
                var item = random.Next(source.Count());
                if (!randomList.Any())
                {
                    questionDtoList.Add(source.ElementAt(item));
                    randomList.Add(item);
                }
                else
                {
                    while (randomList.Any(x => x == item) == true)
                    {
                        item = random.Next(source.Count());
                    }
                    questionDtoList.Add(source.ElementAt(item));
                    randomList.Add(item);
                }
            }
            return questionDtoList;
        }
    }
}
