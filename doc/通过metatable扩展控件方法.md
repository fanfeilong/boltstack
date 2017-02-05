1、通过这个方式给控件添加方法不需要在xml里添加Method，但为了跟在xml里通过Method添加到方法区别，
这里以小写字母开头：setControlVisible。

2、通过这个方式添加的方法，从控件外部亦可以调用，这破坏了封装性，不过在lua这种动态语言里，封装本来
就是弱约束的，主要是靠约定，比如此处我约定了小写开头的方法属于内部调用的，外部不要调用。

3、原理上，lua里面的userdata都可以通过metatable动态添加方法，是否要这么做，取决于你，以及风格的一致性
考虑，最好在同一个项目里保持一致性。

```
function XXControl_InitControl(self)  
  constructor(self)  
  init(self)
end

function constructor(XXConbtrol)
  local metatable = getmetatable(XXControl)

  ----------------------
  --成员函数，设置控件可见性
  ----------------------
  function metatable:setControlVisible(visible)
    local bkgObj=self:GetControlObject("bkg")
    bkgObj:SetVisible(visible)
    bkgObj:SetChildrenVisible(visible)
  end
end

function init(self)
  --此处可直接通过self:XXX调用在constructor里面对控件元表添加的新方法  
  self:setControlVisible(false)
end
```

