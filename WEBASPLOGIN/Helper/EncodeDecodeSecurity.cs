using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBASPLOGIN.Helper
{
    public class EncodeDecodeSecurity
    {
        public static string EncriptPassword(string valPassword)
        {
            string vResult = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(valPassword);
            vResult = Convert.ToBase64String(encryted);
            return vResult;
        }

        public static string DecryptPassword(string valPassword)
        {
            string vResult = string.Empty;
            byte[] decrypt = Convert.FromBase64String(valPassword);
            vResult = System.Text.Encoding.Unicode.GetString(decrypt);
            return vResult;
        }
    }
}
