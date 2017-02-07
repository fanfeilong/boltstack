## 问题
Tip没法和窗口联动？
```lua
local templateMananger = XLGetObject("Xunlei.UIEngine.TemplateManager")
local tipsHostWndTemplate = templateMananger:GetTemplate("ToolTipWnd","HostWndTemplate")
tipsHostWndTemplate:CreateInstance("ToolTipWnd")

local hostWndManager = XLGetObject("Xunlei.UIEngine.HostWndManager")
local tipsHostWnd = hostWndManager:GetHostWnd("ToolTipWnd")
local frameHostWnd = hostWndManager:GetHostWnd("MainWnd")

tipsHostWndelayPopup(1000)
local l,t,r,b = frameHostWnd:GetWindowRect()
tipsHostWnd:SetPositionByObject(l+300,t+93,frameHostWnd)
frameHostWnd:AddSyncWnd(tipsHostWnd:GetWndHandle(), {"position","visible"})
```

## 解释
tips本身的逻辑是失去焦点后就消失，所以不能用来做联动。用其它方法实现这个需求。