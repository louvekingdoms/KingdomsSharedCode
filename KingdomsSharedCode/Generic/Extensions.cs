using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingdomsSharedCode.Generic
{
    static class Extensions
    {

        public static T PickRandom<T>(this List<T> list)
        {
            if (list.Count <= 0)
            {
                throw new IndexOutOfRangeException("The provided list is of zero size.");
            }
            var rnd = new Random();
            return list[rnd.Next(list.Count)];
        }

        public static byte ToByte(this ushort val)
        {
            return (byte)(val % (byte.MaxValue+1));
        }

        public static string ToSumString(this byte[] sum)
        {
            var sb = new StringBuilder();
            foreach(var byt in sum)
            {
                sb.Append(byt.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
