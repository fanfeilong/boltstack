using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BOLTStack
{
    [StructLayout(LayoutKind.Sequential)]
    public struct XLLRTGlobalAPI
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public String funName;
        public LuaCFunction func;
        public UInt32 permission;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XLLRTObject
    {
        public XLLRTFuncGetObject pfnGetObject;

        public IntPtr userData;

        [MarshalAs(UnmanagedType.LPStr)]
        public String objName;

        [MarshalAs(UnmanagedType.LPStr)]
        public String className;

        public IntPtr memberFunctions;//XLLRTGlobalAPI数组

        public UInt32 permission;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XLLRTClass
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public String className;

        [MarshalAs(UnmanagedType.LPStr)]
        public String fahterClassName;

        public IntPtr MemberFunctions;//XLLRTGlobalAPI数组

        public UInt32 permission;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XL_LRT_ERROR_STACK
    {
        public IntPtr logs;
        public XLLRTErrorHash hash;
    }

    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate int fnLuaErrorHandle(
        IntPtr luaState,
        [MarshalAs(UnmanagedType.LPWStr)]
        string pExtInfo,
        [MarshalAs(UnmanagedType.LPWStr)]
        string luaErrorString,
        IntPtr pStackInfo
    );

    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate long fnGlobalSetCallback(
        IntPtr luaState,
        string globalObjID,
        IntPtr udata
    );

    [StructLayout(LayoutKind.Sequential)]
    public struct XLLRTErrorHash
    {
        public UInt16 top;
        public UInt32 topsix;
        public UInt32 all;
    }

    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    public delegate int LuaCFunction(IntPtr luaState);

    public delegate IntPtr XLLRTFuncGetObject(IntPtr ud);

    public sealed class XLRTObjectInfo
    {
        public string ClassName { get; set; }
        public string ObjectName { get; set; }
        public IntPtr UserData { get; set; }
        public XLLRTFuncGetObject GetFunction { get; set; }
        public SortedList<string, LuaCFunction> Methods { get; set; }
    }
    public sealed class XLRTClassInfo
    {
        public string ClassName { get; set; }
        public string FatherClassName { get; set; }
        public LuaCFunction DeleteFunction { get; set; }
        public SortedList<string, LuaCFunction> Methods { get; set; }
    }
    public sealed class XLRTMethodsInfo
    {
        public SortedList<string, LuaCFunction> Methods { get; set; }
    }

    public static class XLLuaRuntime
    {
        #region CHUNK类型定义
        public const int XLLRT_HISTORY_TYPE_ERROR = 0;
        public const int XLLRT_CHUNK_TYPE_STRING = 0;
        public const int XLLRT_CHUNK_TYPE_FILE = 1;
        public const int XLLRT_CHUNK_TYPE_MODULE = 2;
        #endregion

        #region DEBUG参数定义
        public const int XLLRT_DEBUG_TYPE_HOOK = 0;
        public const int XLLRT_DEBUG_TYPE_NOHOOK = 1;
        public const int XLLRT_DEBUG_TYPE_DUMPSTATE = 2;

        public const int XLLRT_DEBUG_MAX_LOG_LEN = 1024;
        public const int XLLRT_DEBUG_CALL_LOG_COUNT = 1024;
        public const int XLLRT_DEBUG_STACK_LOG_COUNT = 100;
        public const int XLLRT_DEBUG_MAX_NAME_LEN = 128;
        #endregion

        #region 错误码定义
        public const int XLLRT_RESULT_SUCCESS = 0;
        public const int XLLRT_RESULT_ENV_EXIST = 1;
        public const int XLLRT_RESULT_ENV_NOT_FOUND = 2;
        public const int XLLRT_RESULT_ENV_INVALID = 3;
        public const int XLLRT_RESULT_CHUNK_EXIST = 4;
        public const int XLLRT_RESULT_CHUNK_NOT_FOUND = 5;
        public const int XLLRT_RESULT_CHUNK_CODE_LENGTH_NOT_ALLOW = 6;
        public const int XLLRT_RESULT_CHUNK_COMPLIED = 7;
        public const int XLLRT_RESULT_CANNOT_CREATE_LUASTATE = 8;
        public const int XLLRT_RESULT_CHUNK_INVALID = 9;
        public const int XLLRT_RESULT_CHUNK_NEED_ID = 10;
        public const int XLLRT_RESULT_CHUNK_COMPLIE_ERROR = 11;
        public const int XLLRT_RESULT_RUNTIME_RUN_ERROR = 12;
        public const int XLLRT_RESULT_RUNTIME_NOT_READY = 13;
        public const int XLLRT_RESULT_RUNTIME_HAVE_INIT = 14;
        public const int XLLRT_RESULT_RUNTIME_INVALID = 15;
        public const int XLLRT_RESULT_RUNTIME_EXIST = 16;
        public const int XLLRT_RESULT_RUNTIME_NOT_FOUND = 17;
        public const int XLLRT_RESULT_FUNCTION_EXIST = 18;
        public const int XLLRT_RESULT_FUNCTION_NOT_FOUND = 19;
        public const int XLLRT_RESULT_GLOBAL_OBJ_INVALID = 20;
        public const int XLLRT_RESULT_GLOBAL_OBJ_EXIST = 21;
        public const int XLLRT_RESULT_GLOBAL_OBJ_NOT_FOUND = 22;
        public const int XLLRT_RESULT_CLASS_NOT_FOUND = 23;
        public const int XLLRT_RESULT_FILE_NOT_FOUND = 24;
        public const int XLLRT_RESULT_CLASS_EXIST = 25;
        public const int XLLRT_RESULT_NO_PERMISSION = 26;
        public const int XLLRT_RESULT_CHUNK_MOUDLE_RUN = 27;
        public const int XLLRT_RESULT_DEBUG_BUFNOTENOUGH = 30;
        public const int XLLRT_RESULT_PARAM_INVALID = 31;
        public const int XLLRT_RESULT_NOT_IMPL = 32;
        public const int XLLRT_RESULT_TYPEERROR_NOTUSERDATA = 33;
        public const int XLLRT_RESULT_TYPEERROR_NOMETATABLE = 34;
        public const int XLLRT_RESULT_TYPEERROR_NOCLASSNAME = 35;
        public const int XLLRT_RESULT_TYPEERROR_CLASSMISMATCH = 36;
        #endregion

        #region XLLRT API
        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_GetVersion();

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_CreateEnv([MarshalAs(UnmanagedType.LPWStr)] string pstrEnvName);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DestroyEnv([MarshalAs(UnmanagedType.LPWStr)] string pEnvName);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_GetEnv([MarshalAs(UnmanagedType.LPWStr)] string pstrEnvName);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_AddRefEnv(IntPtr hEnv);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_ReleaseEnv(IntPtr hEnv);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_RegisterGlobalAPI(IntPtr hEnv, XLLRTGlobalAPI theAPI);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_RemoveGlobalAPI(IntPtr hEnv, string theAPIName);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool XLLRT_IsGlobalAPIRegistered(IntPtr hEnv, string theAPIName);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_RegisterGlobalObj(IntPtr hEnv, XLLRTObject theObj);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_RemoveGlobalObj(IntPtr hEnv, string objName);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_RegisterGlobalSetCallback(IntPtr hEnv, fnGlobalSetCallback pfnCallback, IntPtr udata);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_IsGlobalObjRegistered(IntPtr hEnv, string objName);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_RegisterClass(IntPtr hEnv, string className, IntPtr MemberFunctions, string fahterClassName, UInt32 permission);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_UnRegisterClass(IntPtr hEnv, string className);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DoRegisterClass(string className, IntPtr luaState);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool XLLRT_IsClassRegistered(IntPtr hEnv, string className);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool XLLRT_IsDerivedClass(IntPtr hEnv, string lpDerivedClass, string lpBaseClass);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_BeginEnumGlobalAPI(IntPtr hEnv);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool XLLRT_GetNextGlobalAPI(IntPtr hEnum, IntPtr lpGlobalAPI);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_BeginEnumGlobalObject(IntPtr hEnv);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool XLLRT_GetNextGlobalObject(IntPtr hEnum, IntPtr lpGlobalObj);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_BeginEnumGlobalClass(IntPtr hEnv);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool XLLRT_GetNextGlobalClass(IntPtr hEnum, IntPtr lpGlobalClass);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool XLLRT_EndEnum(IntPtr hEnum);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern string XLLRT_GetXLObjectClass(IntPtr luaState, int index);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_PushXLObject(IntPtr luaState, string className, IntPtr pRealObj);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_GetXLObject(IntPtr luaState, int index, string className, IntPtr lplpObj);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_CheckXLObject(IntPtr luaState, int index, string className, IntPtr lplpObj);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_CreateRunTime(IntPtr hEnv, string id, UInt32 permission);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DestroyRunTime(IntPtr hEnv, string pID);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_GetRuntime(IntPtr hEnv, string pRuntimeID);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_GetRuntimePermission(IntPtr hRuntime);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_ReleaseRunTime(IntPtr hRunTime);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_AddRefRunTime(IntPtr hRunTime);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_GetOwnerEnv(IntPtr hRunTime);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern string XLLRT_GetRuntimeID(IntPtr hRuntime);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_GetLuaState(IntPtr hRunTime);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_GetRuntimeFromLuaState(IntPtr luaState);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int XLLRT_GetAllLuaState(IntPtr luaState, int nCount);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static extern string XLLRT_AddLoadLuaFile([MarshalAs(UnmanagedType.LPWStr)] string lpLuaFile);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int XLLRT_GetLoadLuaFileCount();

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_GetLoadLuaFileByIndex(int nIndex, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder wstrFilePath, int nLen);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_AddRefChunk(IntPtr hChunk);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_ReleaseChunk(IntPtr hChunk);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static extern string XLLRT_GetChunkName(IntPtr hChunk);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_GetChunkType(IntPtr hChunk);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_CreateChunk([MarshalAs(UnmanagedType.LPWStr)] string pstrName, string pCodeBuffer, UInt32 len, IntPtr pResult);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_CreateChunkFromFile([MarshalAs(UnmanagedType.LPWStr)] string pstrName, [MarshalAs(UnmanagedType.LPWStr)] string path, IntPtr pResult);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_CreateChunkFromModule([MarshalAs(UnmanagedType.LPWStr)] string pstrName, [MarshalAs(UnmanagedType.LPWStr)] string modulePath, string func, IntPtr pResult);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_RunChunk(IntPtr hRunTime, IntPtr hChunk);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_PrepareChunk(IntPtr hRunTime, IntPtr hChunk);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_LuaCall(IntPtr luaState, int args, int results, [MarshalAs(UnmanagedType.LPWStr)] string contextInfo);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_ErrorHandle(fnLuaErrorHandle pfnErrorHandle);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern string XLLRT_GetLastError(IntPtr luaState);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_SetGlobalAPIHook(IntPtr hEnv, string name, LuaCFunction func, [MarshalAs(UnmanagedType.Bool)] bool pre);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool XLLRT_RemoveGlobalAPIHook(IntPtr hEnv, string name, IntPtr hook);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_SetGlobalObjectFunctionHook(IntPtr hEnv, string objName, string name, LuaCFunction func, [MarshalAs(UnmanagedType.Bool)] bool pre);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool XLLRT_RemoveGlobalObjectFunctionHook(IntPtr hEnv, string objName, string name, IntPtr hook);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr XLLRT_SetClassFunctionHook(IntPtr hEnv, string className, string name, LuaCFunction func, [MarshalAs(UnmanagedType.Bool)] bool pre);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool XLLRT_RemoveClassFunctionHook(IntPtr hEnv, string className, string name, IntPtr hook);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugLogsPopNextLog(string pLog, IntPtr logs);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugMemPoolGetLogs(IntPtr MemPool, IntPtr logs);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugCreateStackMemPool(IntPtr pMemPool);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugDesroyStackMemPool(IntPtr MemPool);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugCreateLogs(IntPtr pLogs);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugDestroyLogs(IntPtr logs);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugInit(IntPtr DebugeeName, UInt32 dwDbgType);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugGetDebugeeName(StringBuilder pBuf);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugGetType(IntPtr pType);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugGetCallLog(IntPtr pMemPool);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugGetStackLog(IntPtr pMemPool);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugGetProcessCallLog(IntPtr DebugeeName, IntPtr pMemPool);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugGetProcessStackLog(IntPtr DebugeeName, IntPtr pMemPool);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugOutputLuaStack(IntPtr pState, IntPtr MemPool);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugGetCurState(IntPtr ppState);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugGetProcessCurState(IntPtr DebugeeName, IntPtr ppState);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugDestroyState(IntPtr pState);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int XLLRT_DebugGetDumpList(IntPtr CallbackParam, IntPtr CallbackInput, IntPtr CallbackOutput);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DebugGetStateFromDump(IntPtr pDumpData, IntPtr pMemList, IntPtr ppState);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern LuaCFunction XLLRT_GetFunctionAddress(LuaCFunction lpFun);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_Stat(UInt32 type);

        [DllImport("XLUE.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 XLLRT_DumpTable(IntPtr hRuntime, UInt32 tableRef);


        #endregion

        #region 注册对象和类辅助方法
        private static readonly int GLOBAL_API_SIZE = Marshal.SizeOf(typeof(XLLRTGlobalAPI));
        public static void RegisterGlobalObject(IntPtr hEnviroment, XLRTObjectInfo info)
        {
            int i = 0;
            var pClassMember = Marshal.AllocHGlobal(GLOBAL_API_SIZE * (info.Methods.Count + 1));
            foreach (var methodInfo in info.Methods)
            {
                var pClassMemberGlobalAPI = new XLLRTGlobalAPI
                {
                    permission = 0,
                    funName = methodInfo.Key,
                    func = methodInfo.Value
                };
                var pos = pClassMember.ToInt32() + i * GLOBAL_API_SIZE;
                var pClassMemberPos = new IntPtr(pos);
                Marshal.StructureToPtr(pClassMemberGlobalAPI, pClassMemberPos, false);
                i++;
            }
            var pNullMemberGlobalAPI = new XLLRTGlobalAPI
            {
                permission = 0,
                funName = null,
                func = null
            };
            var pNullMemberPos = new IntPtr(pClassMember.ToInt32() + info.Methods.Count * GLOBAL_API_SIZE);
            Marshal.StructureToPtr(pNullMemberGlobalAPI, pNullMemberPos, false);

            var factoryObject = new XLLRTObject
            {
                className = info.ClassName,
                objName = info.ObjectName,
                userData = info.UserData,
                pfnGetObject = info.GetFunction,
                memberFunctions = pClassMember
            };
            XLLuaRuntime.XLLRT_RegisterGlobalObj(hEnviroment, factoryObject);
            Marshal.FreeHGlobal(pClassMember);
        }
        public static void RegisterClass(IntPtr hEnviroment, XLRTClassInfo info)
        {
            int i = 0;
            var pClassMember = Marshal.AllocHGlobal(GLOBAL_API_SIZE * (info.Methods.Count + 2));
            foreach (var methodInfo in info.Methods)
            {
                var pClassMemberGlobalAPI = new XLLRTGlobalAPI
                {
                    permission = 0,
                    funName = methodInfo.Key,
                    func = methodInfo.Value
                };
                var pClassMemberPos = new IntPtr(pClassMember.ToInt32() + i * GLOBAL_API_SIZE);
                Marshal.StructureToPtr(pClassMemberGlobalAPI, pClassMemberPos, false);
                i++;
            }
            var pDeleteMemberGlobalAPI = new XLLRTGlobalAPI
            {
                permission = 0,
                funName = "__gc",
                func = info.DeleteFunction
            };
            var pDeleteMemberPos = new IntPtr(pClassMember.ToInt32() + info.Methods.Count * GLOBAL_API_SIZE);
            Marshal.StructureToPtr(pDeleteMemberGlobalAPI, pDeleteMemberPos, false);

            var pNullMemberGlobalAPI = new XLLRTGlobalAPI
            {
                permission = 0,
                funName = null,
                func = null
            };
            var pNullMemberPos = new IntPtr(pClassMember.ToInt32() + (info.Methods.Count + 1) * GLOBAL_API_SIZE);
            Marshal.StructureToPtr(pNullMemberGlobalAPI, pNullMemberPos, false);

            XLLuaRuntime.XLLRT_RegisterClass(hEnviroment, info.ClassName, pClassMember, info.FatherClassName, 0);
            Marshal.FreeHGlobal(pClassMember);
        }
        public static void RegisterGlobalAPI(IntPtr hEnviroment, XLRTMethodsInfo info)
        {
            foreach (var methodInfo in info.Methods)
            {
                var pClassMemberGlobalAPI = new XLLRTGlobalAPI
                {
                    permission = 0,
                    funName = methodInfo.Key,
                    func = methodInfo.Value
                };
                XLLRT_RegisterGlobalAPI(hEnviroment, pClassMemberGlobalAPI);
                Console.WriteLine("Return {0}", XLLRT_IsGlobalAPIRegistered(hEnviroment, methodInfo.Key));
                if (XLLRT_IsGlobalAPIRegistered(hEnviroment, methodInfo.Key))
                {
                    Console.WriteLine("Func {0} has been registed!", methodInfo.Key);
                }
            }
        }
        #endregion

        #region 全局方法迭代器
        public static void PrintAllGlobalMethods()
        {
            var hEnviroment = XLLuaRuntime.XLLRT_GetEnv(null);
            var hEnum = XLLRT_BeginEnumGlobalAPI(hEnviroment);
            var luaAPI = new XLLRTGlobalAPI();
            var luaAPIPtr = Marshal.AllocHGlobal(GLOBAL_API_SIZE);
            while (XLLRT_GetNextGlobalAPI(hEnum, luaAPIPtr))
            {
                luaAPI = (XLLRTGlobalAPI)Marshal.PtrToStructure(luaAPIPtr, typeof(XLLRTGlobalAPI));
                Console.WriteLine("name={0}", luaAPI.funName);
            }
            XLLRT_EndEnum(hEnum);
        }
        public static void PrintAllGlobalClasses()
        {
            var hEnviroment = XLLuaRuntime.XLLRT_GetEnv(null);
            var hEnum = XLLRT_BeginEnumGlobalClass(hEnviroment);
            var luaClass = new XLLRTClass();
            var luaClassPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(XLLRTClass)));
            while (XLLRT_GetNextGlobalClass(hEnum, luaClassPtr))
            {
                luaClass = (XLLRTClass)Marshal.PtrToStructure(luaClassPtr, typeof(XLLRTClass));
                if (!luaClass.className.Contains("Tree")) continue;
                Console.WriteLine("name={0}", luaClass.className);

                var luaAPIPtr = luaClass.MemberFunctions;
                var pClassMemberPos = luaAPIPtr;
                var luaAPI = (XLLRTGlobalAPI)Marshal.PtrToStructure(pClassMemberPos, typeof(XLLRTGlobalAPI));
                var i = 1;
                while (luaAPI.funName != null)
                {
                    Console.WriteLine("name={0}", luaAPI.funName);

                    var pos = luaAPIPtr.ToInt32() + i * GLOBAL_API_SIZE;
                    pClassMemberPos = new IntPtr(pos);
                    luaAPI = (XLLRTGlobalAPI)Marshal.PtrToStructure(pClassMemberPos, typeof(XLLRTGlobalAPI));
                    i++;
                }
                break;
            }
            XLLRT_EndEnum(hEnum);
        }
        #endregion
    }
}
