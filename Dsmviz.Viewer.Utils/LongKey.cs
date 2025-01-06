namespace Dsmviz.Viewer.Utils
{
    public class LongKey
    {
        public static long Create(int first, int second)
        {
            return (first * 0x10000) + second;
        }
    }
}
