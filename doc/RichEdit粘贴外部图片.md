## 问题
richedit 粘贴外部图片触发什么事件，怎么处理？

## 解释
1 响应richedit的键盘ctrl+v事件, 调c方法获得剪切板的图片, 转成xl的bitmap handle, 传回lua
2 lua创建一个imageobject对象, 指定大小, 用SetBitmpa方法设置c传过来的bitmap
3 用richedit的InsertObject方法, 插入创建的imageobject
