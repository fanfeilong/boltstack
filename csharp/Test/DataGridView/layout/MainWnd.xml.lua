--//--------------------------------------------
--// Copyright (c) Xunlei, Ltd. 2004-2014
--//--------------------------------------------
--//
--// Author     : fanfeilong
--// Create     : 2014-11-14
--// Histrory   :
--// Description: MainWnd.xml.lua，主界面脚本
--//
--//--------------------------------------------

local simulator = XLGetObject("Xunlei.Simulator.Object")

function GetWnd(obj)
    return obj:GetOwner():GetBindHostWnd()
end

function ShowInit(self)
    --local redObj = self:GetUIObject("main.logview")
    --redObj:AppendText("LogMonitor is Inited\r\n")
    
    local logPanelObj = self:GetUIObject("main.logview")
    for i=1,100 do
    logPanelObj:AppendItem(0,-1,"192.168.91.82",9999,"log monitor is inited."..i)
    end
end

function ShowLog(self,logNo,type,ip,port,log,userData)
    --local redObj = self:GetUIObject("main.logview")
    --local info = string.format("#%s,%s]@%s:%s#:   %s",ip,port,logNo,type,log)
    --redObj:AppendText(info)
    
    local logPanelObj = self:GetUIObject("main.logview")
    logPanelObj:AppendItem(logNo,type,ip,port,log)
end

function MainWnd_InitControl(self)
    --//在此初始化
end

function MainWnd_Create(self)
    local metatable = getmetatable(self)
    metatable.ShowInit = ShowInit
    metatable.ShowLog = ShowLog
    self:Center()
end

function MainWnd_Size(self)
    --//界面大小改变事件
end

function MainWnd_CloseButtonClick(self)
    GetWnd(self):Close()   
    simulator:Quit()                                  
end

function MainWnd_StartButtonClick(self)
    local objectTree = self:GetOwner()
    local peerIdTemplateObj = objectTree:GetUIObject("main.peerid.template")
    local peerCountObj = objectTree:GetUIObject("main.peerid.count")
    local downloaderPathObj = objectTree:GetUIObject("main.downloader.path")
    local taskUrlObj = objectTree:GetUIObject("main.task.url.path")
    local taskSaveObj = objectTree:GetUIObject("main.task.save.path")
    
    local peerIdTemplate = peerIdTemplateObj:GetText()
    local peerCount = peerCountObj:GetText()
    local downloaderPath = downloaderPathObj:GetText()
    local taskUrl = taskUrlObj:GetText()
    local taskSaveDir = taskSaveObj:GetText()
    
    simulator:Start(peerIdTemplate,peerCount,downloaderPath,taskUrl,taskSaveDir)
end

function MainWnd_StopButtonClick(self)
    simulator:Stop()
end

function MainWnd_ClearButtonClick(self)
    local logPanelObj = self:GetOwner():GetUIObject("main.logview")
    logPanelObj:Clear()
end