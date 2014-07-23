using System;

namespace BOLTStack
{
    public sealed class LuaAction : LuaBaseFunctor
    {
        private LuaAction(IntPtr luaState) : base(luaState) { }

        private Action Create(Action caller)
        {
            return () =>
            {
                BeginCall();
                caller();
                EndCall();
            };
        }

        public static Action Create(IntPtr luaState, Action caller)
        {
            return new LuaAction(luaState).Create(caller);
        }
    }
    public sealed class LuaAction<T> : LuaBaseFunctor
    {
        private LuaAction(IntPtr luaState) : base(luaState) { }

        private Action<T> Create(Action<T> caller)
        {
            return t =>
            {
                BeginCall();
                caller(t);
                EndCall();
            };
        }

        public static Action<T> Create(IntPtr luaState, Action<T> caller)
        {
            return new LuaAction<T>(luaState).Create(caller);
        }
    }
    public sealed class LuaAction<T1, T2> : LuaBaseFunctor
    {
        private LuaAction(IntPtr luaState) : base(luaState) { }

        private Action<T1, T2> Create(Action<T1, T2> caller)
        {
            return (t1, t2) =>
            {
                BeginCall();
                caller(t1, t2);
                EndCall();
            };
        }

        public static Action<T1, T2> Create(IntPtr luaState, Action<T1, T2> caller)
        {
            return new LuaAction<T1, T2>(luaState).Create(caller);
        }
    }
    public sealed class LuaAction<T1, T2, T3> : LuaBaseFunctor
    {
        private LuaAction(IntPtr luaState) : base(luaState) { }

        private Action<T1, T2, T3> Create(Action<T1, T2, T3> caller)
        {
            return (t1, t2, t3) =>
            {
                BeginCall();
                caller(t1, t2, t3);
                EndCall();
            };
        }

        public static Action<T1, T2, T3> Create(IntPtr luaState, Action<T1, T2, T3> caller)
        {
            return new LuaAction<T1, T2, T3>(luaState).Create(caller);
        }
    }
    public sealed class LuaAction<T1, T2, T3, T4> : LuaBaseFunctor
    {
        private LuaAction(IntPtr luaState) : base(luaState) { }

        private Action<T1, T2, T3, T4> Create(Action<T1, T2, T3, T4> caller)
        {
            return (t1, t2, t3, t4) =>
            {
                BeginCall();
                caller(t1, t2, t3, t4);
                EndCall();
            };
        }

        public static Action<T1, T2, T3, T4> Create(IntPtr luaState, Action<T1, T2, T3, T4> caller)
        {
            return new LuaAction<T1, T2, T3, T4>(luaState).Create(caller);
        }
    }
    public sealed class LuaAction<T1, T2, T3, T4, T5> : LuaBaseFunctor
    {
        private LuaAction(IntPtr luaState) : base(luaState) { }

        private Action<T1, T2, T3, T4, T5> Create(Action<T1, T2, T3, T4, T5> caller)
        {
            return (t1, t2, t3, t4, t5) =>
            {
                BeginCall();
                caller(t1, t2, t3, t4, t5);
                EndCall();
            };
        }

        public static Action<T1, T2, T3, T4, T5> Create(IntPtr luaState, Action<T1, T2, T3, T4, T5> caller)
        {
            return new LuaAction<T1, T2, T3, T4, T5>(luaState).Create(caller);
        }
    }
}
