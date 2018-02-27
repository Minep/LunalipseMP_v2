using Lunalipse.Resource.Generic;
using Lunalipse.Resource.Generic.Types;
using Lunalipse.Resource.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static Lunalipse.Resource.Generic.Structure;

namespace Lunalipse.Resource
{
    public class LrssWriter : ILrssWriter
    {
        LPS_HEADER header;
        int len_header, len_fheader, len_dblock;
        List<LrssResource> Resources;
        FileStream fs;
        int magic;

        public event Delegates.ChuckOperated OnChuckWrited;
        public event Delegates.EndpointReached OnEndpointReached;

        public LrssWriter()
        {
            Resources = new List<LrssResource>();
            len_header = Marshal.SizeOf(typeof(LPS_HEADER));
            len_fheader = Marshal.SizeOf(typeof(LPS_FHEADER));
            len_dblock = Marshal.SizeOf(typeof(LPS_FBLOCK));
        }
        public void Initialize(int Magic, string signature, string dest, byte[] EncKey = null)
        {
            magic = Magic;
            header = new LPS_HEADER();
            header.H_MAGIC = EncKey == null ? Magic : Magic.XorEncrypt(EncKey);
            header.H_FH_LOC = new long[32];
            header.H_SIG = signature;
            header.H_ENCRYPTED = EncKey != null;

            if (File.Exists(dest)) File.Delete(dest);
            fs = new FileStream(dest, FileMode.OpenOrCreate);
        }
        
        public async Task<bool> Export()
        {
            await Task.Run(() =>
            {
                fs.Write(new byte[len_header], 0, len_header);
                for (int i = 0; i < Resources.Count;)
                {
                    _writeHeader(Resources[i], ref i);
                    Resources[i - 1] = null;
                }
                Resources.Clear();
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(header.ToBytes(len_header), 0, len_header);
                OnEndpointReached?.Invoke(Resources.Count);
            });
            fs.Close();
            return true;
        }

        public void RemoveResource(int index)
        {
            Resources.RemoveAt(index);
        }

        public async Task<bool> AppendResource(string path)
        {
            if (Resources.Count >= 32) return false;
            await Task.Run(() => Resources.Add(new LrssResource(path)));
            return true;
        }

        public async Task<bool> AppendResources(string baseDir)
        {
            foreach (string path in Directory.GetFiles(baseDir))
            {
                if (!await AppendResource(path)) return false;
            }
            return true;
        }

        public async Task<bool> AppendResources(params string[] pathes)
        {
            foreach(string path in pathes)
            {
                if (!await AppendResource(path)) return false;
            }
            return true;
        }


        private void _writeHeader(LrssResource lr, ref int index)
        {
            header.H_FH_LOC[index] = fs.Length;
            LPS_FHEADER fhe = new LPS_FHEADER()
            {
                FH_INDEX = index,
                FH_SIZE = lr.Size,
                FH_NAME = lr.Name,
                FH_TYPE = lr.Type,
                FH_BCOUNT = (int)Math.Ceiling(lr.Size / 1024d)
            };
            fs.Write(fhe.ToBytes(len_fheader).XorCrypt(magic), 0, len_fheader);
            _writeBlocks(lr.Data, fhe.FH_BCOUNT);
            index++;
        }

        private void _writeBlocks(byte[] b, int bcount)
        {
            int paddingBytes = b.Length % 1024;
            int index = 0;
            for(int i = 0; i < bcount; i++)
            {
                LPS_FBLOCK lfb = new LPS_FBLOCK();
                lfb.FB_INDEX = index;
                bool bl = i == bcount - 1 && paddingBytes != 0;
                lfb.FB_DAT = new byte[1024];
                Array.Copy(b, i * 1024, lfb.FB_DAT, 0, bl ? paddingBytes : 1024);
                fs.Write(lfb.ToBytes(len_dblock).XorCrypt(magic), 0, len_dblock);
                index++;
                OnChuckWrited?.Invoke(index, bcount);
            }
        }
    }
}
