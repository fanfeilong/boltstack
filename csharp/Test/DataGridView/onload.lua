--//--------------------------------------------
--// Copyright (c) Xunlei, Ltd. 2004-2014
--//--------------------------------------------
--//
--// Author     : fanfeilong
--// Create     : 2014-11-14
--// Histrory   :
--// Description: 迅雷P2P调度器控制界面
--//
--//--------------------------------------------

-----------------------
--//[函数说明]
--//加载子模块
-----------------------
function LoadModule(module,entry)
    local pos1, pos2 = string.find(__document, "downloadersimulator")
    local packagePath = string.sub(__document, 1, pos2);
    local modulePath   = string.format("%s/%s",packagePath,module); 
	local md =XLLoadModule(modulePath)
	local entryFunc = md[entry]
	if not entryFunc then
	    local message = string.format("%s,missing entry function:%s",modulePath,entry)
	    XLMessageBox(message)
	    return
	end
	entryFunc()
end

-----------------------
--//[函数说明]
--//显示主窗口
-----------------------
function Run()
    local hostWndFactory =  XLGetGlobal("HostWndFactory")        

    --// 创建窗体
    local mainWnd  = hostWndFactory:CreateHostWnd("main.wnd","MainWnd")
    if not mainWnd then return end
    
    --// 设置全局变量
    XLSetGlobal("MainWnd.Instance",mainWnd)
    
    --// 设置委托
    local simulator = XLGetObject("Xunlei.Simulator.Object")
    simulator:SetDelegate(function()
        --// On XLDS Inited
        local mainWnd = XLGetGlobal("MainWnd.Instance")
        mainWnd:ShowInit() 
    end,function(logNo,type,ip,port,log,userData)
        --// Show Log
        local mainWnd = XLGetGlobal("MainWnd.Instance")
        mainWnd:ShowLog(logNo,type,ip,port,log,userData)    
    end,nil)

    --// 创建对象树
    local mainTree = hostWndFactory:CreateObjectTree("main.tree","MainTree")
    if not mainTree then return end

    --// 绑定对象树到窗体
    mainWnd:BindUIObjectTree(mainTree)
    mainWnd:Create()
end


function Main()
    --// 1. 加载子模块
    local modules =
    {
        "factory/HostWndFactory.lua",    
    }
    for i,module in ipairs(modules) do
        LoadModule(module,"Register")
    end

    --// 2. 显示主窗口
    Run()
end

Main()

