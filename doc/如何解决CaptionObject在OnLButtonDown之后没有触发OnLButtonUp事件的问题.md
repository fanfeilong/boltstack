## 问题
CaptionObject对象在OnLButtonDown之后移动鼠标(鼠标未松开)，松开鼠标后没有触发OnLButtonUp事件，请问是什么原因哦？

## 解释

这个是系统吞掉了，490及以后的版本可以使用hostwnd的OnEnterResizeMove和OnExitResizeMove消息来处理
这两个事件在hostwnd被拖着移动和改变大小时候会触发，分别在开始和结束时候触发，事件原型如下：

```
long result, bool handled, bool callnext OnEnterResizeMove(hostwnd self,  int type)
long result, bool handled, bool callnext OnExitResizeMove(hostwnd self,  int type)
```

其中type=0表示是resize事件，1表示是move事件