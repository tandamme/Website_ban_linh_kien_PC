using System.Security.Cryptography;
using System.Text;

namespace Do_an_lap_trinh_c_.Utils
{
    public class MyUtils
    {
        public static string keyGenerator(int length = 10)
        {
            var chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            var rd = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rd.Next(s.Length)]).ToArray());
        }

        public static string ToMd5Hash(string password, string key)
        {
            using MD5 md5 = MD5.Create();
            byte[] input = Encoding.UTF8.GetBytes(password + key);
            byte[] hash = md5.ComputeHash(input);
            return Convert.ToHexString(hash);
        }
    }
}