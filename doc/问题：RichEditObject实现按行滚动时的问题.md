## 问题
最近在写RichEdit按行滚动时遇到了些问题，其中行高为参数，便于适应各种情况，目前已经适应自写List和大多数情况下的RichEdit的按行翻页滚动（除了RichEdit的OnMouseWheel）
需求涉及面：

1. RichEdit滚动按钮拖动事件时，需要按行；（√）
2. RichEdit滚动条的各对象的OnMouseWheel事件时，需要按行；（√）
3. RichEdit滚动条的UP，DOWN按钮点击事件时，需要按行；（√）
4. RichEdit的OnMouseWheel事件时，需要按行；（X）

现在已经1,2,3都已完成实现，只有第四种情况，同样的逻辑，同样的代码下,RichEdit所展示的位置却不一样，调试跟踪后发现在同样的ScrollPos情况下，显示也并不相同。
为了解决这个问题，做了如下尝试：

1. 挂在RichEdit的OnmouseWheel事件，进行直接编码，计算滚动像素，效果比较小，效果有，可是并不是按照预期进行的。
2. 将RichEdit的OnMouseWheel事件，重转向给其子对象ScrollBar进行处理，消息重转向成功，可是效果基本同上。

## 解释
重定向到滚动条以后，在滚动条的mousewheel事件里面，把返回值的handled设置为true，也就是要return 0,true,true，这样会拦截RichEditObject的对MouseWheel事件的默认处理