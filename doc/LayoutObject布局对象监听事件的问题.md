## 问题
我用LayoutObject对象做外层布局，然后内层（子对象）中有UIObject对象和自定义控件对象。现在我需要监听外层LayoutObject是OnMouseEnter 和 OnMouseLeave 事件，然后对LayoutObject对象以及子对象隐藏和显示俩个事件监听都没问题。问题就出在，我鼠标进入LayoutObject后，然后在进入LayoutObject内部的自定义控件对象，这时居然触发了LayoutObject对象的OnMouseLeave事件但是还是在LayoutObject内部啊，只是进入了内部的子自定义控件而已。而鼠标进入LayoutObject内部的UIObject元对象就不会触发外层LayoutObject的OnMouseLeave事件。导致我现在只好，监听每个内部自定义控件的OnControlMouseEnter和OnControlMouseLeave事件，然后调用外层对应的回调函数。很麻烦，而且还会有些BUG，不知道有没有彻底的解决方案呢？还是我使用错误了？

## 解释
有两种办法

第一种，通过OnMouseLeave事件的回调函数的x y 参数来判断触发当前事件时，鼠标是否还停留在LayoutObject内部，如果还在内部，则不隐藏，如果已经移动到外部了，则隐藏该对象以及所有子对象：
```
function LayoutObj_MouseLeave(self,x,y)
    local left,top,right,bottom = self:GetObjPos()
    local width,height = right-left,bottom-top
    local self = self:GetOwnerControl()
    if x<0 or x>width or y<=0 or y>=height then
        self:SetVisible(false)
        self:SetChildrenVisible(false)
    end
end
```

第二种，通过监听OnControlMouseEnter和OnControlMouseLeave处理，会更简单。