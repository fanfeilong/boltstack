using System;
using BOLTStack;

namespace UserQuery
{

    [LuaClass(CreatePolicy.Factory)]
    internal sealed partial class MyClass : LuaBaseClass<MyClass, MyClass>
    {
        [LuaClassMethod]
        private static int Add(IntPtr L)
        {
            var instance = GetInstance(L);          
            var left = L.GetInt32(2);               
            var right = L.GetInt32(3);             
            var result = instance.Add(left, right);
            L.PushInt32(result); 
            return 1; 
        }

        [LuaClassMethod]
        private static int AttachResultListener(IntPtr L)
        {
            var instance = GetInstance(L);          
            if (!L.IsLuaFunction(-1)) return 0;
            var function = L.ToAction<int>(result =>
            {
                L.PushInt32(result);
                L.Call(1, 0);
            });

            instance.OnAddFinish += function;
            return 0;
        }
    }

    partial class MyClass
    {
        public event Action<int> OnAddFinish;
        public int Add(int lhs, int rhs)
        {
            int result = lhs + rhs;
            if (OnAddFinish != null)
            {
                OnAddFinish(result);
            }
            return result;
        }
    }
}