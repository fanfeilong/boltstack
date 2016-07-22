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

local xarManager = XLGetObject("Xunlei.UIEngine.XARManager")
local xarFactory = xarManager:GetXARFactory()

function createViewItem(self,attr,k)
	local viewItemName=string.format("logitem.%s",k)
	local viewItem = xarFactory:CreateUIObject(viewItemName,"LogItem")
    local attr = self:GetAttribute()
	viewItem:AttachListener("OnClick",true,function(viewItem_,eventName)
		--//TODO
		local modalItem = viewItem:GetModalItem()
		attr.highlight.ip =  modalItem.ip
		attr.highlight.port = modalItem.port
		Update(self)
	end)
	return viewItem
end

function AppendItem(self,logNo,type,ip,port,log)
    local attr = self:GetAttribute()
    local modalItem = nil
    --[[
    for i,m in ipairs(attr.modalItems) do
        if m.ip==ip and m.port==port then
            modalItem = m
        end
    end
    --]]
    
    if not modalItem then
        modalItem = {}
        modalItem.ip = ip;
        modalItem.port = port
        table.insert(attr.modalItems,modalItem)
    end

    modalItem.id = logNo
    modalItem.type = type
    modalItem.log = log
    
    
    --//# Geometry Point 1
    attr.currentBottom = #(attr.modalItems)
    if attr.currentBottom>attr.viewItemCapacity then
       attr.currentTop = attr.currentBottom-attr.viewItemCapacity 
    end
    
    if attr.enableupdate then
        Update(self)
    end
end

function Clear(self)
    local attr = self:GetAttribute()
    attr.modalItems = {}
    attr.currentTop = 1
    attr.currentBottom = 0
    attr.highlight={}
    attr.highlight.ip = -1
    attr.highlight.port = -1
    attr.enableUpdate = true
    
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

function Update(self)
    local attr = self:GetAttribute()
    if attr.enableupdate~=true then
        return
    end
    
    
    --// ChangeVisible
    for i=1,#attr.viewItems do
        local viewItem = attr.viewItems[i]
        viewItem:SetVisible(false)
    end
    
    if attr.currentTop>attr.currentBottom then
        return
    end
    
    --// Render Here
    local bkgObj = self:GetControlObject("logpanel.bkg")
    local l,t,r,b = self:GetObjPos()
    local w,h=r-l,b-t
    local size = #(attr.viewItems)
    local capacity = attr.viewItemCapacity
    for i=attr.currentTop,attr.currentBottom do
        local modalItem = attr.modalItems[i]
        if not modalItem then
            break
        end 
        --// Get Or Create ViewItem
        local viewItemIndex = i-attr.currentTop+1
        local viewItem = nil
        if viewItemIndex<=size then
            viewItem = attr.viewItems[viewItemIndex]
        else
            if size>capacity then
                XLMessageBox("not reached")
                return     
            else
                --// Create a new viewItem
                viewItem = createViewItem(self,attr,viewItemIndex) 
                bkgObj:AddChild(viewItem)  
                table.insert(attr.viewItems,viewItem) 
                                
                --// Set viewItem ObjPos
                local left = attr.itemStyle.margin
                local top  = (attr.itemStyle.height+attr.itemStyle.margin)*(i-1)
                local width = w-attr.itemStyle.margin*2
                local height =attr.itemStyle.height
                viewItem:SetObjPos2(left,top,width,height)
            end    
        end
        
        --// Bind modalItem to viewItem
        
        viewItem:BindLog(modalItem)
        if modalItem.ip==attr.highlight.ip and modalItem.port==attr.highlight.port then
            viewItem:SetBackColor(attr.itemStyle.highlightcolor,true)
        else
            viewItem:SetBackColor(attr.itemStyle.normalcolor,false)
        end
        viewItem:SetVisible(true)
    end   
end

function LogPanel_InitControl(self)
    local l,t,r,b = self:GetObjPos()
    local w,h=r-l,b-t

    local attr = self:GetAttribute()
    attr.enableupdate = true
    attr.mouseEnter = false
    attr.modalItems = {}
    attr.viewItems  = {}
    attr.itemStyle = {}
    attr.itemStyle.margin = 3
    attr.itemStyle.height = 24
    attr.itemStyle.highlightcolor="system.lightorange"
    attr.itemStyle.normalcolor="system.white"
    attr.viewItemCapacity = math.floor(h/attr.itemStyle.height)
    attr.currentTop = 1
    attr.currentBottom = 0
    attr.highlight={}
    attr.highlight.ip = -1
    attr.highlight.port = -1
    Update(self)
end

function LogPanel_VisibleChange(self,visible)
    self:SetChildrenVisible(visible)
end

function LogPanel_EnableChange(self,enable)
    self:SetChildrenEnable(enable)
end

function LogPanel_FocusChange(self)
    --//TODO
end

function LogPanel_LButtonDown(self)
    --//TODO
end

function LogPanel_LButtonUp(self)
    --//TODO
end

function LogPanel_ControlMouseEnter(self)
    local attr = self:GetAttribute()
    attr.mouseEnter = true
    --//TODO
end

function LogPanel_ControlMouseLeave(self)
    local attr = self:GetAttribute()
    attr.mouseEnter = false
    --//TODO
end

function LogPanel_ControlMouseWheel(self,x,y,distance)
    local attr = self:GetAttribute()
    if distance>0 then
        if attr.currentTop>1  then
            attr.currentTop = attr.currentTop-1
            attr.currentBottom = attr.currentBottom-1
        else
            --Ignore
        end
    elseif distance<0 then
        local newBottom = attr.currentBottom+1
        if newBottom>attr.viewItemCapacity and  newBottom<=#(attr.modalItems)+2 then
           attr.currentTop = newBottom-attr.viewItemCapacity
           attr.currentBottom = newBottom 
        end
    end
    if attr.enableupdate then
        Update(self)
    end
end
