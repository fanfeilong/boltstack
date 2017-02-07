有不少同学反映在BOLT里想做一些绘制（比如实现一个画图版），这个功能用组合原子uiobject的方法不好实现，于是问我们bolt里有没有绘制函数。
Bolt里确实有一些绘制函数，但因为BOLT本身的图形库是为贴图优化的，优先考虑性能而不精度，所以用来做自由绘制，在一些边界条件下效果会有问题。
我们推荐的解决方法是：使用ＧＤＩ＋进行绘制，基本思路是这样的
１.在bolt里用一个ImageObject作为画布
2.在C++里得到这个ImageObject,然后用GDI+创建一个内存GDI的位图，在这个GDI位图上绘制，画好后吧这个GDI位图转化为XL_BITMAP，最后把这个XL_BITMAP设置到ImageObject里。

下面代码片段来自光影魔术手
-- lua中
```
local imageProcessor = XLGetObject("NeoViewer.LuaImageProcessor")
local clipBitmap = imageProcessor:DrawThumbnailViewLayer(width, height, attr.ClipL, attr.ClipT, attr.ClipW, attr.ClipH)
if clipBitmap then 
        local layerObj = self:GetControlObject("client.view.Layer")
        layerObj:SetBitmap(clipBitmap)
end
```

-- C++中
```
XL_BITMAP_HANDLE CImageProcessor::DrawThumbnailViewLayer(int nTotalWidth, int nTotalHeight, int nLeftPos, int nTopPos, int nRectWidth, int nRectHeight){
        if (nTotalWidth < 1 || nTotalHeight < 1){
                return NULL;
        }

        if (nLeftPos < 0 || nLeftPos >= nTotalWidth || nTopPos < 0 || nTopPos >= nTotalHeight){
                return NULL;
        }

        if (nLeftPos + nRectWidth > nTotalWidth){
                nRectWidth = nTotalWidth - nLeftPos;
        }
        if (nTopPos + nRectHeight > nTotalHeight){
                nRectHeight = nTotalHeight - nRectHeight;
        }

        RectF rect(0, 0, nTotalWidth, nTotalHeight);
        RectF clipRect(nLeftPos, nTopPos, nRectWidth, nRectHeight);

        // 创建一个位图
        XL_BITMAP_HANDLE hBitmap = XL_CreateBitmap(XLGRAPHIC_CT_ARGB32, nTotalWidth, nTotalHeight);
        if (hBitmap == NULL){
                return NULL;
        }

        XLBitmapInfo bmpInfo;
        XL_GetBitmapInfo(hBitmap, &bmpInfo);
        Bitmap newBitmap(bmpInfo.Width, bmpInfo.Height, bmpInfo.ScanLineLength, PixelFormat32bppARGB, XL_GetBitmapBuffer(hBitmap, 0, 0));
        Gdiplus::Graphics graphics(&newBitmap);

        // 全部透明填充
        SolidBrush TransparentBrush(Color(1, 0, 0, 0));
        graphics.FillRectangle(&TransparentBrush, 0, 0, nTotalWidth, nTotalHeight);

        // 填充非透明区域
        Region ClipArea(clipRect);
        Region defaultRegion(rect);
        defaultRegion.Exclude(&ClipArea);
        graphics.SetClip(&defaultRegion);
        SolidBrush HalfTransparentBrush(Color(100, 0, 0, 0));
        graphics.FillRectangle(&HalfTransparentBrush, 0, 0, nTotalWidth, nTotalHeight);

        graphics.SetClip(rect);
        Pen pen(Color(255, 255, 255, 255));
        Pen pen1(Color(255, 255, 255, 255));
        pen.SetAlignment(PenAlignmentInset);
        pen1.SetDashStyle(DashStyleDash );

        // 画4条线
        graphics.DrawLine(&pen1, nLeftPos, nTopPos, nLeftPos, nTopPos+nRectHeight-1);
        graphics.DrawLine(&pen1, nLeftPos, nTopPos, nLeftPos+nRectWidth-1, nTopPos);
        graphics.DrawLine(&pen1, nLeftPos+nRectWidth-1, nTopPos, nLeftPos+nRectWidth-1, nTopPos+nRectHeight-1);
        graphics.DrawLine(&pen1, nLeftPos, nTopPos+nRectHeight-1, nLeftPos+nRectWidth-1, nTopPos+nRectHeight-1);
        return hBitmap;
}
```