```lua
function OnInitControl(self)
    --自己用c++封装个操作ie的对象
    local MyIE = XLGetObject("xxxxxxx")
    --创建一个ie窗口 当然是在c++里面实现 返回ie的窗口句柄
    local ie_handler = MyIE:Create("http://www.baidu.com")
    
    local function onsize(...)
        local args = {...}
        local width = args[8]-args[6]
        local height = args[9]-args[7]
        MyIE:SetSize(ie_handler,width,height)
    end
    
    local function onclose(...)
        MyIE:Delete(ie_handler)
    end
    
    --绑定监听 如 改变大小 关闭什么的
    self:AttachListener("OnPosChange",onsize)
    self:AttachListener("OnDestroy",onclose)
    
    --设置句柄到self self应该是对应的realobject
    self:SetWindow(ie_handler)
end
```