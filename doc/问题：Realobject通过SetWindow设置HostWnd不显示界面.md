## 问题
```lua
function OnBind(self,wnd,isbind)
	if not isbind then 
		return 
	end

	wnd:Move(100,100,880,620)
	local obj = self:GetUIObject("chat.wnd.content")
	local chatwnd = XLGetGlobal("Eoopen.Chat.Wnd")
	local chatwndins = chatwnd:Open(111,2)
	if chatwndins then
		obj:SetWindow(chatwndins:GetWndHandle())
	end
end
```

## 解释
窗口要是非层窗口模式才行
