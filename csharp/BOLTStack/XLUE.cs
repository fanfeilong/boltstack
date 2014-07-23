using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BOLTStack
{
    public static class XLUE
    {
        [DllImport("XLUE.dll", EntryPoint = "XL_InitGraphicLib")]
        public static extern long XL_InitGraphicLib(int theParam);

        [DllImport("XLUE.dll", EntryPoint = "XL_SetFreeTypeEnabled")]
        public static extern long XL_SetFreeTypeEnabled(int isEnable);

        [DllImport("XLUE.dll", EntryPoint = "XLUE_InitLoader")]
        public static extern long XLUE_InitLoader(int theParam);

        [DllImport("XLUE.dll", CharSet = CharSet.Unicode, EntryPoint = "XLUE_AddXARSearchPath")]
        public static extern long XLUE_AddXARSearchPath(String theParam);

        [DllImport("XLUE.dll", CharSet = CharSet.Ansi, EntryPoint = "XLUE_LoadXAR")]
        public static extern long XLUE_LoadXAR(String xarName);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern long XLUE_GetLuaStack(IntPtr luaState, StringBuilder lpStackBuffer, int bufferSize);
    }
}