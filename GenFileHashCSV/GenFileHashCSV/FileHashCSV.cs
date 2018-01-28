using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace GenFileHashCSV
{
    public class FileHashCSV
    {
        public  static void Generate(string path)
        {
            Uri abPath = new Uri(path);
            DirectoryInfo dinfo = new DirectoryInfo(path);

            Dictionary <string, string> finfo = dinfo.GetFiles("*", SearchOption.AllDirectories).ToDictionary(x =>
            {
                Uri fullName = new Uri(x.FullName);
                return abPath.MakeRelativeUri(fullName).ToString();
            }
            ,x=>GetHash(x.FullName)
            );
        }

        private static string GetHash(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    return Encoding.Default.GetString(md5.ComputeHash(stream));
                }
            }
        }



    }
}
