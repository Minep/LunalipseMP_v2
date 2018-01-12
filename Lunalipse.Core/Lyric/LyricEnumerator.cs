using Lunalipse.Common.Interfaces.ILyric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunalipse.Common.Data;
using System.IO;
using Lunalipse.Common.Generic;

namespace Lunalipse.Core.Lyric
{
    class LyricEnumerator : ILyricEnumerator
    {
        public ILyricTokenizer Tokenizer { get; set; }
        public string LyricDefaultDir{ get; set; }
        private List<LyricToken> tokens = null;
        public bool AcquireLyric(MusicEntity Music)
        {
            if (Tokenizer == null)
            {
                return false;
            }
            tokens = Tokenizer.CreateTokensFromFile(GetLyricFile(Music.Path, Music.Name));
            if(tokens == null) return false;
            return true;
        }

        public LyricToken Enumerating(TimeSpan current)
        {
            if (tokens != null)
            {
                for (int i = 0; i < tokens.Count; i++)
                {
                    if (i == tokens.Count - 1)
                    {
                        return tokens[i];
                    }
                    else if (isInRangeBetween(tokens[i].TimeStamp, tokens[i + 1].TimeStamp, current))
                    {
                        return tokens[i];
                    }
                }
                return null;
            }
            return null;
        }

        private string GetLyricFile(string path, string name)
        {
            return "{0}/{1}/{2}.lrc".FormateEx(Path.GetDirectoryName(path), LyricDefaultDir, name);
        }

        private bool isInRangeBetween(TimeSpan first, TimeSpan last, TimeSpan current)
        {
            return current >= first && current <= last;
        }
    }
}
