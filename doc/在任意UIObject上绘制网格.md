BOLT设计了XML布局和Lua脚本，灵活定制可以满足各种产品需求

```lua
function renderCoord(obj)
        local left,top,right,bottom = obj:GetObjPos()
        local width = right-left
        local height = bottom-top
        local cellSize = 22
        local rowCount = math.floor(height/cellSize)
        local columnCount = math.floor(width/cellSize)
        local xarManager = XLGetObject("Xunlei.UIEngine.XARManager")
        local xarFactory = xarManager:GetXARFactory()
        for i=1,rowCount do
                local line = xarFactory:CreateUIObject(string.format("x.%s",i),"BlueLineH")
                line:SetObjPos(0,i*cellSize,width,i*cellSize+1)
                obj:AddChild(line)
        end
        for j=1,columnCount do
                local line = xarFactory:CreateUIObject(string.format("y.%s",j),"BlueLineV")
                line:SetObjPos(j*cellSize,0,j*cellSize+1,height)
                obj:AddChild(line)
        end
end
```

```xml
<control class="BlueLineH">
        <objtemplate>
            <children>
                <obj id="bkg" class="LineObject">
                    <attr>
                        <width>father.width</width>
                        <height>father.height</height>
                        <pen>pen.solid</pen>
                        <color>system.lightblue</color>
                        <srcpt>0,0</srcpt>
                        <destpt>width,0</destpt>
                        <zorder>100000</zorder>
                    </attr>
                </obj>
            </children>
        </objtemplate>
</control>
<control class="BlueLineV">
        <objtemplate>
            <children>
                <obj id="bkg" class="LineObject">
                    <attr>
                        <width>father.width</width>
                        <height>father.height</height>
                        <pen>pen.solid</pen>
                        <color>system.lightblue</color>
                        <srcpt>0,0</srcpt>
                        <destpt>0,height</destpt>
                        <zorder>100000</zorder>
                    </attr>
                </obj>
            </children>
        </objtemplate>
</control>
```