在bolt里面，往往会有很多lua辅助代码，也就是传统的MVC里面的M层的东西，供全局使用，那么怎么来设计和共享这种代码？

bolt的luaruntime对lua文件里面定义的默认全局变量，函数等做了限制，只会在该文件的作用域内生效，避免全局命名空间被污染，所以需要真正全局变量的时候，需要调用全局API XLSetGlobal来设置

简单的全局lua函数，直接设置函数，比如
```
function Add(x, y)
  return x+ y
end
XLSetGlobal("Add", Add)
```

那么在使用的地方可以通过下面方法来使用
```
local add = XLGetGlobal("Add")
ret = add(100, 1000)
```

对于一组函数，这种情况不推荐每个函数都调用XLSetGlobal，这样会造成过多的全局变量，容易命名冲突或者运行效率下降，而是应该引入一个中间层table，对函数族合理的分组，然后放到一个table里面，再把这个table进行XLSetGlobal，比如

```
function Add(x, y)
return x+ y
end

function Sub(x, y)
return x- y
end

local obj = {}
obj.Add= Add
obj.Sub= Sub
 
XLSetGlobal("math.helper",obj)
```

那么在使用的地方，可以如下使用：
```
local mathHelper = XLGetGlobal("math.helper")
mathHelper.Add(100, 1000)
mathHelper.Sub(100, 1000)
```

对于在lua里面来模拟C++/Java的标准class，没有必要做的和C++/Java非常一致，class无非就是数据和代码的封装，但是lua代码里面有个缺陷是权限不好控制，这个可以通过变量命名、内部规范之类的来控制
一般来说，最简单的模拟class方法就是使用table，和上面的第二个例子类似，只是在table里面加多一些类数据成员就可以了，示例代码如下：

首先我们定义一个名为Math的“lua class”，如下所示
```
--两个“类”函数，注意第一个参数是self
function Add(self, x, y)
  return self.x + x + y;
end
        
function Sub(self, x, y)
return x - y - self.y
end

--math类的创建者，math的实例需要通过此方法来创建，一般来说该方法是全局方法
function MathCreator()
  local mathobj = {}

  --数据成员初始化
  mathobj.x = 10
  mathobj.y = 20

  --设定类方法
  mathobj.Add = Add
  mathobj.Sub = Sub     
  return mathobj
end
        
XLSetGlobal("MathCreator", MathCreator)
```

那么在使用的地方，可以创建一个Math类实例，并使用：
```
local mathcreator = XLGetGlobal("MathCreator") --获取mathhelper的工厂方法
local mathhelper = mathcreator() --创建一个mathhelper对象
local ret = mathhelper:Add(100, 200) --直接使用math class，注意这时候调用类方法是:而不是.了
ret = mathhelper:Sub(200, 100)
```

上面只是给出了大概的思路，如果真要完全模拟class，还可能需要设计构造函数，拷贝构造函数，变量的读取和设置接口等，但是绝大部分情况下这些都没有必要了，只要够用就好

另外一种方法是使用元表，但是核心思想和上面的例子相同，只是MathCreator函数变更如下：
```
function MathCreator()
  local mathobj = {}
  mathobj.x = 10
  mathobj.y = 20

  local mathmethods = {}
  mathmethods.__index = mathmethods
  mathmethods.Add = Add
  mathmethods.Sub = Sub

  setmetatable(mathobj, mathmethods)
  return mathobj
end
```