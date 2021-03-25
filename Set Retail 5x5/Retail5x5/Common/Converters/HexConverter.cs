using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Set_Retail_5x5.Retail5x5.Common.Converters
{
    public static class HexConverter
    {
        private static string SetLedZero(object input, int neededLength)
        {
            string inStr = input.ToString();

            int delta = neededLength - inStr.Length;

            if (neededLength > inStr.Length)
            {
                for (int i = 0; i < delta; i++)
                {
                    inStr = $"{0}{inStr}";
                }
            }
            return inStr;
        }

        public static string ToDec(this string hexCode)
        {
            if (hexCode.Length != 8)
            {
                throw new ArgumentException("Это не карта!");
            }
            var b = new byte[4];
            b[3] = Convert.ToByte(hexCode.Substring(6, 2), 16);
            b[2] = Convert.ToByte(hexCode.Substring(4, 2), 16);
            b[1] = Convert.ToByte(hexCode.Substring(2, 2), 16);
            b[0] = Convert.ToByte(hexCode.Substring(0, 2), 16);
            var decCode = string.Concat(SetLedZero(b[0], 3), SetLedZero(b[1], 3), SetLedZero(b[2], 3), SetLedZero(b[3], 3));
            return decCode;
        }

        public static string ToHex(this string decCode)
        {
            if (decCode.Length != 12)
            {
                throw new ArgumentException("Это не карта!");
            }
            var b = new object[4];
            b[3] = Convert.ToByte(decCode.Substring(9, 3), 10).ToString("X2");
            b[2] = Convert.ToByte(decCode.Substring(6, 3), 10).ToString("X2");
            b[1] = Convert.ToByte(decCode.Substring(3, 3), 10).ToString("X2");
            b[0] = Convert.ToByte(decCode.Substring(0, 3), 10).ToString("X2");
            var hexCode = string.Concat(b[0], b[1], b[2], b[3]);
            return hexCode;
        }

        public static string AddLedZero(this string input, int neededLength)
        {
            return SetLedZero(input, neededLength);
        }

    }
}
