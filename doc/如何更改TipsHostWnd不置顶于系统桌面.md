## 问题
TipsHostWnd默认是置顶于系统桌面吧，如何不让其置顶呢？

## 解释
自动模式下如果没有指定owner，那么默认置顶；在弹出之前调用SetParent可以指定owner，这种情况下不会强制指定；非自动模式下根据Create是否指定owner来决定
