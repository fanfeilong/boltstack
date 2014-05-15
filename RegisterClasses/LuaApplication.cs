using System;
using ComicDown.UI.Core.Bolt;

namespace HelloBolt.NET
{
    /// <summary>
    /// 注册单例对象给BOLT环境示例：
    /// 
    /// [功能]：
    /// 调用LuaApplication.Register()注册Application类的一个单例给BOLT环境 
    /// 
    /// [BOLT用例]：
    /// local app = XLGetObject("HelloBOlt.NET.Application")
    /// app:WriteLine("message")
    /// </summary>
    [LuaClass(ComicDown.UI.Core.Bolt.CreatePolicy.Singleton)]
    internal sealed class LuaApplication : LuaBaseClass<HelloBolt.NET.Application, LuaApplication>
    {
        [LuaClassMethod]
        private static int WriteLine(IntPtr L)
        {
            var instance = GetInstance(L);  //调用父类的GetInstance(L)方法获取Application单例对象
            var str = L.GetString(2);       //获取L栈的第2个元素，L.GetXXX是一系列扩展方法，方便获取Lua栈的元素
            instance.WriteLine(str);        //调用Application的WriteLine方法打印字符串
            return 0;
        }

        [LuaClassMethod]
        private static int Write(IntPtr L)
        {
            var instance = GetInstance(L); //调用父类的GetInstance(L)方法获取Application单例对象
            var str = L.GetString(2);      //获取L栈的第2个元素，L.GetXXX是一系列扩展方法，方便获取Lua栈的元素
            instance.Write(str);           //调用Application的Write方法打印字符串
            return 0;
        }

        [LuaClassMethod]
        private static int Quit(IntPtr L)
        {
            var instance = GetInstance(L);
            instance.Quit();
            return 0;
        }
    }
}
