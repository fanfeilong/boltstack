using System;
using System.Runtime.InteropServices;

namespace ComicDown.UI.Core.Bolt
{
    public enum LuaInnerIndex
    {
        LUA_REGISTRYINDEX = -10000,
        LUA_ENVIRONINDEX = -10001,
        LUA_GLOBALSINDEX = -10002,
    }

    public enum LuaTypes
    {
        LUA_TNIL = 0,
        LUA_TBOOLEAN = 1,
        LUA_TLIGHTUSERDATA = 2,
        LUA_TNUMBER = 3,
        LUA_TSTRING = 4,
        LUA_TTABLE = 5,
        LUA_TFUNCTION = 6,
        LUA_TUSERDATA = 7,
        LUA_TTHREAD = 8
    }

    public static class Lua
    {
        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushstring(
            IntPtr luaState,
            [MarshalAs(UnmanagedType.CustomMarshaler,
                MarshalTypeRef = typeof(UTF8Marshaler))]
            string s);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushinteger(IntPtr luaState, int n);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushnil(IntPtr luaState);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushboolean(IntPtr luaState, int boolean);

        public static void lua_pushboolean(IntPtr luaState, bool boolean)
        {
            lua_pushboolean(luaState, boolean ? 1 : 0);
        }

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushnumber(IntPtr luaState, double n);

        [DllImport("XLUE.dll", CharSet = CharSet.Unicode,
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler,
            MarshalTypeRef = typeof(UTF8MarshalerNoCleanUp))]
        public static extern string lua_tolstring(
            IntPtr luaState,
            int index,
            UIntPtr len);

        public static string lua_tostring(IntPtr luaState, int index)
        {
            return lua_tolstring(luaState, index, UIntPtr.Zero);
        }

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_tointeger(IntPtr luaState, int index);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool lua_toboolean(IntPtr luaState, int index);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double lua_tonumber(IntPtr luaState, int index);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_objlen(IntPtr luaState, int index);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_setfield(
            IntPtr luaState,
            int index,
            [MarshalAs(UnmanagedType.CustomMarshaler,
                MarshalTypeRef = typeof(UTF8Marshaler))]
            string key);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawgeti(IntPtr luaState, int index, int n);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawseti(IntPtr luaState, int idx, int n);


        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_createtable(
            IntPtr luaState,
            int narr,
            int nrec);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_settable(IntPtr luaState, int idx);

        public static void lua_newtable(IntPtr luaState)
        {
            lua_createtable(luaState, 0, 0);
        }

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr luaL_checkudata(
            IntPtr luaState,
            int ud,
            string tname);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_type(IntPtr luaState, int index);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_ref(IntPtr luaState, int t);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_unref(IntPtr luaState, int t, int n);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_gettop(IntPtr luaState);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_settop(IntPtr luaState, int newTop);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushlightuserdata(IntPtr L, IntPtr handle);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_touserdata(IntPtr L, int idx);
    }
}
