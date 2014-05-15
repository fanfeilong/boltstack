using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ComicDown.UI.Core.Bolt
{
    public abstract class LuaBaseClass<T, LT> : LuaBase
        where T : new()
    {
        private static SortedList<int, T> _instancesDict;
        private static int _currentIndex;
        private static T _instance;

        private static XLLRTFuncGetObject _lua_GetClassFactoryInstance = GetClassFactoryInstance;
        private static LuaCFunction _lua_CreateInstance = CreateInstance;
        private static LuaCFunction _lua_DeleteInstance = DeleteInstance;

        private static CreatePolicy _createPolicy;
        private static string _typeFullName;
        private static string _typeFactoryClassName;
        private static string _typeFactoryObjectName;
        private static SortedList<string, LuaCFunction> _lua_functions = new SortedList<string, LuaCFunction>();

        static LuaBaseClass()
        {
            CollectTypeInformations();
            CollectLuaClassMembers();
        }
        public static void Register()
        {
            //
            //0、得到Lua环境句柄
            //
            var pNull = new IntPtr(0);
            var hEnviroment = XLLuaRuntime.XLLRT_GetEnv(null);

            switch (_createPolicy)
            {
                case CreatePolicy.Factory:
                    //1、注册类型工厂
                    XLLuaRuntime.RegisterGlobalObject(hEnviroment, new XLRTObjectInfo
                    {
                        ClassName = _typeFactoryClassName,
                        ObjectName = _typeFactoryObjectName,
                        UserData = pNull,
                        GetFunction = _lua_GetClassFactoryInstance,
                        Methods = new SortedList<string, LuaCFunction>{
                            {"CreateInstance",_lua_CreateInstance}
                        }
                    });

                    //2、注册类型
                    XLLuaRuntime.RegisterClass(hEnviroment, new XLRTClassInfo
                    {
                        ClassName = _typeFullName,
                        FatherClassName = null,
                        DeleteFunction = _lua_DeleteInstance,
                        Methods = _lua_functions
                    });
                    return;
                case CreatePolicy.Singleton:
                    //1、注册单例对象
                    XLLuaRuntime.RegisterGlobalObject(hEnviroment, new XLRTObjectInfo
                    {
                        ClassName = _typeFullName,
                        ObjectName = _typeFullName,
                        UserData = pNull,
                        GetFunction = _lua_GetClassFactoryInstance,
                        Methods = _lua_functions
                    });
                    return;
                default:
                    throw new Exception("UnSupport CreatePolicy!");
            }
        }
        public static T GetInstance(IntPtr luaState)
        {
            switch (_createPolicy)
            {
                case CreatePolicy.Factory:
                    var instancePointer = Lua.luaL_checkudata(luaState, 1, _typeFullName);
                    var instanceIndex = new IntPtr(Marshal.ReadInt32(instancePointer));
                    var instance = _instancesDict[instanceIndex.ToInt32()];
                    return instance;
                case CreatePolicy.Singleton:
                    return _instance;
                default:
                    return default(T);
            }
        }

        private static IntPtr GetClassFactoryInstance(IntPtr ud)
        {
            return new IntPtr(0);
        }
        private static int CreateInstance(IntPtr luaState)
        {
            _instancesDict[_currentIndex] = new T();
            XLLuaRuntime.XLLRT_PushXLObject(luaState, _typeFullName, new IntPtr(_currentIndex));
            _currentIndex++;
            return 1;
        }
        private static int DeleteInstance(IntPtr luaState)
        {
            var instancePointer = Lua.luaL_checkudata(luaState, 1, _typeFullName);
            var instanceIndex = new IntPtr(Marshal.ReadInt32(instancePointer));
            _instancesDict[instanceIndex.ToInt32()] = default(T);
            _instancesDict.Remove(instanceIndex.ToInt32());
            return 0;
        }
        private static void CollectTypeInformations()
        {
            var ttype = typeof(T);
            _typeFullName = ttype.FullName;
            _typeFactoryClassName = _typeFullName + ".Factory.Class";
            _typeFactoryObjectName = _typeFullName + ".Factory";
        }
        private static void CollectLuaClassMembers()
        {
            //
            //初始化_lua_functions
            //
            _lua_functions.Clear();

            //收集需要注册到Lua环境的类成员函数信息
            var ttype = typeof(LT);
            foreach (MethodInfo method in ttype.GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
            {
                foreach (LuaClassMethodAttribute attribute in method.GetCustomAttributes(typeof(LuaClassMethodAttribute), true))
                {
                    var name = attribute.HasName ? attribute.Name : method.Name;
                    var classMemberFunction = (LuaCFunction)Delegate.CreateDelegate(typeof(LuaCFunction), method);
                    if (!_lua_functions.ContainsKey(name))
                    {
                        _lua_functions.Add(name, classMemberFunction);
                    }
                }
            }

            //
            //搜集类型创建策略，默认为Factory
            //
            var singletonPolicy =
              ttype.GetCustomAttributes(typeof(LuaClassAttribute), true)
                   .Select(attr => attr as LuaClassAttribute)
                   .FirstOrDefault(attr => attr.CreatePolicy == CreatePolicy.Singleton);

            if (singletonPolicy == null)
            {
                _createPolicy = CreatePolicy.Factory;
                _instancesDict = new SortedList<int, T>();
            }
            else
            {
                _createPolicy = CreatePolicy.Singleton;
                _instance = new T();
            }
        }
    }
}