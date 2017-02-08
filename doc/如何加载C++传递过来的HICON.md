## 问题
如何加载C++传递过来的HICON，传递HICON时是以pushinteger传递的吗？

## 解释
你要传递到什么地方？假如只是你们程序自己的接口接收，那么什么类型取决于你们协定，假如要传给imageobject来显示，那么必须转换成XL_BITMAP_HANDLE才可以，可以通过XGP库的接口来完成转换。

hIcon是你们自己加载的HICON句柄
```
XLGP_ICON_HANDLE hXLIcon = XLGP_CreateIconFromHandle(hIcon);
XL_BITMAP_HANDLE hBitmap = XLGP_IconGetBitmap(hXLIcon);

// 在合适的地方，push位图到lua里面，lua里面可以直接设置到imageobject来显示
XLUE_PushBitmap(hBitmap)

XL_ReleaseBitmap(hBitmap)
XLGP_ReleaseIcon(hXLIcon)
```