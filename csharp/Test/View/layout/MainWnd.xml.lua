------------------------------------------------
--//lua文件必须是UTF-8编码的(最好无BOM头)
--//BOLT测试代码
------------------------------------------------

local util = XLGetObject("UserQuery.Util")

function close_btn_OnLButtonDown(self,name)
	---创建内置动画的实例
	local aniFactory = XLGetObject("Xunlei.UIEngine.AnimationFactory")
	local alphaAni = aniFactory:CreateAnimation("AlphaChangeAnimation")
	alphaAni:SetTotalTime(700)
	alphaAni:SetKeyFrameAlpha(255,0)
	local owner = self:GetOwner()
	local icon = owner:GetUIObject("icon")
	alphaAni:BindRenderObj(icon) 
	owner:AddAnimation(alphaAni)
	alphaAni:Resume()
	
	local posAni = aniFactory:CreateAnimation("PosChangeAnimation")
	posAni:SetTotalTime(700)
	posAni:SetKeyFrameRect(45,100,45+60,100+60,45-30,100-30,45+60+30,100+60+30)
	posAni:BindLayoutObj(icon)
	owner:AddAnimation(posAni)
	posAni:Resume()

	local alphaAni2 = aniFactory:CreateAnimation("AlphaChangeAnimation")
	alphaAni2:SetTotalTime(700)
	alphaAni2:SetKeyFrameAlpha(255,0)
	local msg = owner:GetUIObject("msg")
	alphaAni2:BindRenderObj(msg)
	owner:AddAnimation(alphaAni2)
	alphaAni2:Resume()
	
	---定义动画结束的回调函数
	local function onAniFinish(self,oldState,newState)
		if newState == 4 then
            util:Quit()
		end
	end

	local posAni2 = aniFactory:CreateAnimation("PosChangeAnimation")
	posAni2:SetTotalTime(800)
	posAni2:BindLayoutObj(msg)
	posAni2:SetKeyFramePos(135,100,500,100)
	--当动画结束后，应用程序才退出
	posAni2:AttachListener(true,onAniFinish)
	owner:AddAnimation(posAni2)
	posAni2:Resume()
end

function OnInitControl(self)
	local objTree  = self:GetOwner()
	local editObj = objTree:GetUIObject("edit")
	editObj:SetText("nihao-------------------------------------")
end

function Title_InitControl(self)
	local titleObj = self
	titleObj:SetObjPos2("father.width/2-86/2","8","86","24")
	if titleObj.GetObjPosExp then
		util:WriteLine("titleObj:GetObjPosExp is not nil")
	end
	local x,y,w,h  = titleObj:GetObjPosExp()
	util:WriteLine(string.format("pos expression:x=%s,y=%s,w=%s,h=%s",x,y,w,h))
end

function MSG_OnMouseMove(self)
	self:SetTextFontResID ("msg.font.bold")
	self:SetCursorID ("IDC_HAND")
end

function MSG_OnMouseLeave(self)
	self:SetTextFontResID ("msg.font")
	self:SetCursorID ("IDC_ARROW")
end

function userdefine_btn_OnClick(self,name)
    -----------------------
    --BOLT使用C#注册的单例和工厂类示例：
    -----------------------
    util:WriteLine("开始计算加法：")
    util:Write("a+b=")

    local myClassFactory = XLGetObject("UserQuery.MyFactory.Factory")
	local myClass = myClassFactory:CreateInstance()
	myClass:AttachResultListener(function(result)
        util:Write(string.format("%s\n",result))
        util:WriteLine("result is "..result)
        util:WriteLine("计算结束！")
    end)
	myClass:Add(100,200)
end

function OnSelChange(self)
	local selBegin,selEnd=self:GetSel()
	
	local lineNO=self:LineFromChar(selBegin)
	util:WriteLine(selBegin..' '..lineNO)
	local lineBegin = self:LineIndex(lineNO)
	hang=thisWnd:GetObject("state"):GetObject("hang")
	hang:SetText(lineNO)
	lie=thisWnd:GetObject("state"):GetObject("lie")
	lie:SetText(selBegin - lineBegin)
end
