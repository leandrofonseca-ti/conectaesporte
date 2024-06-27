using System.Text;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace ConectaEsporte.Core
{
    public static class Util
    {

        public static string GetPhoneDDD(string phone)
        {
            var numericChars = "0123456789".ToCharArray();
            var numbers = new String(phone.Where(c => numericChars.Any(n => n == c)).ToArray());
            if (!String.IsNullOrEmpty(numbers))
                return numbers.Substring(0, 2);

            return string.Empty;
        }

        public static string GetPhoneNumber(string phone)
        {
            var numericChars = "0123456789".ToCharArray();
            var numbers = new String(phone.Where(c => numericChars.Any(n => n == c)).ToArray());
            if (!String.IsNullOrEmpty(numbers))
                return numbers.Substring(2);

            return string.Empty;
        }

        public static string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a ‘readable’ string
            return BitConverter.ToString(encodedBytes);
        }

    }
}
