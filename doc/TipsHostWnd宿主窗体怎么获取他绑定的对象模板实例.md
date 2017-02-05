## 问题
TipsHostWnd宿主窗体怎么获取他绑定的对象模板实例呢？

## 解释
设置模板和DelayPopup，这是auto模式的tipshostwnd，这种情况下，只有在真正创建窗口系统对象时候，才会对模板实例化，创建对象树，然后绑定到hostwnd上面，先后顺序如下：

1、对象模板实例化
2、创建一个对象树，把对象实例绑定到对象树
3、触发hostwnd的OnPreBindObjectTree事件
4、对象树绑定到hostwnd
5、触发hostwnd的OnBindObjectTree事件
6、创建hostwnd的系统窗口对象
7、hostwnd触发OnCreate等事件
8、tipshostwnd触发OnDelayPopup事件

上面的3-8阶段，都可以取到绑定到hostwnd的对象树，然后GetRootObject便可以取到模板实例化对象

## 代码
```
function ShowTipsWnd(object,x,y,tips)
    --显示tips
    local hostWndManager = XLGetObject("Xunlei.UIEngine.HostWndManager")
    local tipsHostWnd = hostWndManager:GetHostWnd("Bolt.Tips")

    --设置文本
    local function OnCreate(hostwnd)
        local objTree = hostwnd:GetBindUIObjectTree()
        if objTree == nil then return end
        local textObj = objTree:GetUIObject("tips.text")
        if textObj == nil then
            return
        end
        textObj:SetMultilineTextLimitWidth(130)
        local left,top,right,bottom = textObj:GetObjPos()

        --设置提示文本
        textObj:SetText(tips)

        --获取当前文本需要的区域
        local width,height = textObj:GetTextExtent()
        textObj:SetObjPos2(left,top,width,height)

        --获取背景对象
        local bkg = objTree:GetUIObject("bolt.tips.bkg")
        if bkg == nil then
            return
        end
        local left,top = bkg:GetObjPos()
        bkg:SetObjPos2(left,top,width+20,height+20)
    end   

    local cookie = tipsHostWnd:AttachListener("OnCreate",true,OnCreate)
    tipsHostWnd:DelayPopup(300) -- 延时300ms来popup，新的调用会覆盖老的调用
    tipsHostWnd:SetPositionByObject(x,y+10,object) 
end
```