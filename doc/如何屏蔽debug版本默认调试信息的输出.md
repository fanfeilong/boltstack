## 问题
debug版调试的时候 vs输出了大量的迅雷调试信息(如鼠标事件的调用), 怎么屏蔽啊 ，好乱，都看不到自己的输出了

## 解释

在C:\\TSLOG目录下面，新建一个XLUE.ini配置文件，在里面添加下面的配置项：

```
DebugView=OFF
FileLog=OFF
```

其中debugview用来开关控制台日志输出，而filelog用来开关文件输出