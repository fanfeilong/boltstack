因为项目需求，需要同xar下切换不同资源包，其中包含继承功能。
实现过程中，主要参考在线文档：
http://xldoc.xl7.xunlei.com/0000000018/00000000180001000050.html

具体步骤是，在res根目录下，先声明好package.cfg文件，如下：
```
<xlue name = "default" author="Thunder Corporation" copyright="(C) Thunder Corporation.">
        <nametablecfg path="./nametable.cfg" type="xml"/>
        <mainres package="default"/>

        <loadscript path="./layout/onload.lua" type="lua"/>
</xlue>
```

其中说明我们的xar包主资源师default文件夹。

然后是主default文件夹下的resource.cfg文件，内容如下：

```
<xlue name="default" author="Thunder Corporation" copyright="(C) Thunder Corporation.">
</xlue>
```

紧接着是我们子文件夹下的resource.cfg文件，内容如下：
```
<xlue name="dntgol" author="
Thunder  Corporation" copyright="(C) Thunder Corporation." fathername="default">
</xlue>
```

其中dntgol是子文件夹的名称，fathername是父文件夹的名称。

接下来是比较重要的一点，也是本人出现过的问题，就是我以为把子包未修改的资源文件(比如图片png)删除，理论上程序就应该自动从父包里寻找并应用，但是运行程序后，会发现错误提示“load texture bitmap failed....”，经过询问，感谢幻灰龙、零点终,了解到：
继承的那一部分，在子包的资源声明xml中无需再重复声明！

一语惊醒梦中人，其实不是什么大问题，留贴方便以后出现类似错误的时候有个参考。