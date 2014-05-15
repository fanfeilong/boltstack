using System;

namespace ComicDown.UI.Core.Bolt
{
    public sealed class LuaFunc<T> : LuaBaseFunctor
    {
        private LuaFunc(IntPtr luaState) : base(luaState) { }

        private Func<T> Create(Func<T> caller)
        {
            return () =>
            {
                BeginCall();
                T result = caller();
                EndCall();
                return result;
            };
        }

        public static Func<T> Create(IntPtr luaState, Func<T> caller)
        {
            var instance = new LuaFunc<T>(luaState);
            return instance.Create(caller);
        }
    }
    public sealed class LuaFunc<T1, T2> : LuaBaseFunctor
    {
        private LuaFunc(IntPtr luaState) : base(luaState) { }

        private Func<T1, T2> Create(Func<T1, T2> caller)
        {
            return t1 =>
            {
                BeginCall();
                var t2 = caller(t1);
                EndCall();
                return t2;
            };
        }

        public static Func<T1, T2> Create(IntPtr luaState, Func<T1, T2> caller)
        {
            return new LuaFunc<T1, T2>(luaState).Create(caller);
        }
    }
    public sealed class LuaFunc<T1, T2, T3> : LuaBaseFunctor
    {
        private LuaFunc(IntPtr luaState) : base(luaState) { }

        private Func<T1, T2, T3> Create(Func<T1, T2, T3> caller)
        {
            return (t1, t2) =>
            {
                BeginCall();
                var t3 = caller(t1, t2);
                EndCall();
                return t3;
            };
        }

        public static Func<T1, T2, T3> Create(IntPtr luaState, Func<T1, T2, T3> caller)
        {
            return new LuaFunc<T1, T2, T3>(luaState).Create(caller);
        }
    }
    public sealed class LuaFunc<T1, T2, T3, T4> : LuaBaseFunctor
    {
        private LuaFunc(IntPtr luaState) : base(luaState) { }

        private Func<T1, T2, T3, T4> Create(Func<T1, T2, T3, T4> caller)
        {
            return (t1, t2, t3) =>
            {
                BeginCall();
                var t4 = caller(t1, t2, t3);
                EndCall();
                return t4;
            };
        }

        public static Func<T1, T2, T3, T4> Create(IntPtr luaState, Func<T1, T2, T3, T4> caller)
        {
            return new LuaFunc<T1, T2, T3, T4>(luaState).Create(caller);
        }
    }
    public sealed class LuaFunc<T1, T2, T3, T4, T5> : LuaBaseFunctor
    {
        private LuaFunc(IntPtr luaState) : base(luaState) { }

        private Func<T1, T2, T3, T4, T5> Create(Func<T1, T2, T3, T4, T5> caller)
        {
            return (t1, t2, t3, t4) =>
            {
                BeginCall();
                var t5 = caller(t1, t2, t3, t4);
                EndCall();
                return t5;
            };
        }

        public static Func<T1, T2, T3, T4, T5> Create(IntPtr luaState, Func<T1, T2, T3, T4, T5> caller)
        {
            return new LuaFunc<T1, T2, T3, T4, T5>(luaState).Create(caller);
        }
    }
    public sealed class LuaFunc<T1, T2, T3, T4, T5, T6> : LuaBaseFunctor
    {
        private LuaFunc(IntPtr luaState) : base(luaState) { }

        private Func<T1, T2, T3, T4, T5, T6> Create(Func<T1, T2, T3, T4, T5, T6> caller)
        {
            return (t1, t2, t3, t4, t5) =>
            {
                BeginCall();
                var t6 = caller(t1, t2, t3, t4, t5);
                EndCall();
                return t6;
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6> Create(IntPtr luaState, Func<T1, T2, T3, T4, T5, T6> caller)
        {
            return new LuaFunc<T1, T2, T3, T4, T5, T6>(luaState).Create(caller);
        }
    }
    public sealed class LuaFunc<T1, T2, T3, T4, T5, T6, T7> : LuaBaseFunctor
    {
        private LuaFunc(IntPtr luaState) : base(luaState) { }

        private Func<T1, T2, T3, T4, T5, T6, T7> Create(Func<T1, T2, T3, T4, T5, T6, T7> caller)
        {
            return (t1, t2, t3, t4, t5, t6) =>
            {
                BeginCall();
                var t7 = caller(t1, t2, t3, t4, t5, t6);
                EndCall();
                return t7;
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7> Create(IntPtr luaState, Func<T1, T2, T3, T4, T5, T6, T7> caller)
        {
            return new LuaFunc<T1, T2, T3, T4, T5, T6, T7>(luaState).Create(caller);
        }
    }
    public sealed class LuaFunc<T1, T2, T3, T4, T5, T6, T7, T8> : LuaBaseFunctor
    {
        private LuaFunc(IntPtr luaState) : base(luaState) { }

        private Func<T1, T2, T3, T4, T5, T6, T7, T8> Create(Func<T1, T2, T3, T4, T5, T6, T7, T8> caller)
        {
            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                BeginCall();
                var t8 = caller(t1, t2, t3, t4, t5, t6, t7);
                EndCall();
                return t8;
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8> Create(IntPtr luaState, Func<T1, T2, T3, T4, T5, T6, T7, T8> caller)
        {
            return new LuaFunc<T1, T2, T3, T4, T5, T6, T7, T8>(luaState).Create(caller);
        }
    }
}
