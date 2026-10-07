using System.Drawing;

namespace Rngic.Core
{
    public class Confuser
    {
        public Bitmap Bitmap { get; }
        public int Key { get; }

        private readonly Random _ran;
        private readonly ImageBuffer _buffer;

        private int[] _targets;

        public Confuser(Bitmap bitmap, int key)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            Bitmap = bitmap;
            Key = key;

            _ran = new Random(key);
            _buffer = new ImageBuffer(bitmap);

            int totalPixelCount = _buffer.Width * _buffer.Height;
            _targets = new int[totalPixelCount];
            for (int i = 0; i < totalPixelCount; i++)
                _targets[i] = _ran.Next(0, totalPixelCount);
        }


        public Bitmap Confuse()
        {
            for (int i = 0; i < _targets.Length; i++)
            {
                int targetPixel = _targets[i];

                int x = i % _buffer.Width;
                int y = i / _buffer.Width;

                int targetX = targetPixel % _buffer.Width;
                int targetY = targetPixel / _buffer.Width;

                _buffer.SwapPixel(x, y, targetX, targetY);
            }

            return _buffer.ToBitmap();
        }

        public Bitmap Deconfuse()
        {
            for (int i = _targets.Length - 1; i >= 0; i--)
            {
                int targetPixel = _targets[i];

                int x = i % _buffer.Width;
                int y = i / _buffer.Width;

                int targetX = targetPixel % _buffer.Width;
                int targetY = targetPixel / _buffer.Width;

                _buffer.SwapPixel(x, y, targetX, targetY);
            }

            return _buffer.ToBitmap();
        }
    }
}
