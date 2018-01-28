using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
//using System.Threading.Tasks;
using System.Security.Cryptography;

namespace GenFileHashCSV
{
    public class FileHashCSV
    {
        public  static void Generate(string path)
        {
            Uri abPath = new Uri(path);
            DirectoryInfo dinfo = new DirectoryInfo(path);


            //Linq way
            //Dictionary <string, string> finfo = dinfo.GetFiles("*", SearchOption.AllDirectories).ToDictionary(x =>
            //{
            //    Uri fullName = new Uri(x.FullName);
            //    return abPath.MakeRelativeUri(fullName).ToString();
            //}
            //,x=>GetHash(x.FullName)
            //);

            //WriteCSV(finfo);


            //Classic way
            Dictionary<string, string> dicAllfiles = new Dictionary<string, string>();
            FileInfo[] lfinfo = dinfo.GetFiles("*", SearchOption.AllDirectories);
            foreach (var item in lfinfo)
            {
                Uri fullName = new Uri(item.FullName);
                dicAllfiles.Add(abPath.MakeRelativeUri(fullName).ToString(), GetHash(item.FullName));
            }
            WriteCSV(dicAllfiles);
            
            
        }

        private static string GetHash(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    return Encoding.Unicode.GetString(md5.ComputeHash(stream));
                }
            }
        }

        private static void WriteCSV(IDictionary<string, string> dict)
        {
            StringBuilder csv = new StringBuilder();
            foreach (var item in dict)
            {//Hash: is included, becasue only comma may cause confusion, as hash may also start with a comma.
                csv.AppendLine(item.Key + ",Hash:" + item.Value);
            }

            using (TextWriter writer = File.AppendText("AllFiles.csv"))
            {
                writer.Write(System.DateTime.Now + Environment.NewLine);
                
                writer.Write(csv.ToString());
            }

        }

    }
}
