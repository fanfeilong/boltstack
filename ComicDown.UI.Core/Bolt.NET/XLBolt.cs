using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ComicDown.UI.Core.Bolt
{
    internal sealed class Hook { }

    /// <summary>
    /// Xunlei Bolt .NET Wrapper
    /// </summary>
    public sealed class XLBolt
    {
        private const string XLBOLT_INVOKE_ACTION = "XLBOLT_INVOKE_ACTION";
        private static readonly object Locker = new object();
        private static readonly fnLuaErrorHandle ErrorHandle = BoltErrorHandle;
        private static bool _isMessageLoopBegin;
        private static uint _invokeActionMessage;

        private readonly BackGroundForm _backGroundForm;
        private readonly List<IMessageFilter> _messageFilters;

        private Queue<Action> _invokeActions;
        private uint _threadID;

        private static bool IsMessageLoopBegin
        {
            get
            {
                lock (Locker)
                {
                    return _isMessageLoopBegin;
                }
            }
            set
            {
                lock (Locker)
                {
                    _isMessageLoopBegin = value;
                }
            }
        }
        public int ManagerdThreadID { get; private set; }
        public BackGroundForm HostForm
        {
            get
            {
                return _backGroundForm;
            }
        }

        private static XLBolt _instance;
        public static XLBolt Instance()
        {
            return _instance ?? (_instance = new XLBolt());
        }
        private XLBolt()
        {
            _messageFilters = new List<IMessageFilter>();
            _invokeActions = new Queue<Action>();
            _backGroundForm = new BackGroundForm();
            _backGroundForm.TimerTick += InvokeActions;
        }


        public void Run(string xarSearchPath, string xar, Action callback, bool initXGP = false)
        {
            Initialization(initXGP);
            XLLuaRuntime.XLLRT_ErrorHandle(ErrorHandle);
            AddXarSearchPath(xarSearchPath);
            callback();
            LoadXar(xar);
            MessageLoop();
        }
        public void Invoke(Action action)
        {
            Monitor.Enter(this);
            try
            {
                if (_invokeActions == null)
                {
                    _invokeActions = new Queue<System.Action>();
                }
            }
            finally
            {
                Monitor.Exit(this);
            }

            Monitor.Enter(_invokeActions);
            try
            {
                if (_invokeActionMessage == 0)
                {
                    _invokeActionMessage = Win32.RegisterWindowMessage(XLBOLT_INVOKE_ACTION);
                }
                _invokeActions.Enqueue(action);
            }
            finally
            {
                Monitor.Exit(_invokeActions);
            }

            if (IsMessageLoopBegin)
            {
                Win32.PostThreadMessage(_threadID, _invokeActionMessage, UIntPtr.Zero, IntPtr.Zero);
            }
        }
        public void AddMessageFilter(IMessageFilter filter)
        {
            _messageFilters.Add(filter);
        }
        public void RemoveMessageFilter(IMessageFilter filter)
        {
            _messageFilters.Remove(filter);
        }

        private static void Initialization(bool initXGP = false)
        {
            XLUE.XL_InitGraphicLib(0);
            XLUE.XL_SetFreeTypeEnabled(1);

            XLUE.XLUE_InitLoader(0);
            if (!initXGP) return;

            var size = Marshal.SizeOf(typeof(XLGraphics.tagXLGraphicPlusParam));
            var ptr = Marshal.AllocHGlobal(size);
            var strcut = new XLGraphics.tagXLGraphicPlusParam
            {
                bInitLua = true
            };
            Marshal.StructureToPtr(strcut, ptr, false);
            XLGraphics.XLGP_InitGraphicPlus(ptr);
        }
        private static void AddXarSearchPath(String newFolderPath)
        {
            XLUE.XLUE_AddXARSearchPath(newFolderPath);
        }
        private static void LoadXar(String xarName)
        {
            XLUE.XLUE_LoadXAR(xarName);
        }
        private static int BoltErrorHandle(IntPtr luaState, string pExtInfo, string luaErrorString, IntPtr pStackInfo)
        {
            DumpError(luaState, pExtInfo, luaErrorString);
            return 0;
        }

        private void MessageLoop()
        {
            _threadID = (uint)Win32.GetCurrentThreadId();
            ManagerdThreadID = Thread.CurrentThread.ManagedThreadId;
            var msg = new Win32.MSG();
            Win32.PeekMessage(ref msg, IntPtr.Zero, Win32.WM_USER, Win32.WM_USER, Win32.PM_NOREMOVE);
            IsMessageLoopBegin = true;
            while (Win32.GetMessage(ref msg, IntPtr.Zero, 0, 0))
            {
                if (msg.message == Win32.WM_QUIT)
                    return;
                if (ThreadMessageProc(ref msg))
                {
                    continue;
                }
                Win32.TranslateMessage(ref msg);

                var csMsg = new Message
                {
                    HWnd = msg.hwnd,
                    LParam = msg.lParam,
                    WParam = msg.wParam,
                    Msg = msg.message
                };
                foreach (var messageFilter in _messageFilters)
                {

                    messageFilter.PreFilterMessage(ref csMsg);
                }
                Win32.DispatchMessage(ref msg);
            }
        }
        private bool ThreadMessageProc(ref Win32.MSG msg)
        {
            if (msg.message == _invokeActionMessage)
            {
                InvokeActions();
                return true;
            }
            return false;
        }
        private void InvokeActions()
        {
            if (_invokeActions == null) return;
            if (Monitor.TryEnter(_invokeActions))
            {
                try
                {
                    if (_invokeActions.Count == 0)
                    {
                        return;
                    }

                    Action action = null;
                    if (_invokeActions.Count > 0)
                    {
                        action = _invokeActions.Dequeue();
                    }
                    while (action != null)
                    {
                        Monitor.Exit(_invokeActions);
                        try
                        {
                            action();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Monitor.Enter(_invokeActions);
                        action = _invokeActions.Count > 0 ? _invokeActions.Dequeue() : null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Monitor.Exit(_invokeActions);
                }
            }
        }

        [Conditional("DEBUG")]
        private static void DumpError(IntPtr luaState, string pExtInfo, string luaErrorString)
        {
            var e = new StringBuilder();
            var stackBuffer = new StringBuilder(1024);
            XLUE.XLUE_GetLuaStack(luaState, stackBuffer, 1024);
            e.Append("\n──────────────────────────────────")
             .Append("★BOLT错误：")
             .Append("──────────────────────────────────")
             .Append(pExtInfo)
             .Append(luaErrorString ?? "未知")
             .Append("─────────\n")
             .Append(stackBuffer)
             .Append("─────────\n");
            DBG.DumpError(e.ToString());
        }
    }
}
