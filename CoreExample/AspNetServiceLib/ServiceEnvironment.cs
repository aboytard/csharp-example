using System.Security.Cryptography;
using System.Text;

namespace AspNetServiceLib
{
    public static class ServiceEnvironment
    {
        public static string ServiceExampleWorkDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ServiceExample");

        public static string GetServiceWorkDir(string serviceName) => Path.Combine(ServiceExampleWorkDir, GetSanitizedDirectoryName(serviceName));

        private static string GetSanitizedDirectoryName(string name)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            if (name.IndexOfAny(invalidChars) < 0)
            {
                return name;
            }

            var nameBuilder = new StringBuilder(name);
            foreach (var invalidChar in invalidChars)
            {
                nameBuilder.Replace(invalidChar, '-');
            }

            nameBuilder.Append("_");
            //Append hash of original name to ensure unique directory name:
            nameBuilder.Append(CreateStringHash(name));

            return nameBuilder.ToString();
        }

        private static string CreateStringHash(string input)
        {
            using (var hashAlgo = SHA256.Create())
            {
                var hash = hashAlgo.ComputeHash(Encoding.UTF8.GetBytes(input));
                return string.Join("-", hash.Select(value => value.ToString("X2")));
            }
        }
    }
}
