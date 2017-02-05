## 问题
如何让模态窗口在初始化的时候获取焦点？

## 解释
可以在OnCreate里面，使用AsynCall对窗口或者窗口上需要的元对象设置焦点

```
local function SetFocus()
	obj:SetFocus(true)
end
AsynCall(SetFocus)
```