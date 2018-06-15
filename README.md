# MinePlug by Zip

> Zip 2018.06.14
> ( 此项目已废弃，将于近期使用python重写，感谢关注 )

---

## 环境

win10 vs2015(v14) .net-v4.5.2

## 暴力判断

暴力实现的部分很简单。就两部分，数字和周围块数相等就把周围都插上旗，数字和周围旗子数相等就把周围其他的都点掉。

这部分主要是为了封装一些操作为了下面的操作。

## 容斥

这里有一个简单的二元容斥需要推一下，这个基本能解决大部分初学者(百秒开外的)的判断想法了。

我们设一个数字周围的所有没有确定的块为集合 A ， 其中有 AL 颗雷 ， 设另一个数字周围的所有没有确定的块为集合 B ， 其中有 BL 颗雷 。 他们相交的部分为 C (如果有) ， 其中有 CL 颗雷 。

简单容斥可以得到

![](http://latex.codecogs.com/gif.latex?CL\\leq\\min(AL,BL,C))

![](http://latex.codecogs.com/gif.latex?CL\\geq\\max(AL-(A-C),BL-(B-C),0))

接下来讨论处理几种情况 ， 当 CL 的值正好确定的时候 ：

* 如果 A-C == AL-CL ， 给 A-C 的部分插旗子。

* 如果 AL-CL == 0 ， 点开 A-C 的部分。

* 如果 C == CL ， 给 C 的部分插旗子。

* 如果 CL == 0 ， 点开 C 的部分。

就酱

## random

RT。。。

(已迁移) https://blog.ziposcar.cn/mineplug/

