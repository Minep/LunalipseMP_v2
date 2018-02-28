﻿using Lunalipse.Resource.Generic;
using Lunalipse.Resource.Generic.Types;
using Lunalipse.Resource.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using static Lunalipse.Resource.Generic.Structure;
using static Lunalipse.Resource.Generic.Delegates;

namespace Lunalipse.Resource
{
    public class LrssReader : ILrssReader, IDisposable
    {
        LPS_HEADER HEADER;

        FileStream fs;

        int len_header, len_fheader, len_dblock;
        int MAGIC;

        public LrssReader()
        {
            len_header = Marshal.SizeOf(typeof(LPS_HEADER));
            len_fheader = Marshal.SizeOf(typeof(LPS_FHEADER));
            len_dblock = Marshal.SizeOf(typeof(LPS_FBLOCK));
        }

        public LrssReader(string path, byte[] DecKey = null) : this()
        {
            len_header = Marshal.SizeOf(typeof(LPS_HEADER));
            len_fheader = Marshal.SizeOf(typeof(LPS_FHEADER));
            len_dblock = Marshal.SizeOf(typeof(LPS_FBLOCK));
            fs = new FileStream(path, FileMode.Open);
            ReadHeader();
            if (HEADER.H_ENCRYPTED && DecKey == null)
            {
                fs.Close();
                throw new UnauthorizedAccessException("File has been encrypted, need key.");
            }
            else if (HEADER.H_ENCRYPTED)
                MAGIC = HEADER.H_MAGIC.XorDecrypt(DecKey);
        }

        public List<LrssIndex> GetIndex()
        {
            List<LrssIndex> lis = new List<LrssIndex>();
            for (int i = 0; i < HEADER.H_FH_LOC.Length; i++)
            {
                long l = HEADER.H_FH_LOC[i];
                if (l == 0) continue;
                lis.Add(new LrssIndex(ReadFileHeader(l), l));
            }
            return lis;
        }

        public async Task<LrssResource> ReadResource(LrssIndex li)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return new LrssResource(li.Size, li.Name, li.Type)
                    {
                        Data = GetContent(li.Size, li.Address, li.Occupied)
                    };
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    OnEndpointReached?.Invoke(li.Occupied);
                }
            });
        }

        private void ReadHeader()
        {
            byte[] b = new byte[len_header];
            fs.Read(b, 0, len_header);
            HEADER = (LPS_HEADER)b.ToStruct(typeof(LPS_HEADER));
            fs.Seek(0, SeekOrigin.Begin);
        }

        private LPS_FHEADER ReadFileHeader(long pos)
        {
            fs.Seek(pos, SeekOrigin.Begin);
            byte[] b = new byte[len_fheader];
            fs.Read(b, 0, len_fheader);
            fs.Seek(0, SeekOrigin.Begin);
            return (LPS_FHEADER)b.XorCrypt(MAGIC).ToStruct(typeof(LPS_FHEADER));
        }

        private byte[] GetContent(long size, long addr, int dcount)
        {
            long dblock = addr + len_fheader;
            int left = (int)(size % 1024);
            byte[] _dat = new byte[size];
            fs.Seek(dblock, SeekOrigin.Begin);
            for (int i = 0; i < dcount; i++)
            {
                Array.Copy(GetBlock().FB_DAT, 0, _dat, i * 1024, (i == dcount - 1 && left != 0) ? left : 1024);
                OnChuckLoaded?.Invoke(i, dcount);
            }
            return _dat;
        }

        private LPS_FBLOCK GetBlock()
        {
            byte[] b = new byte[len_dblock];
            fs.Read(b, 0, len_dblock);
            return (LPS_FBLOCK)b.XorCrypt(MAGIC).ToStruct(typeof(LPS_FBLOCK));
        }

        public void Dispose()
        {
            ((IDisposable)fs).Dispose();
        }
    }
}
