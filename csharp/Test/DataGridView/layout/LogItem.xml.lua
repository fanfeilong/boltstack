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

local dataKeys = {
    "id","type","ip","port","log"
}

function RenderText(self)
    local attr = self:GetAttribute()
    if attr.modalItem then
        for i,k in ipairs(dataKeys) do
            local objId = string.format("logitem.%s",k)
            local textObj = self:GetControlObject(objId)
            if textObj then
                textObj:SetText(attr.modalItem[k])
            end 
        end 
    end
end

function BindLog(self,modalItem)
    local attr = self:GetAttribute()
    attr.modalItem = modalItem
    if attr.enableupdate then
        RenderText(self)
    end
    return self
end

function SetBackColor(self,value,hold)
    local attr = self:GetAttribute()
    attr.holdcolor = value
    attr.hold=hold
    local bkgObj = self:GetControlObject("logitem.bkg")
    bkgObj:SetSrcColor(value)
    bkgObj:SetDestColor(value)
end

function GetModalItem(self)
    local attr = self:GetAttribute()
    return attr.modalItem
end

function SetStatus(self,value)
    local attr = self:GetAttribute()
    attr.status = value
    Update(self)
    return self
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

function SetBkgColor(attr,bkgObj,value)
    if attr.hold then
        bkgObj:SetSrcColor(attr.holdcolor)
        bkgObj:SetDestColor(attr.holdcolor)        
    else
        bkgObj:SetSrcColor(value)
        bkgObj:SetDestColor(value)
    end
end

function Update(self)
    local attr = self:GetAttribute()
    if attr.enableupdate~=true then
        return
    end
    
    local bkgObj = self:GetControlObject("logitem.bkg")
    RenderText(self)
    
    if attr.status=="normal" then
        bkgObj:SetEnable(true)
        bkgObj:SetChildrenEnable(true)
        SetBkgColor(attr,bkgObj,"system.white")
    elseif attr.status=="hover" then
        bkgObj:SetEnable(true)
        bkgObj:SetChildrenEnable(true)
        SetBkgColor(attr,bkgObj,"system.gray")
    elseif attr.status=="down" then
        bkgObj:SetEnable(true)
        bkgObj:SetChildrenEnable(true)
        SetBkgColor(attr,bkgObj,"system.blue")
    elseif attr.status=="disable" then
        bkgObj:SetEnable(false)
        bkgObj:SetChildrenEnable(false)
        SetBkgColor(attr,bkgObj,"system.darkgray")
    else
        XLMessageBox("Button Status Error!!")
    end  
end

function LogItem_InitControl(self)
    local attr = self:GetAttribute()
    attr.enableupdate = true
    attr.mouseEnter = false
    attr.status = "normal"
    Update(self)
end

function LogItem_VisibleChange(self,visible)
    self:SetChildrenVisible(visible)
end

function LogItem_EnableChange(self,enable)
    self:SetChildrenEnable(enable)
    if enable then
        SetStatus(self,"normal")
    else
        SetStatus(self,"disable")
    end
end

function LogItem_FocusChange(self)
    --//TODO
end

function LogItem_LButtonDown(self)
    SetStatus(self,"down")
end

function LogItem_LButtonUp(self)
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

function LogItem_ControlMouseEnter(self)
    local attr = self:GetAttribute()
    attr.mouseEnter = true
    if attr.status=="normal" then
        SetStatus(self,"hover")        
    end
end

function LogItem_ControlMouseLeave(self)
    local attr = self:GetAttribute()
    SetStatus(self,"normal")
    attr.mouseEnter = false
end
