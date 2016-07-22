using System;
using BOLTStack;

namespace UserQuery
{
    [LuaClass(BOLTStack.CreatePolicy.Singleton)]
    internal sealed partial class Utils : LuaBaseClass<Utils, Utils>
    {
        [LuaClassMethod]
        private static int WriteLine(IntPtr L)
        {
            var instance = GetInstance(L); 
            var str = L.GetString(2);
            instance.WriteLine(str);
            return 0;
        }

        [LuaClassMethod]
        private static int Write(IntPtr L)
        {
            var instance = GetInstance(L);
            var str = L.GetString(2);
            instance.Write(str);
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

    partial class Utils
    {
        private void WriteLine(string str)
        {
            Console.WriteLine(str);
        }

        private void Write(string str)
        {
            Console.Write(str);
        }

        private void Quit()
        {
            Environment.Exit(0);
        }
    }
}
