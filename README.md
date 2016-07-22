### boltstack开源项目：
----------------------
地址：https://github.com/fanfeilong/boltstack

### BOLT最常用文档有哪些？
-------------------------
- 这里是官方整理过的[FAQ](http://bolt.xunlei.com/faq.html)，可逐条细读。
- 这里是[官方文档](http://xldoc.xl7.xunlei.com/0000000018/index.html)，左侧目录树好好利用，最好的办法就是对左侧目录树烂熟于胸，要查相关文档的时候就会很高效快捷。
- 这里是非官方的[文档压缩包](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=672&extra=page%3D1)，可能会过时（在线文档会及时更新内容），不过需要的同学可以试用。
- 这里是非官方的[思维导图](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=532&extra=page%3D1)，可以对照上一条的文档目录树，加深对BOLT体系层次结构的理解。
- 这里是[水哥的控件教程](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=204&extra=page%3D1)。
- 这里是[BOLTSDK的git仓库](https://github.com/lurenpluto/BOLT_SDK)。
- 这里是[BOLT扩展元对象的git仓库](https://github.com/lurenpluto/BXF)。
- 这里是[官方示例控件库的git仓库](https://github.com/lurenpluto/BOLT_CONTROLS)。

### FQA
-------

#### 如何在一个UI事件里完成后执行动作？
* 在UI事件里使用`AsynCall`或者`SetOnceTimer`去执行异步动作。
```
function HostWnd_OnShowWindow(self)
  AsynCall(function ()
    --do something here , this function will be called after hostwnd shown.
  end)
end
```
* 这是由于UI事件是在UI线程被同步触发的，如果事件响应函数被阻塞住，则会卡住界面的后续逻辑。使用`AsynCall`或者`SetOnceTimer`将待执行动作封装成一个消息并投递给UI线程，异步执行逻辑。由于是在同一个线程，不需要加锁。

#### 如何调用WebBrowser的JavaScript函数？
1. 在OnDocumentComplated事件里获取[GetRawWebBrowser](参考：http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=197&highlight=GetRawWebBrowser)发起调用。
2. 在C++里转发调用。
```
IWebBrowser2* pWebBrowser2 = *(reinterpret_cast<IWebBrowser2**>(lua_touserdata(2)));
IDispatch* pDispatchDocument = NULL;
pWebBrowser2->get_Document(&pDispatchDocument);
if(pDispatchDocument==NULL)
{
	return 0;
}
CComPtr<IHTMLDocument2> pHtmlDocument2 = NULL;
pDispatchDocument->QueryInterface(IID_IHTMLDocument2,reinterpret_cast<void**>(&pHtmlDocument2);
pDispatchDocument->Release();

CComBSTR bStrLan = _T("javascript");
CComBSTR bStrFun = _T"test2()";
VARIANT var;
var.vt = VT_EMPTY;
CComPtr<IHTMLWindow2> pHtmlWindow2 = NULL;
pHtmlDocument2->get_parentWindow(&pHtmlWindow2);
pHtmlWindow2->execScript(bStrFun,bStrLan,&var);
```

#### 如何防止子对象的OnInitControl被调用多次
如果在OnInitControl里通过AddChild添加子对象会导致子对象的OnInitControl被调用多次，此时建议使用AsyncCall去动态添加子对象，由于BOLT的时序是从父对象到子对象Bind，然后从子对象到父对象Init，从而一般建议在OnInitControl里获取子对象，但此时同步动态添加子对象会导致复杂的时序问题。

#### 如何监听不可见对象的事件？
在不可见对象外层套一个LayoutObject，监听LayoutObject对象的事件。

#### 如何正确处理控件的Enter和Leave事件？
处理控件的OnControlMouseEnter和OnControlMouseLeave事件。

#### 如何在win32工程下获取bolt创建的主窗体句柄啊？
XLUE.h里有两个C接口：
```
XLUE_API(XLUE_HOSTWND_HANDLE) XLUE_GetHostWndByID(const char* id);
XLUE_API(OS_HOSTWND_HANDLE) XLUE_GetHostWndWindowHandle(XLUE_HOSTWND_HANDLE hHostWnd);[/color]
```

#### BOLT里使用require会报错，如何组织模块。
参考这个[模块分离的帖子](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=205&highlight=%E6%A8%A1%E5%9D%97)。

#### Lua的table元素是否是有序的吗？
Lua的table分数组段和字典段，如果table只被当作数组用时，是有序的，当做字典用时则不保证顺序。
参考云风的[博客](http://blog.codingnow.com/2005/10/lua_table.html)。

#### 什么时候Bitmap或Texture资源需要做成ImageList，为什么？
首先
1. 深紫色(255,0,255)是Imagelist（图像列表）的分割线
2. 半紫色(127，0，127)是Texture（纹理）的分割线

会做成这样，有两个原因：
1. 纹理(Texture)本身是需要半紫色分割线的，纹理包括三宫格、五宫格以及九宫格或者用户自定义不同部分伸缩。
2. Imagelist是为了方便把不同的Bitmap或者Texture素材合并到一张PNG图片里，把逻辑上属于一组的放在一起，即有利于资源管理（对开发者而言），
   美工也不需要每张单独导出，另一方面资源的加载代码应该也会做一些优化和缓存。
3. 这也是为什么纹理和ImageList的默认分割线会有区分的原因吧。


#### BOLT的Lua环境下有提供获取屏幕宽、高的全局函数么？
木有，用户可以自己通过BOLT的Lua API、Object、Class注册机制注册自己的API给Lua环境做这件事。

#### UIObject的属性可以在Lua脚本里直接使用么？比如直接调用attr.left,attr.top等？
不可以，UIObject是C++注册给Lua的UserData：在xml里可直接配置；在Lua脚本里，只能通过UIObject的方法去获取和设置。

#### 为什么对UIObject调用GetAttribute方法出错？
只有自定义控件才有GetAttribute方法，UIObject并没有GetAttribute方法。这个地方需要说明的是，UIObject文档里的那些属性是只可以在xml里配置或者在Lua里通过对应的方法去Get和Set。
而自定义控件是继承自LayoutObject的，对自定义控件GetAttribute得到的只是一个空table，并不包含父类LayoutObject的那些xml配置属性。
实际上我认为这个名字应该叫GetUserAttribute更贴切点吧。不过这个已经没法改了，其实只要稍加区分就可以。

#### xar 资源包的继承关系怎么弄？
见这个[关于资源包继承问题小结帖子](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=683&highlight=%E8%B5%84%E6%BA%90%E5%8C%85)。

#### 如果用预定义动画组合做复杂动画时太麻烦怎么办？
用自定义动画，这边有个[自定义动画的帖子](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=411&extra=page%3D1)。

#### 如何做滚动条？
BOLT的官方控件库里有List控件，里面有用到滚动条控件，可参考。

#### 如何在C#里使用BOLT，如何在Delphi里使用BOLT？
分别参考：
* [DELPHI的例子](http://bolt.xunlei.com/bbs/forum.php?mod=forumdisplay&fid=43)
* [BOLT.NET子论坛](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=223&highlight=delphi)

#### 如何监听键盘事件，处理键盘按键消息？
处理LayoutObject的OnKeyDown、OnKeyUp、OnChar、OnHotKey等事件的文档。
关于KeyCode则参考msdn：[VirualKeyCode](http://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx)
另外这边有个辅助代码：[KeyCode.lua](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=409&extra=page%3D1)

#### 如何自定义事件？
- [xml里自定义事件的指南](http://xldoc.xl7.xunlei.com/0000000018/00000000180001000007.html)
- [描述了在自定义控件里自定义事件，以及使用FireExtEvent在控件内部触发自定义事件](http://xldoc.xl7.xunlei.com/0000000018/0000000018000100002900006.html)
- [一个例子代码](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=470)
- [自定义事件参数和内置事件参数不同的说明](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=171)


#### 如何理解事件重定向，比如`RoureToFather`？
参考文档：
- [输入事件分发和路由策略介绍](http://xldoc.xl7.xunlei.com/0000000018/00000000180001000035.html)
- [元对象输入事件重定向](http://xldoc.xl7.xunlei.com/0000000018/00000000180001000036.html)

#### 如何做系统托盘？
- [最小Win32系统托盘示例](https://bobobobo.wordpress.com/2009/03/),在BOLT下，最好弄一个后台窗口做消息转发和处理，配合系统托盘。
- [系统托盘分享代码](http://bolt.xunlei.com/bbs/home.php?mod=space&uid=78)
- [一个例子](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=634&highlight=%E6%89%98%E7%9B%98)。
- [另一个例子](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=708&extra=page%3D1)。
- [MenuWnd，打开后，鼠标点击非Menu区域，如何将MenuWnd关掉哦？](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=557&highlight=%E6%89%98%E7%9B%98][color=#FF0000]

#### 如何注册全局函数
首先，参考[XLLRT_RegisterGlobalAPI用法](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=267&highlight=XLLRT%5C_RegisterGlobalAPI)

其次，另一种做法：
```
void LOG(const char* str)
{
  std::cout<<str<<std::endl;
}
static const luaL_Reg s_utilXLRTAPI[] =
{
    {"LOG", LOG},
    {NULL,NULL}
};
void RegisterGlobalFunc(XL_LRT_ENV_HANDLE hEnv)
{
    lua_State* luaState = XLLRT_GetLuaState(XLLRT_GetRuntime(hEnv,NULL));
    int pos = 0;
    while(s_utilXLRTAPI[pos].func)
    {
        lua_pushcfunction(luaState,s_utilXLRTAPI[pos].func);
        lua_setglobal(luaState,s_utilXLRTAPI[pos].name);
        pos++;
    }
}
```

#### BOLT有哪些全局函数?
[BOLT提供的全局函数](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=33&page=1&extra=#pid92)

#### Angle动画执行后如何保持最后一刻的状态?
参考：
[帖子1](http://bolt.xunlei.com/bbs/home.php?mod=space&uid=78) [帖子2](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=454) [帖子3](http://bolt.xunlei.com/bbs/home.php?mod=space&uid=3893) [帖子4](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=709#lastpost)

#### BOLT下如何注册C++类，对象到Lua环境？
参考HelloBOLT第7课。这里也有一个QQ群成员?天蝎莮提供的[BOLT注册C++类到Lua环境的辅助宏定义以及示例](http://blog.163.com/lvan100@yeah/blog/static/6811721420142982815555/)。

#### 如何对窗口或者某个UIObject以及其子对象做截图？
使用RenderFactory:RenderObject(UIObject srcObject,Bitmap destBitmap)，参考[RenderFactory的文档](http://xldoc.xl7.xunlei.com/0000000018/000000001800001000020000100008.html)。
也可以参考这个帖子，[青青三叶草的说明](http://bolt.xunlei.com/bbs/forum.php?mod=viewthread&tid=23&highlight=%E5%BF%AB%E7%85%A7)，
里面提到了对HostWnd可以使用XL_BITMAP_HANDLE GetWindowBitmap()获取整个窗口的截图。
如果是RealObject，则使用[RealObject:GetWindowBitmap()方法](http://xldoc.xl7.xunlei.com/0000000018/00000000180000200002000010000900005.html)。

#### 遇到如下链接错误怎么办？
```
error LNK2019: 无法解析的外部符号 __imp__PathAppendW@8，该符号在函数 "wchar_t const * __cdecl GetResDir(void)" (?GetResDir@@YAPB_WXZ) 中被引用
```
答：
```
#include <Shlwapi.h>
#pragma comment(lib,"shlwapi.lib")
```
另外[msdn上关于shlwapi.h的一个帖子](http://social.msdn.microsoft.com/Forums/en-US/04660e1b-f858-44d7-80fb-04a62321de73/shlwapih-not-found)有段有趣的评论：

>Those path functions reside in the Shell Lightweight API, which require SHLWAPI.DLL. That DLL (despite being a critical component used by several Windows DLLs, and its absence will lead to an unbootable OS) is a part of IE (several IE components use it as well). It's sort of like Microsoft's[color=rgb(0, 149, 196)][utils/misc.cpp](http://www.flounder.com/badprogram.htm#Global%20Header%20Files)[/color] file.IMO, the Shell lightweight API is one of the worst designed libraries to come out of the Windows team. Firstly, the name "Lightweight" is a misnomer (it's 450Kb in size, but its dependencies may take up to 100MB of disk space!). The DLL is just a repository for junk functions that "didn't quite fit anywhere else". Granted, you have some partially useful functions like SHDeleteKey or PathStripPath. But then you're forced to have completely useless functions like IUnknown_AtomicRelease and UrlFixUpW, which comes with their own set of dependencies (OLE). Since it is used by both IE and the shell (the "webby-explorer"), Microsoft don't even know whether to put it in the IE SDK or the core SDK (btw that's why you're seeing it in your help). And when you use one Shlwapi function, your application will stop working unless the user has chosen to install IE.[/font][/align][align=left][font=simsun]If Microsoft split the lower level SHLWAPIs (like PathAppend and PathStripPath) from the higher level ones (SHAutoComplete, ConnectToConnectionPoint), the entire library would be a bit more useful. Unfortunately, any changes made to it now will break many thousands of apps which rely on the functions exported by it. That's what you get when you build a library that's based around your Attic folder

题外话：编译和链接两个阶段，编译检查的更多是语法错误，链接则是是做符号解析。链接时遇到“error LNK2019:无法解析的外部符号..."，则意味着某个符号所在地lib没有被添加，所以连接器找不到。
关于链接过程，这里有个帖子详细解释了[链接中符号查找的规则](http://blogs.msdn.com/b/oldnewthing/archive/2013/01/07/10382714.aspx)。
题外话2：上面的错误信息里的符号是被修饰过的名字。可以参考这篇[Visual C++名字修饰](http://zh.wikipedia.org/wiki/Visual_C%2B%2B%E5%90%8D%E5%AD%97%E4%BF%AE%E9%A5%B0)的wiki。

#### 如何在BOLT的Lua环境下使用Json库？
1. 直接获取Lua的Json库，在BOLT的Lua环境下加载使用。
2. 封装某个C/C++的Json库，注册自己的JSON API给BOLT的Lua环境。比如使用cJSON库。
3. 各种语言的JSON库见 [json.org](http://json.org)，这个页面上包含了Json的介绍，Json的文法，Json的状态转换图，以及各种语言的Json库连接。

#### 为什么调用控件的GetControlObject获取不到目标对象？
有很多种情景会导致获取的是nil。
- 首先，调用GetControlObject的必须是一个控件对象。
- 其次，GetControlObject只能获取当前控件的直接子对象，如果控件A的子对象B也是一个控件，你希望获取B控件的子对象C，则应该先调用通过A调用GetControlObject获取到B，再对B调用GetControlObject获取C。
- 再者，OnBind的时候，子对象还没绑定，这个时候调用GetControlObject可能会失败，所以应该在OnInitControl事件里调用。
- 剩下的可能是，目标对象的Id不对，比如拼写错误，xml里的id前后多了空格等。

对象定位除了GetControlObject外，还有GetObject，GetUIObject等，请参考
[引擎对象管理和定位](http://xldoc.xl7.xunlei.com/0000000018/00000000180001000040.html)

#### 如何添加控件的成员变量
1. 在XML的attr_def节点定义
2. 使用控件的GetAttribute()返回属性表，动态增加。例如：
```
local attr = self:GetAttribute()
attr.myvariable=xxx
attr.subtable={}
```
建议分开管理界面成员和数据成员，例如初始化的时候分开两个子表
```
local attr = self:GetAttribute()
attr.uiItems={}
attr.data={}
```

#### 如何从HostWnd直接通过Id路径获取Object？
```
function GetHostWndObject(wnd,id)
  local objTree = wnd:GetBindUIObjectTree()
  local rootObj = objTree:GetRootObject()
  return rootObj:GetObject(id)
end
```

#### 什么是LuaState
[stack over flow: What is a Lua State?](http://stackoverflow.com/questions/4201284/what-is-a-lua-state)
[lua_State Description from lua.org](http://www.lua.org/manual/5.1/manual.html#lua_State)

>typedef struct lua_State lua_State;

>Opaque structure that keeps the whole state of a Lua interpreter. The Lua library is fully reentrant: it has no global variables. All information about a state is kept in this structure.

>A pointer to this state must be passed as the first argument to every function in the library, except to lua_newstate, which creates a Lua state from scratch. 

#### 如何制作圆角、发光等效果
全部用图片资源或者纹理资源来做。BOLT没有提供内置的属性做这种效果。以后有可能会提供扩展元对象来做「自动化」特效。

#### lua_tostring返回的字符串指针生命周期
[lua_tostring](http://www.lua.org/manual/5.1/manual.html#lua_tostring)是个宏定义，等价于[lua_tolstring](http://www.lua.org/manual/5.1/manual.html#lua_tolstring)
`const char *lua_tolstring (lua_State *L, int index, size_t *len);`

>Converts the Lua value at the given acceptable index to a C string. If len is not NULL, it also sets *len with the string length. The Lua value must be a string or a number; otherwise, the function returns NULL. If the value is a number, then lua_tolstring also changes the actual value in the stack to a string. (This change confuses lua_next when lua_tolstring is applied to keys during a table traversal.)

>lua_tolstring returns a fully aligned pointer to a string inside the Lua state. This string always has a zero ('\0') after its last character (as in C), but can contain other zeros in its body. Because Lua has garbage collection, there is no guarantee that the pointer returned by lua_tolstring will be valid after the corresponding value is removed from the stack. 

#### SetCaptureMouse惯用法
1. 设置一个私有属性isMouseEnter，在OnMouseEnter的时候设置为true，在OnMouseLeave的时候设置为false。
2. 在OnMouseDown的时候设置SetCaptureMouse(true);
3. 在OnMouseUp的时候，判断isMouseEnter如果为false，则SetCaptureMouse(false);

#### 关于对象生命周期
>SetBitmap给ImageObject，此时ImageObject里增加了对Bitmap的引用，Lua的GC是不会回收这个Bitmap的。Bitmap是一个UserData，对应的C++类采用的是引用计数的生命周期管理，在C++里采用引用计数的方式管理，但是对象Push到Lua环境后，则生命周期是由Lua的GC系统控制的。

>当你把脚本里对一个Bitmpa的引用都置为nil，下一次lua的gc发生的时候，lua的gc系统判定该对象没有被任何对象树上的对象持有就会回收该对象，这个时候会调用Bitmap的__gc方法，该__gc方法BOLT在注册Bitmap的时候有提供，对应的是C++层的Bitmap的引用计数减持。如果此时该Bitmap的C++层的引用计数为0，才真正被释放内存，删掉对象。

>如果你是从C++里Create一个Bitmap，此时引用计数为1，调用PushBitmapAPI Push到Lua，引用计数为2.这个时候即使Lua里对该Bitmap的持有都设置为nil，下一次GC触发__gc方法，减持Bitmap，Bitmap的引用计数为1。这样Bitmap就泄漏了。所以Create再Push后，要立刻减持一次引用。BOLT的对象创建后引用计数就为1，这是和COM的引用计数规则不一样的地方。使用C++处理Bitmap的同学需要注意这点。

>如果你在C++里Create后没有Push到Lua环境，直接Release，立刻释放内存。Push到Lua环境的时候，Push方法会先增加以下引用计数，再Push到Lua环境，之后就交给Lua的GC管理，直到GC调用__gc的时候会Release一次。所以Create/Relese要保持平衡。Push的那个增加，对应的是Lua的__gc保持平衡。CheckXXX则只是把对象从Lua栈里取出来，并没有增加引用计数。

>这也很好理解的，C++里new/delete malloc/free要保持平衡。所以Create/Release，AddRef/Release保持平衡，Push/LuaGC保持平衡。都是符合C++的RAII资源管理规则的。

>用引用计数，在c++里需要避免循环引用，这是其缺点，这个靠人肉保证。

>这个其实很有意思，C++的生命周期管理和Lua的生命周期管理的边界是明确的。如果你是用c#，则在c#里，对象是.NET的GC系统负责,在Lua里，对象是Lua的GC系统负责。只有两个GC都判定一个对象死了，才死了。如果这个对象又是个BOLT的基于引用计数的对象，那还要加上BOLT的引用计数管理系统判定该对象死了，才彻底死翘翘了。

>GC和引用计数并不是一个概念，GC一般采用标记/清除算法，像.NET的GC还会分代。新分配的对象在第一代堆内存上分配，多次GC后，如果这个对象还存活，则把它移动到第二代堆内存上。再多次GC后，还存活在第二代堆内存上的对象移动到第三代堆内存上。一般来说第一代堆内存被GC的频率较高，第二代次之，第三代最不频繁。这是因为每次GC的时候会整理内存，在同一代堆内存里做内存碎片整理，所以存活率越高的对象应该移动到碎片整理频率较低的代上去，避免不必要的内存整理。这也是为什么c#里不可以直接对一个对象做指针操作，对象的内存在每次GC的时候都可能会变，指针会失效。所以如果你要做不安全的指针操作，需要Fix以下那个对象，表示说在下面的一个范围内的代码GC的时候请不要移动我的内存。

>GC的时候要做整个对象树的遍历，所以在GC系统来看就是暂停了整个世界。比较牛逼的GC系统，可以做并发GC，代码指令流不用暂停，不过也相应的增加了无数复杂度。

### 知识库
----------

#### 理解窗口之间的关系
* 窗口相关的显示问题，一定要区分窗口的关系：
 Owner、Child、Parent
 这个可以参考MSDN：
    - [Window Features]
      (http://msdn.microsoft.com/en-us/library/ms632599%28v=VS.85%29.aspx)
    - [What is a Window]
      (http://msdn.microsoft.com/en-us/library/windows/desktop/ff381403(v=vs.85).aspx)
* 在BOLT里主要是使用NormalHostWnd:Show方法，具体的枚举值参考BOLT文档或者MSDN。

#### Lua 参考手册
Lua 的c 接口的使用可以参考[Lua reference manual interface](http://pgl.yoyo.org/luai/i/about), 比如：[lua_pushstring](http://pgl.yoyo.org/luai/i/lua_pushstring)
