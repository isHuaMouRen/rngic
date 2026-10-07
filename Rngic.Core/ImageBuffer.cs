using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Rngic.Core;

internal sealed class ImageBuffer
{
    private readonly byte[] _buffer;

    public int Width { get; }
    public int Height { get; }

    public const int BytesPerPixel = 4;

    public int Stride => Width * BytesPerPixel;

    public ImageBuffer(int width, int height)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width));

        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height));

        Width = width;
        Height = height;

        _buffer = new byte[checked(width * height * BytesPerPixel)];
    }

    public ImageBuffer(Bitmap bitmap)
    {
        ArgumentNullException.ThrowIfNull(bitmap);

        Width = bitmap.Width;
        Height = bitmap.Height;

        _buffer = new byte[checked(Width * Height * BytesPerPixel)];

        BitmapData? data = null;

        try
        {
            data = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int sourceStride = data.Stride;
            int copyStride = Stride;

            for (int y = 0; y < Height; y++)
            {
                IntPtr source = IntPtr.Add(data.Scan0, y * sourceStride);

                Marshal.Copy(source, _buffer, y * copyStride, copyStride);
            }
        }
        finally
        {
            if (data != null)
                bitmap.UnlockBits(data);
        }
    }

    public Pixel GetPixel(int x, int y)
    {
        int index = GetIndex(x, y);

        return new Pixel(_buffer[index + 2], _buffer[index + 1], _buffer[index], _buffer[index + 3]);
    }

    public void SetPixel(int x, int y, Pixel pixel)
    {
        int index = GetIndex(x, y);

        _buffer[index] = pixel.B;
        _buffer[index + 1] = pixel.G;
        _buffer[index + 2] = pixel.R;
        _buffer[index + 3] = pixel.A;
    }

    public void GetPixel(int x, int y, out byte r, out byte g, out byte b, out byte a)
    {
        int index = GetIndex(x, y);

        b = _buffer[index];
        g = _buffer[index + 1];
        r = _buffer[index + 2];
        a = _buffer[index + 3];
    }

    public void SetPixel(int x, int y, byte r, byte g, byte b, byte a)
    {
        int index = GetIndex(x, y);

        _buffer[index] = b;
        _buffer[index + 1] = g;
        _buffer[index + 2] = r;
        _buffer[index + 3] = a;
    }

    public void SwapPixel(int x1, int y1, int x2, int y2)
    {
        int index1 = GetIndex(x1, y1);
        int index2 = GetIndex(x2, y2);

        if (index1 == index2)
            return;

        byte temp;

        temp = _buffer[index1];
        _buffer[index1] = _buffer[index2];
        _buffer[index2] = temp;

        temp = _buffer[index1 + 1];
        _buffer[index1 + 1] = _buffer[index2 + 1];
        _buffer[index2 + 1] = temp;

        temp = _buffer[index1 + 2];
        _buffer[index1 + 2] = _buffer[index2 + 2];
        _buffer[index2 + 2] = temp;

        temp = _buffer[index1 + 3];
        _buffer[index1 + 3] = _buffer[index2 + 3];
        _buffer[index2 + 3] = temp;
    }


    public Bitmap ToBitmap()
    {
        Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

        BitmapData? data = null;

        try
        {
            data = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int destinationStride = data.Stride;
            int copyStride = Stride;

            for (int y = 0; y < Height; y++)
            {
                IntPtr destination = IntPtr.Add(data.Scan0, y * destinationStride);

                Marshal.Copy(_buffer, y * copyStride, destination, copyStride);
            }

            bitmap.UnlockBits(data);
            data = null;

            return bitmap;
        }
        catch
        {
            if (data != null)
                bitmap.UnlockBits(data);

            bitmap.Dispose();
            throw;
        }
    }

    private int GetIndex(int x, int y)
    {
        if ((uint)x >= (uint)Width)
            throw new ArgumentOutOfRangeException(nameof(x));

        if ((uint)y >= (uint)Height)
            throw new ArgumentOutOfRangeException(nameof(y));

        return checked((y * Width + x) * BytesPerPixel);
    }
}