using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ComicDown.UI.Core.Bolt
{
    public abstract class LuaBaseCoClass<T, LT> : LuaBase
    {
        #region 私有静态成员变量
        private static string _typeFullName;
        private static SortedList<string, LuaCFunction> _lua_functions = new SortedList<string, LuaCFunction>();
        private static LuaCFunction _lua_DeleteInstance = DeleteInstance;
        #endregion

        static LuaBaseCoClass()
        {
            CollectTypeInformations();
            CollectLuaClassMembers();
        }
        public static void Register()
        {
            //
            //0、得到Lua环境句柄
            //
            //var pNull = new IntPtr(0);
            var hEnviroment = XLLuaRuntime.XLLRT_GetEnv(null);

            //1、注册类型
            XLLuaRuntime.RegisterClass(hEnviroment, new XLRTClassInfo
            {
                ClassName = _typeFullName,
                FatherClassName = null,
                DeleteFunction = _lua_DeleteInstance,
                Methods = _lua_functions
            });
        }
        public static int GetIndex(IntPtr luaState)
        {
            var instancePointer = Lua.luaL_checkudata(luaState, 1, _typeFullName);
            var instanceIndex = new IntPtr(Marshal.ReadInt32(instancePointer));
            return instanceIndex.ToInt32();
        }

        private static int DeleteInstance(IntPtr luaState)
        {
            return 0;
        }
        private static void CollectTypeInformations()
        {
            var ttype = typeof(T);
            _typeFullName = ttype.FullName;
        }
        private static void CollectLuaClassMembers()
        {
            //收集需要注册到Lua环境的类成员函数信息
            var ttype = typeof(LT);
            foreach (MethodInfo method in ttype.GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
            {
                foreach (LuaClassMethodAttribute attribute in method.GetCustomAttributes(typeof(LuaClassMethodAttribute), true))
                {
                    var name = attribute.HasName ? attribute.Name : method.Name;
                    var classMemberFunction = Delegate.CreateDelegate(typeof(LuaCFunction), method) as LuaCFunction;
                    if (!_lua_functions.ContainsKey(name))
                    {
                        _lua_functions.Add(name, classMemberFunction);
                    }
                }
            }
        }
    }
}
