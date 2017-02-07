## 结构
- 设计两层结构的UI组织模式：ListControl，ListItem
- 设计ListItem的数据模型（ViewModel）比如叫ListItemData，用数组或者其他容器存储，根据不同场景可能有多层次结构。
- 为ListControl设计数据模型ADT：AddListItemData，RemoveListItemData,InsertListItemData，Clear
- 为ListControl设计UI刷新模型ADT：BeginUpdate、EndUpdate

## UI变动的更新代码
```
BeginUpdate()
    for ....
        AddListItemData()
    end
EndUpdate()
```

## 原则：
- ListControl最多只创建可视区域可以容纳的下的ListItem，一开始全部不可见。
- 在BeginUpdate的时候设置一个变量，挂起UI更新
- 在BeginUpdate和EndUpdate之间的代码负责更新ListControl的ViewModel。
- 整个ListControl只在EndUpdate这个函数里更新界面，包括**重新绑定**可视区域ListItem当前所绑定的ListItemData，设置可见性等等。
- 根据上一条，需要维护一堆ListItem和ListItemData之间的映射
- 所有交互操作，都会导致当前可视区域的ListItem所绑定的ListItemData的映射表的变动，所以全部交互都会采用上面的BeginUpdate和EndUpdate更新界面。