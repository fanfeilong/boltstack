--//--------------------------------------------
--// Copyright (c) Xunlei, Ltd. 2004-2014
--//--------------------------------------------
--//
--// Author     : fanfeilong
--// Create     : 2014-11-14
--// Histrory   :
--// Description: Button.xml.lua，按钮脚本
--//
--//--------------------------------------------

function SetText(self,value)
    local attr = self:GetAttribute()
    attr.text=value
    if attr.enableupdate then
        local textObj = self:GetControlObject("button.text")
        textObj:SetText(attr.text)
    end
    return self
end
  
function GetText(self)
    local attr = self:GetAttribute()
    return attr.text
end

function SetTextPos(self,left,top)
    local attr = self:GetAttribute()
    attr.textleft = left
    attr.texttop = top
    if attr.enableupdate then
        --//TODO
    end
    return self
end

function GetTextPos(self)
    local attr = self:GetAttribute()
    return attr.textleft,attr.texttop
end

function SetStatus(self,value)
    local attr = self:GetAttribute()
    attr.status = value
    Update(self)
    return self
end

function GetStatus(self)
    local attr = self:GetAttribute()
    return attr.status
end

function BeginUpdate(self)
    local attr = self:GetAttribute()
    attr.enableupdate = false
    return self
end

function EndUpdate(self)
    local attr = self:GetAttribute()
    attr.enableupdate = true
    Update(self)
    return self
end

function Update(self)
    local attr = self:GetAttribute()
    if attr.enableupdate~=true then
        return
    end
    
    local bkgObj = self:GetControlObject("button.bkg")
    local textObj = self:GetControlObject("button.text")
    textObj:SetText(attr.text)
    
    if attr.status=="normal" then
        self:SetEnable(true)
        self:SetChildrenEnable(true)
        bkgObj:SetSrcColor(attr.normalbackcolor)
        bkgObj:SetDestColor(attr.normalbackcolor)
        textObj:SetTextColorResID(attr.normalfrontcolor)
        textObj:SetTextFontResID(attr.normalfont)
    elseif attr.status=="hover" then
        self:SetEnable(true)
        self:SetChildrenEnable(true)
        bkgObj:SetSrcColor(attr.hoverbackcolor)
        bkgObj:SetDestColor(attr.hoverbackcolor)
        textObj:SetTextColorResID(attr.hoverfrontcolor)
        textObj:SetTextFontResID(attr.hoverfont)
    elseif attr.status=="down" then
        self:SetEnable(true)
        self:SetChildrenEnable(true)
        bkgObj:SetSrcColor(attr.downbackcolor)
        bkgObj:SetDestColor(attr.downbackcolor)
        textObj:SetTextColorResID(attr.downfrontcolor)
        textObj:SetTextFontResID(attr.downfont)
    elseif attr.status=="disable" then
        self:SetEnable(false)
        self:SetChildrenEnable(false)
        bkgObj:SetSrcColor(attr.disablebackcolor)
        bkgObj:SetDestColor(attr.disablebackcolor)
        textObj:SetTextColorResID(attr.disablefrontcolor)
        textObj:SetTextFontResID(attr.disablefont)
    else
        XLMessageBox("Button Status Error!!")
    end  
end

function Button_InitControl(self)
    local attr = self:GetAttribute()
    attr.enableupdate = true
    attr.mouseEnter = false
    Update(self)
end

function Button_VisibleChange(self,visible)
    self:SetChildrenVisible(visible)
end

function Button_EnableChange(self,enable)
    self:SetChildrenEnable(enable)
    if enable then
        SetStatus(self,"normal")
    else
        SetStatus(self,"disable")
    end
end

function Button_FocusChange(self)
    --//TODO
end

function Button_LButtonDown(self)
    SetStatus(self,"down")
end

function Button_LButtonUp(self)
    local attr = self:GetAttribute()
    local onclick = false
    if attr.status=="down" then
        onclick = true        
    end
    if attr.mouseEnter then
        SetStatus(self,"hover") 
    else
        SetStatus(self,"normal")       
    end
    if onclick then
        self:FireExtEvent("OnClick")
    end
end

function Button_ControlMouseEnter(self)
    local attr = self:GetAttribute()
    attr.mouseEnter = true
    if attr.status=="normal" then
        SetStatus(self,"hover")        
    end
end

function Button_ControlMouseLeave(self)
    local attr = self:GetAttribute()
    SetStatus(self,"normal")
    attr.mouseEnter = false
end
