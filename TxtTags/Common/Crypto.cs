using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TxtTags.Common
{
    public class Crypto
    {
        public static string GetMd5Str(string ConvertString)
        {
            //System.Convert.ToBase64String
            char[] hexDigits = {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c',
                'd', 'e', 'f'
            };

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] md = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(ConvertString));

            int j = md.Length;
            char[] str = new char[j * 2];

            int k = 0;

            for (int i = 0; i < j; i++)
            {
                byte byte0 = md[i];
                str[k++] = hexDigits[byte0 >> 4 & 0xf];
                str[k++] = hexDigits[byte0 & 0xf];
            }

            return new String(str);
        }
    }
}
