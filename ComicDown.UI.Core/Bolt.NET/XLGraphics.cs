using System;
using System.Runtime.InteropServices;

namespace ComicDown.UI.Core.Bolt
{
    public static class XLGraphics
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct tagXLGraphicPlusParam
        {
            public bool bInitLua;
        }

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern IntPtr XLGP_LoadGifFromFile(string lpFileName);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern IntPtr XLGP_GifGetFrame(IntPtr hGif, int uFrameIndex);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern IntPtr XLGP_GifGetFirstFrame(IntPtr hGif);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern IntPtr XLGP_GifGetNextFrame(IntPtr hGif);

        public static IntPtr XLGP_LoadFirstGifFrame(string lpFileName)
        {
            var gif = XLGP_LoadGifFromFile(lpFileName);
            return XLGP_GifGetFirstFrame(gif);
        }

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern IntPtr XLGP_LoadJpegFromFile(string lpFileName);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern IntPtr XLGP_LoadBmpFromFile(string lpFileName);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern IntPtr XL_LoadPngFromFile(string lpFileName);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int XL_ReleaseBitmap(IntPtr hBitmap);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool XLGP_PushBitmap(IntPtr luaState, IntPtr hBitmap);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool XLGP_CheckBitmap(IntPtr luaState, int index, ref IntPtr lpBitmap);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XL_StretchBitmap(IntPtr hBitmap, int newWidth, int newHeight);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern long XLGP_InitGraphicPlus(IntPtr theParam);
    }
}