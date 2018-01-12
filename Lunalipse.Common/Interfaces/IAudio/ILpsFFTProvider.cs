namespace Lunalipse.Common.Interfaces.IAudio
{
    public interface ILpsFFTProvider
    {
        bool GetFftData(float[] fftBuffer, object context);
        int GetFftBandIndex(float frequency);
    }
}