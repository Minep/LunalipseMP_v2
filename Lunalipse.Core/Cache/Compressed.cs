using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunalipse.Core.Cache
{
    public class Compressed
    {
        public static bool writeCompressed(byte[] b, string path,bool enableCompress = true)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    if (enableCompress)
                    {
                        using (GZipStream gzs = new GZipStream(fs, CompressionMode.Compress, false))
                        {
                            gzs.Write(b, 0, b.Length);
                        }
                    }
                    else
                    {
                        fs.Write(b, 0, b.Length);
                    }
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public static string readCompressed(string path, bool isCompress = true)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                string str;
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    if (isCompress)
                    {
                        using (GZipStream gzs = new GZipStream(fs, CompressionMode.Decompress, false))
                        {
                            gzs.CopyTo(ms);
                        }
                    }
                    else
                    {
                        fs.CopyTo(ms);
                    }
                }
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    str = sr.ReadToEnd();
                }
                ms.Close();
                return str;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}
