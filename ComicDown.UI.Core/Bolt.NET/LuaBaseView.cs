using System;

namespace ComicDown.UI.Core.Bolt
{
    public abstract class LuaBaseView
    {
        protected IntPtr GetLuaRuntime()
        {
            //var pNull = new IntPtr(0);
            var hEnviroment = XLLuaRuntime.XLLRT_GetEnv(null);
            var hRuntime = XLLuaRuntime.XLLRT_GetRuntime(hEnviroment, null);
            return hRuntime;
        }

        protected IntPtr GetLuaState()
        {
            //var pNull = new IntPtr(0);
            var hEnviroment = XLLuaRuntime.XLLRT_GetEnv(null);
            var hRuntime = XLLuaRuntime.XLLRT_GetRuntime(hEnviroment, null);
            var luaState = XLLuaRuntime.XLLRT_GetLuaState(hRuntime);
            return luaState;
        }

        protected IntPtr GetLuaChunk(string file, string func)
        {
            var luaChunck = "".CreateChunkFromModule(file, func);
            return luaChunck;
        }
    }
}
