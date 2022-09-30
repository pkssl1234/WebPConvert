using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace WebPConvert
{
#pragma warning disable CA1416 // 驗證平台相容性
    public sealed class Libwebp
    {
        [DllImport("library/libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int WebPEncodeBGRA(IntPtr rgba, int width, int height, int stride, float quality_factor, out IntPtr output);

        [DllImport("library/libwebp.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int WebPFree(IntPtr p);

        public static void Encode()
        {
            using var source = new Bitmap("input.png");
            var data = source.LockBits(
                new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            IntPtr webp_data;
            int size = WebPEncodeBGRA(data.Scan0, source.Width, source.Height, data.Stride, 100, out webp_data);

            byte[] buffer = new byte[size];
            Marshal.Copy(webp_data, buffer, 0, size);
            source.UnlockBits(data);
            WebPFree(webp_data);

            File.WriteAllBytes("encode.webp",buffer);
        }
    }
#pragma warning restore CA1416 // 驗證平台相容性
}