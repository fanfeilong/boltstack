## 问题
RichEditObject 对象中输入纯文本，选中一段文本后键盘操作ctrl+c报错，说是调用什么初始化函数插入ImageObject对象，只选中图片对象键盘操作ctrl+c , OnGetObjectUniqueID事件正常， 并且实现了OnCreateObjectFromUniqueID，但是复制的图片对象没有被插入，而是复制了剪贴板中原有的内容(其他地方复制来的),只粘贴内容到 RichEditObject 是正常的，要怎么处理 RichEditObject 的 ctrl+c 和 ctrl+v 操作，包含插入的 LayoutObject？

## 解释
1. 在程序初始化的时候调用 OleInitialize(NULL)。
2. 相关事件处理里正确返回true,否则会认为你没有处理。而丢弃信息。
```
function Input_OnGetObjectUniqueID(self, obj)
	--your code
	return str,data,true
end
function Input_OnCreateObjectFromUniqueID(self, str, data)
	--your code
	return obj, true
end
```