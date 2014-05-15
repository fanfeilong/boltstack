using System;
using ComicDown.UI.Core.Bolt;

namespace HelloBolt.NET
{
    /// <summary>
    /// 注册工厂类给BOLT环境示例
    /// 
    /// [功能]
    /// 调用LuaMyClass.Register()方法注册MyClass的工厂类给BOLT环境
    /// 
    /// [BOLT用例]
    /// local myClassFactory = XLGetObject("HelloBOlt.NET.MyClass.Factory") 
    /// local myClass = myClassFactory:CreateInstance()
    /// myClass:AttachResultListener(function(result)
    ///     --process result   
    /// end)
    ///	myClass:Add(100,200)
    /// </summary>
    [LuaClass(CreatePolicy.Factory)]
    internal sealed class LuaMyClass : LuaBaseClass<MyClass, LuaMyClass>
    {
        [LuaClassMethod]
        private static int Add(IntPtr L)
        {
            var instance = GetInstance(L);          //通过父类的GetInstance(L)方法获取BOLT环境下通过工厂类创建的MyClass实例
            var left = L.GetInt32(2);               //获取Lua栈的第2个元素
            var right = L.GetInt32(3);              //获取Lua栈的第3个元素
            var result = instance.Add(left, right); //调用MyClass的Add方法
            L.PushInt32(result);                    //将计算结果Push到Lua栈，L.PushXXX是一系列扩展方法，方便将C#数据Push到Lua栈
            //参考：ComicDown.UI.COre/Bolt.NET/LuaExtension.cs
            return 1;                               //1表示往Lua栈里Push了一个元素
        }

        [LuaClassMethod]
        private static int AttachResultListener(IntPtr L)
        {
            var instance = GetInstance(L);          //通过调用父类的GetInstance(L)方法获取BOLT环境下通过工厂类创建的MyClass实例           
            if (!L.IsLuaFunction(-1)) return 0;     //判断Lua栈的栈顶元素是否为function

            //
            //调用L.ToAction<T>(Action<T> caller)扩展方法将Lua栈顶function转为C#的Action<T>委托
            //其中caller用来具体将委托的参数Push到Lua栈，并通过L.Call(int arg,int ret)调用Lua的function
            //   arg表示Push到Lua栈的元素个数
            //   ret表示Lua function的返回值个数
            //
            var function = L.ToAction<int>(result =>
            {
                L.PushInt32(result);  //将resultPush到Lua栈 
                L.Call(1, 0);         //调用Lua的方法，1表示参数个数为1，0表示返回值个数为0
            });

            instance.OnAddFinish += function; //将转换后的委托添加到MyClass实例的OnAddFinish事件上
            return 0;                         //此处并没有往Lua栈里Push元素，故返回0
        }
    }
}


