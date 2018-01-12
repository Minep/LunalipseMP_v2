using Lunalipse.Common.Data;
using Lunalipse.Common.Interfaces.IAudio;
using CSCore.SoundOut;
using CSCore;
using CSCore.Codecs.MP3;
using CSCore.Codecs.AIFF;
using CSCore.Codecs.AAC;
using CSCore.Codecs.WAV;
using CSCore.Codecs.FLAC;
using CSCore.CoreAudioAPI;
using CSCore.Streams.Effects;
using Lunalipse.Common.Generic;
using System.Threading;
using System;
using Lunalipse.Core.Lyric;
using Lunalipse.Common.Interfaces.ILyric;

namespace Lunalipse.Core.LpsAudio
{
    public class LpsAudio : ILpsAudio , IDisposable
    {
        public volatile static LpsAudio LA_instance;
        public readonly static object LA_lock = new object();

        public static LpsAudio INSTANCE(bool immersed = false)
        {
            if (LA_instance == null)
            {
                lock (LA_lock)
                {
                    LA_instance = LA_instance ?? new LpsAudio(immersed);
                }
            }
            return LA_instance;
        }



        ISoundOut wasapiOut;
        IWaveSource iws;
        Equalizer mEqualizer;
        LpsFftWarp lfw;
        Thread Counter;
        LyricEnumerator lEnum;
        bool isPlaying = false;
        public float Volume
        {
            get
            {
                return wasapiOut.Volume;
            }
            set
            {
                wasapiOut.Volume = value;
            }
        }
        public bool wasapiSupport
        {
            get
            {
                return WasapiOut.IsSupportedOnCurrentPlatform;
            }
        }
        public Equalizer LpsEqualizer
        {
            get
            {
                return mEqualizer;
            }
            set
            {
                mEqualizer = value;
            }
        }
        public ILyricTokenizer LyricTokenzier
        {
            get
            {
                return lEnum.Tokenizer;
            }
            set
            {
                lEnum.Tokenizer = value;
            }
        }

        // Constructor
        public LpsAudio(bool immersed=false)
        {
            wasapiOut = WasapiOut.IsSupportedOnCurrentPlatform ? GetWasapiSoundOut(immersed) : GetDirectSoundOut();
            lfw = LpsFftWarp.INSTANCE;
            lEnum = new LyricEnumerator();
            

            wasapiOut.Stopped += (s, e) =>
            {
                //Counter?.Abort();
                //AudioDelegations.PlayingFinished?.Invoke();
            };

            Counter = new Thread(new ThreadStart(CountTimerDelegate));
        }

        //Interface implements
        public void Load(MusicEntity music)
        {
            AudioDelegations.LyricLoadStatus?.Invoke(lEnum.AcquireLyric(music));
            initializeSoundSource(music);
            AudioDelegations.MusicLoaded?.Invoke(music,iws.ToTrack());
        }

        public void MoveTo(long secs)
        {
            if (secs < iws.Length)
            {
                iws.Position = secs;
            }
        }

        public void Pause()
        {
            isPlaying = false;
            wasapiOut.Pause();
            AudioDelegations.StatuesChanged?.Invoke(isPlaying);
        }

        public void Play()
        {
            wasapiOut.Play();
            Counter = new Thread(new ThreadStart(CountTimerDelegate));
            Counter?.Start();
        }

        public void Resume()
        {
            isPlaying = true;
            wasapiOut.Resume();
            AudioDelegations.StatuesChanged?.Invoke(isPlaying);
        }

        public void SetEqualizer(double[] data)
        {
            for(int i = 0; i < data.Length; i++)
            {
                if (!SetEqualizerIndex(i, data[i])) break;
            }
        }

        public bool SetEqualizerIndex(int inx, double data)
        {
            if (inx >= mEqualizer.SampleFilters.Count) return false;
            mEqualizer.SampleFilters[inx].AverageGainDB = data * 20;
            return true;
        }


        // Private Methods
        private IWaveSource GetCodec(string type, string file)
        {
            switch (type)
            {
                case SupportFormat.MP3:
                    if (file != null) return new DmoMp3Decoder(file);
                    break;
                case SupportFormat.FLAC:
                    if (file != null) return new FlacFile(file);
                    break;
                case SupportFormat.WAV:
                    if (file != null) return new WaveFileReader(file);
                    break;
                case SupportFormat.ACC:
                    if (file != null) return new AacDecoder(file);
                    break;
                case SupportFormat.AIFF:
                    if (file != null) return new AiffReader(file);
                    break;
            }
            return null;
        }

        private ISoundOut GetWasapiSoundOut(bool immersed = false, int latency = 100)
        {
            return new WasapiOut(true, immersed ? AudioClientShareMode.Exclusive : AudioClientShareMode.Shared, latency);
        }

        private ISoundOut GetDirectSoundOut(int latency = 100)
        {
            return new DirectSoundOut(latency);
        }

        private void initializeSoundSource(MusicEntity music)
        {
            iws?.Dispose();
            iws = GetCodec(music.Extension, music.Path);
            iws = lfw.Initialize(iws.ToSampleSource().AppendSource(Equalizer.Create10BandEqualizer, out mEqualizer));
            wasapiOut.Initialize(iws);
            Volume = 0.7f;
            wasapiOut.Volume = Volume;
        }
        
        private void CountTimerDelegate()
        {
            double totalMS = iws.GetLength().TotalMilliseconds;
            while(iws.GetPosition().TotalMilliseconds <= totalMS)
            {
                if (isPlaying)
                {
                    AudioDelegations.PostionChanged?.Invoke(iws.GetPosition());
                    LyricToken lt;
                    if((lt = lEnum.Enumerating(iws.GetPosition()))!= null)
                    {
                        AudioDelegations.LyricUpdated?.Invoke(lt);
                    }
                }
                Thread.Sleep(1000);
            }
            AudioDelegations.PlayingFinished?.Invoke();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (iws!=null && wasapiOut!=null)
                    {
                        wasapiOut.Stop();
                        iws.Dispose();
                        wasapiOut.Dispose();
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                disposedValue = true;
                LA_instance = null;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~LpsAudio() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
