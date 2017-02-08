## 问题
写了一个仿metro的界面，设计了一个动画，这个动画会让方块旋转一定角度，但是动画执行完以后，方块立刻恢复原状，而不是保持在最后的旋转状态，如何解决？
```
aniFactory = nil
function OnBind(self)
AniFactory = XLGetObject("Xunlei.UIEngine.AnimationFactory")
OwnerTree = self:GetOwner() 
end

function MouseDown(self)
local AngleAni = AniFactory:CreateAnimation("AngleChangeAnimation")
AngleAni:BindRenderObj(self)
AngleAni:SetTotalTime(150)
AngleAni:SetKeyFrameAngle(0,0,0,15,0,0)
AngleAni:SetCentrePoint (0,67)
AngleAni:SetBlendMode("AntiAlias")
OwnerTree:AddAnimation(AngleAni)
AngleAni:Resume()
end
```

当鼠标按下时，事件处理函数会调用MouseDown函数从而执行动画


## 解释

```
local xlgraphic = XLGetObject("Xunlei.XLGraphic.Factory.Object")
local rotateTrans = xlgraphic:CreateRotateTrans(0, 0, 45)

local l,t,r,b = self:GetObjPos()
local width, height = r - l, b - t

local render = XLGetObject("Xunlei.UIEngine.RenderFactory")
local theBitmap = xlgraphic:CreateBitmap("ARGB32", width, height)

--要先render，也可以在动画结束的时候render（不确定，可以试试）不要在动画的过程中render，因为动画时self是不可见的，render不出来
render:RenderObject(self, theBitmap)

--得到目标宽高        
local endBitmapWidth, endBitmapHeight = rotateTrans:GetDestSize(theBitmap)
local endBitmap = xlgraphic:CreateBitmap("ARGB32", endBitmapWidth, endBitmapHeight)
rotateTrans:DoTransform(theBitmap, endBitmap)

local endTexture = xlgraphic:CreateTexture("Stretch")
endTexture:SetBitmap(endBitmap, 0)

local AniFactory = XLGetObject("Xunlei.UIEngine.AnimationFactory")
local AngleAni = AniFactory:CreateAnimation("AngleChangeAnimation")
AngleAni:BindRenderObj(self)
AngleAni:SetTotalTime(1000)
AngleAni:SetKeyFrameAngle(0, 0, 0, 0, 0, 45)
AngleAni:SetBlendMode("AntiAlias")
local tree = self:GetOwner()
tree:AddAnimation(AngleAni)

local left, top , newwidth, newheight = (l + r - endBitmapWidth)/2 , (t + b - endBitmapHeight)/2, endBitmapWidth, endBitmapHeight
local cookie = AngleAni:AttachListener(true, 
    function (ani, oldState, newState)
        if newState == 4 then
                self:SetObjPos2(left, top , newwidth, newheight)
                self:SetTexture(endTexture)
        end
    end)
AngleAni:Resume()
```