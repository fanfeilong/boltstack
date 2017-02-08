## 问题
比如：
```
local aniFactory = XLGetObject("Xunlei.UIEngine.AnimationFactory")
local posAnim = aniFactory:CreateAnimation("AnimaitionGif.ani") 
```
如何获取动画的ID，我要的是可以通过方法直接获取到我创建的动画实例，比如获取到已经CreateAnimation("AnimaitionGif.ani") 
的实例，“AnimationGif.ani”。

## 解释
这个需要自己保存一下已创建动画ID