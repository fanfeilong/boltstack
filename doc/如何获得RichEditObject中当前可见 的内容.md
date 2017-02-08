## 问题
如何获得RichEditObject中当前可见的内容呢？

## 解释

1. 通过GetFirstVisibleLine来获得当前第一个可见行
2. 通过CharFromPos+对象莫行位置，来获取最后一行的charindex，然乎通过LineFromChar来获取最后一个可见行
3. 通过第一个可见行和最后一个可见行，利用GetLine来获取每一行内容，拼接为整个可见区的内容
