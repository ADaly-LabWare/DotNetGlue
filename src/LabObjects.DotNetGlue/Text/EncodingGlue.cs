using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabObjects.DotNetGlue.Text
{
    internal class EncodingGlue
    {
        public static string GetUTF16LittleEndianBase64(string stringToConvert)
        {
            UnicodeEncoding enc = new UnicodeEncoding(false,true);
            byte[] b = enc.GetBytes(stringToConvert);
            return Convert.ToBase64String(b);
        }
        public static string GetUTF16BigEndianBase64(string stringToConvert)
        {
            UnicodeEncoding enc = new UnicodeEncoding(true,false);

             byte[] b = Encoding.Unicode.GetBytes(stringToConvert);
            return Convert.ToBase64String(b);
        }

        public static string GetUTF7Base64(string stringToConvert)
        {
            byte[] b = Encoding.UTF7.GetBytes(stringToConvert);
            return Convert.ToBase64String(b);
        }

        public static string GetUTF8Base64(string stringToConvert)
        {
            byte[] b = Encoding.UTF8.GetBytes(stringToConvert);
            return Convert.ToBase64String(b);
        }

        public static string GetUTF32Base64(string stringToConvert)
        {
            byte[] b = Encoding.UTF32.GetBytes(stringToConvert);
            return Convert.ToBase64String(b);
        }
    }
}
