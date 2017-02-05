## 问题
如何禁用CaptureObject的双击最大化功能?

## 解释
在相应的CaptionObject里面响应左键双击消息，并指定返回handled=true，这样会拦截该消息的默认处理逻辑
```
function Caption_LButtonDbClick(self)       
   return 0,true,false
end
```