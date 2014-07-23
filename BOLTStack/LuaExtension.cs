using System;
using System.Collections.Generic;

namespace BOLTStack
{
    public static class LuaExtension
    {
        #region L Push Extension
        private static void Push(this IntPtr L, string str)
        {
            Lua.lua_pushstring(L, str);
        }
        private static void Push(this IntPtr L, int value)
        {
            Lua.lua_pushinteger(L, value);
        }
        private static void Push(this IntPtr L, double value)
        {
            Lua.lua_pushnumber(L, value);
        }
        private static void Push(this IntPtr L, bool value)
        {
            Lua.lua_pushboolean(L, value);
        }

        public static void PushHandle(this IntPtr L, IntPtr handle)
        {
            Lua.lua_pushlightuserdata(L, handle);
        }
        public static void PushString(this IntPtr L, string str)
        {
            Lua.lua_pushstring(L, str);
        }
        public static void PushInt32(this IntPtr L, int value)
        {
            Lua.lua_pushinteger(L, value);
        }
        public static void PushDouble(this IntPtr L, double value)
        {
            Lua.lua_pushnumber(L, value);
        }
        public static void PushBool(this IntPtr L, bool value)
        {
            Lua.lua_pushboolean(L, value);
        }
        public static void PushNull(this IntPtr L)
        {
            Lua.lua_pushnil(L);
        }
        public static void PushList(this IntPtr L, IEnumerable<int> list)
        {
            Lua.lua_newtable(L);
            int i = 1;
            foreach (var v in list)
            {
                Push(L, v);
                Lua.lua_rawseti(L, -2, i++);
            }
        }
        public static void PushList(this IntPtr L, IEnumerable<string> list)
        {
            Lua.lua_newtable(L);
            int i = 1;
            foreach (var v in list)
            {
                Push(L, v);
                Lua.lua_rawseti(L, -2, i++);
            }
        }
        public static void PushList(this IntPtr L, IEnumerable<bool> list)
        {
            Lua.lua_newtable(L);
            int i = 1;
            foreach (var v in list)
            {
                Push(L, v);
                Lua.lua_rawseti(L, -2, i++);
            }
        }
        public static void PushList(this IntPtr L, IEnumerable<double> list)
        {
            Lua.lua_newtable(L);
            int i = 1;
            foreach (var v in list)
            {
                Push(L, v);
                Lua.lua_rawseti(L, -2, i++);
            }
        }
        public static void PushDictionary(this IntPtr L, Dictionary<string, int> dict)
        {
            Lua.lua_newtable(L);
            foreach (var pair in dict)
            {
                Push(L, pair.Value);
                Lua.lua_setfield(L, -2, pair.Key);
            }
        }
        public static void PushDictionary(this IntPtr L, Dictionary<string, string> dict)
        {
            Lua.lua_newtable(L);
            foreach (var pair in dict)
            {
                if (pair.Value != null)
                {
                    Push(L, pair.Value);
                }
                else
                {
                    L.PushNull();
                }

                Lua.lua_setfield(L, -2, pair.Key);
            }
        }
        public static void PushDictionary(this IntPtr L, Dictionary<string, bool> dict)
        {
            Lua.lua_newtable(L);
            foreach (var pair in dict)
            {
                Push(L, pair.Value);
                Lua.lua_setfield(L, -2, pair.Key);
            }
        }
        public static void PushDictionary(this IntPtr L, Dictionary<string, double> dict)
        {
            Lua.lua_newtable(L);
            foreach (var pair in dict)
            {
                Push(L, pair.Value);
                Lua.lua_setfield(L, -2, pair.Key);
            }
        }
        public static void PushDictionary(this IntPtr L, Dictionary<int, Tuple<string, string, string, int>> dict)
        {
            Lua.lua_newtable(L);
            foreach (var pair in dict)
            {
                Push(L, pair.Key);

                Lua.lua_newtable(L);

                Push(L, pair.Value.Item1);
                Lua.lua_rawseti(L, -2, 1);

                Push(L, pair.Value.Item2);
                Lua.lua_rawseti(L, -2, 2);

                Push(L, pair.Value.Item3);
                Lua.lua_rawseti(L, -2, 3);

                Push(L, pair.Value.Item4);
                Lua.lua_rawseti(L, -2, 4);

                Lua.lua_settable(L, -3);
            }
        }
        public static void PushXLObject(this IntPtr L, string typeName, IntPtr handle)
        {
            XLLuaRuntime.XLLRT_PushXLObject(L, typeName, handle);
        }
        public static void PushXLObject(this IntPtr L, string typeName, int handle)
        {
            XLLuaRuntime.XLLRT_PushXLObject(L, typeName, new IntPtr(handle));
        }
        public static void PushXLObjectList(this IntPtr L, string typeName, IEnumerable<IntPtr> handles)
        {
            Lua.lua_newtable(L);
            int i = 1;
            foreach (var handle in handles)
            {
                XLLuaRuntime.XLLRT_PushXLObject(L, typeName, handle);
                Lua.lua_rawseti(L, -2, i++);
            }
        }
        public static void PushXLObjectList(this IntPtr L, string typeName, IEnumerable<int> handles)
        {
            Lua.lua_newtable(L);
            int i = 1;
            foreach (var handle in handles)
            {
                XLLuaRuntime.XLLRT_PushXLObject(L, typeName, new IntPtr(handle));
                Lua.lua_rawseti(L, -2, i++);
            }
        }
        public static void PushBitmap(this IntPtr L, IntPtr handle)
        {
            XLGraphics.XLGP_PushBitmap(L, handle);
        }
        public static void Call(this IntPtr L, int arg, int ret)
        {
            XLLuaRuntime.XLLRT_LuaCall(L, arg, ret, null);
        }
        #endregion

        #region L Get Extension
        public static IntPtr GetHandle(this IntPtr L, int index)
        {
            return Lua.lua_touserdata(L, index);
        }
        public static string GetString(this IntPtr L, int index)
        {
            return Lua.lua_tostring(L, index);
        }
        public static int GetInt32(this IntPtr L, int index)
        {
            return Lua.lua_tointeger(L, index);
        }
        public static double GetDouble(this IntPtr L, int index)
        {
            return Lua.lua_tonumber(L, index);
        }
        public static bool GetBool(this IntPtr L, int index)
        {
            return Lua.lua_toboolean(L, index);
        }
        public static IntPtr GetBitmap(this IntPtr L, int index)
        {
            IntPtr bitmap = IntPtr.Zero;
            XLGraphics.XLGP_CheckBitmap(L, index, ref bitmap);
            return bitmap;
        }
        public static List<T> GetList<T>(this IntPtr L, int index, Func<IntPtr, int, T> getor)
        {
            var result = new List<T>();
            var size = Lua.lua_objlen(L, index);
            for (int i = 1; i <= size; i++)
            {
                int _lastTopIndex = Lua.lua_gettop(L);
                Lua.lua_rawgeti(L, index, i);
                var value = getor(L, -1);
                Lua.lua_settop(L, _lastTopIndex);
                result.Add(value);
            }
            return result;
        }
        public static Tuple<T1, T2> GetTuple<T1, T2>(
            this IntPtr L,
            int index,
            Func<IntPtr, int, T1> getor1,
            Func<IntPtr, int, T2> getor2)
        {
            int _lastTopIndex = Lua.lua_gettop(L);

            //Get First Item
            Lua.lua_rawgeti(L, index, 1);
            var value1 = getor1(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Second Item
            Lua.lua_rawgeti(L, index, 2);
            var value2 = getor2(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            return Tuple.Create(value1, value2);
        }
        public static Tuple<T1, T2, T3> GetTuple<T1, T2, T3>(
            this IntPtr L,
            int index,
            Func<IntPtr, int, T1> getor1,
            Func<IntPtr, int, T2> getor2,
            Func<IntPtr, int, T3> getor3)
        {
            int _lastTopIndex = Lua.lua_gettop(L);

            //Get First Item
            Lua.lua_rawgeti(L, index, 1);
            var value1 = getor1(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Second Item
            Lua.lua_rawgeti(L, index, 2);
            var value2 = getor2(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Thrird Item
            Lua.lua_rawgeti(L, index, 3);
            var value3 = getor3(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            return Tuple.Create(value1, value2, value3);
        }
        public static Tuple<T1, T2, T3, T4> GetTuple<T1, T2, T3, T4>(
            this IntPtr L,
            int index,
            Func<IntPtr, int, T1> getor1,
            Func<IntPtr, int, T2> getor2,
            Func<IntPtr, int, T3> getor3,
            Func<IntPtr, int, T4> getor4)
        {
            int _lastTopIndex = Lua.lua_gettop(L);

            //Get First Item
            Lua.lua_rawgeti(L, index, 1);
            var value1 = getor1(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Second Item
            Lua.lua_rawgeti(L, index, 2);
            var value2 = getor2(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Thrird Item
            Lua.lua_rawgeti(L, index, 3);
            var value3 = getor3(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Forth Item
            Lua.lua_rawgeti(L, index, 4);
            var value4 = getor4(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            return Tuple.Create(value1, value2, value3, value4);
        }
        public static Tuple<T1, T2, T3, T4, T5> GetTuple<T1, T2, T3, T4, T5>(
            this IntPtr L,
            int index,
            Func<IntPtr, int, T1> getor1,
            Func<IntPtr, int, T2> getor2,
            Func<IntPtr, int, T3> getor3,
            Func<IntPtr, int, T4> getor4,
            Func<IntPtr, int, T5> getor5)
        {
            int _lastTopIndex = Lua.lua_gettop(L);

            //Get First Item
            Lua.lua_rawgeti(L, index, 1);
            var value1 = getor1(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Second Item
            Lua.lua_rawgeti(L, index, 2);
            var value2 = getor2(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Thrird Item
            Lua.lua_rawgeti(L, index, 3);
            var value3 = getor3(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Forth Item
            Lua.lua_rawgeti(L, index, 4);
            var value4 = getor4(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Fifth Item
            Lua.lua_rawgeti(L, index, 5);
            var value5 = getor5(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            return Tuple.Create(value1, value2, value3, value4, value5);
        }
        public static Tuple<T1, T2, T3, T4, T5, T6> GetTuple<T1, T2, T3, T4, T5, T6>(
            this IntPtr L,
            int index,
            Func<IntPtr, int, T1> getor1,
            Func<IntPtr, int, T2> getor2,
            Func<IntPtr, int, T3> getor3,
            Func<IntPtr, int, T4> getor4,
            Func<IntPtr, int, T5> getor5,
            Func<IntPtr, int, T6> getor6)
        {
            int _lastTopIndex = Lua.lua_gettop(L);

            //Get First Item
            Lua.lua_rawgeti(L, index, 1);
            var value1 = getor1(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Second Item
            Lua.lua_rawgeti(L, index, 2);
            var value2 = getor2(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Thrird Item
            Lua.lua_rawgeti(L, index, 3);
            var value3 = getor3(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Forth Item
            Lua.lua_rawgeti(L, index, 4);
            var value4 = getor4(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Fifth Item
            Lua.lua_rawgeti(L, index, 5);
            var value5 = getor5(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            //Get Sixth Item
            Lua.lua_rawgeti(L, index, 6);
            var value6 = getor6(L, -1);
            Lua.lua_settop(L, _lastTopIndex);

            return Tuple.Create(value1, value2, value3, value4, value5, value6);
        }
        public static int GetFuncRef(this IntPtr L)
        {
            return Lua.luaL_ref(L, (int)LuaInnerIndex.LUA_REGISTRYINDEX);
        }
        public static void Pop(this IntPtr L, int n)
        {
            Lua.lua_settop(L, 0 - n - 1);
        }
        #endregion

        #region L Is Extension
        public static bool IsLuaFunction(this IntPtr L, int index)
        {
            return Lua.lua_type(L, index) == (int)LuaTypes.LUA_TFUNCTION;
        }
        #endregion

        #region L Create Event Extension
        public static Action ToAction(this IntPtr L, Action caller)
        {
            return LuaAction.Create(L, caller);
        }
        public static Action<T> ToAction<T>(this IntPtr L, Action<T> caller)
        {
            return LuaAction<T>.Create(L, caller);
        }
        public static Action<T1, T2> ToAction<T1, T2>(this IntPtr L, Action<T1, T2> caller)
        {
            return LuaAction<T1, T2>.Create(L, caller);
        }
        public static Action<T1, T2, T3> ToAction<T1, T2, T3>(this IntPtr L, Action<T1, T2, T3> caller)
        {
            return LuaAction<T1, T2, T3>.Create(L, caller);
        }
        public static Action<T1, T2, T3, T4> ToAction<T1, T2, T3, T4>(this IntPtr L, Action<T1, T2, T3, T4> caller)
        {
            return LuaAction<T1, T2, T3, T4>.Create(L, caller);
        }
        public static Action<T1, T2, T3, T4, T5> ToAction<T1, T2, T3, T4, T5>(this IntPtr L, Action<T1, T2, T3, T4, T5> caller)
        {
            return LuaAction<T1, T2, T3, T4, T5>.Create(L, caller);
        }
        #endregion

        #region L Func Extension
        public static Func<R> ToFunc<R>(this IntPtr L, Func<R> caller)
        {
            return LuaFunc<R>.Create(L, caller);
        }
        public static Func<T1, R> ToFunc<T1, R>(this IntPtr L, Func<T1, R> caller)
        {
            return LuaFunc<T1, R>.Create(L, caller);
        }
        public static Func<T1, T2, R> ToFunc<T1, T2, R>(this IntPtr L, Func<T1, T2, R> caller)
        {
            return LuaFunc<T1, T2, R>.Create(L, caller);
        }
        public static Func<T1, T2, T3, R> ToFunc<T1, T2, T3, R>(this IntPtr L, Func<T1, T2, T3, R> caller)
        {
            return LuaFunc<T1, T2, T3, R>.Create(L, caller);
        }
        public static Func<T1, T2, T3, T4, R> ToFunc<T1, T2, T3, T4, R>(this IntPtr L, Func<T1, T2, T3, T4, R> caller)
        {
            return LuaFunc<T1, T2, T3, T4, R>.Create(L, caller);
        }
        public static Func<T1, T2, T3, T4, T5, R> ToFunc<T1, T2, T3, T4, T5, R>(this IntPtr L, Func<T1, T2, T3, T4, T5, R> caller)
        {
            return LuaFunc<T1, T2, T3, T4, T5, R>.Create(L, caller);
        }
        public static Func<T1, T2, T3, T4, T5, T6, R> ToFunc<T1, T2, T3, T4, T5, T6, R>(this IntPtr L, Func<T1, T2, T3, T4, T5, T6, R> caller)
        {
            return LuaFunc<T1, T2, T3, T4, T5, T6, R>.Create(L, caller);
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, R> ToFunc<T1, T2, T3, T4, T5, T6, T7, R>(this IntPtr L, Func<T1, T2, T3, T4, T5, T6, T7, R> caller)
        {
            return LuaFunc<T1, T2, T3, T4, T5, T6, T7, R>.Create(L, caller);
        }
        #endregion

        #region Chunk Extension
        public static IntPtr CreateChunkFromModule(this string name, string file, string func)
        {
            IntPtr handle = IntPtr.Zero;
            XLLuaRuntime.XLLRT_CreateChunkFromModule(name, file, func, handle);
            return handle;
        }
        #endregion

        #region Runtime Extension
        public static void PrepareLuaChunk(this IntPtr luaRuntime, IntPtr luaChunk)
        {
            XLLuaRuntime.XLLRT_PrepareChunk(luaRuntime, luaChunk);
        }
        #endregion

        #region L Utilies
        public static void IF(this IntPtr L, Func<IntPtr, bool> filter, Action action)
        {
            if (filter(L))
            {
                action();
            }
        }
        #endregion
    }
}