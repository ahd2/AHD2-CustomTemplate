# **[AHD2-CustomTemplate](https://github.com/ahd2/AHD2-CustomTemplate)**

<img alt="GitHub Release" src="https://img.shields.io/github/v/release/ahd2/AHD2-CustomTemplate?style=for-the-badge"> <img alt="GitHub Release Date" src="https://img.shields.io/github/release-date/ahd2/AHD2-CustomTemplate?style=for-the-badge"> <img alt="GitHub License" src="https://img.shields.io/github/license/ahd2/AHD2-CustomTemplate?style=for-the-badge">

用于Unity项目的右键菜单拓展，可根据自定义的文本模板，在右键菜单创建自定义脚本，Shader，HLSL等文件。

基于[Unity3D研究院编辑器之创建Lua脚本模板（十六） | 雨松MOMO程序研究院 (xuanyusong.com)](https://www.xuanyusong.com/archives/3732)大佬的案例代码进行的拓展。

！！后续补充：自定义模板有更方便的方式 [Unity 自定义shader模板 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/693947160)

我宣布这个包就是一个玩具。

## 安装

### 通过URL安装

<img src="https://github.com/ahd2/AHD2-DocsRepo/blob/main/AHD2_CustomTemplate/1.png?raw=true" alt="1" style="zoom:50%;" />

打开Unity Package Manager，通过URL添加。输入以下URL。

https://github.com/ahd2/AHD2-CustomTemplate.git

## 使用方式

* 模板创建：菜单栏找到Tools - TemplateGenerator。打开模板生成窗口。

  * 输入模板缩写（只支持字母，如hlsl模板，就写hlsl，名字较长时推荐驼峰命名：myFirstTemplate）。
  * 输入菜单显示名称，该项决定右键菜单显示的名称
  * 格式，该项决定生成文件的后缀，必填。如生成HLSL文件，就填入 hlsl 。不用在前面加  .    。
  * 下面文本框填入模板。则每次该文件创建时都会自带里面的内容。可以使用#NAME#作为标识符。标识符会在文件生成后替换为文件名字。

<div align="center">
    <figure style="text-align: center;">
        <img src="https://github.com/ahd2/AHD2-DocsRepo/blob/main/AHD2_CustomTemplate/2.png?raw=true" style="zoom: 80%;" />
        <figcaption>菜单中找到生成器。</figcaption>
    </figure>
</div>

  <figure>   <img src="https://github.com/ahd2/AHD2-DocsRepo/blob/main/AHD2_CustomTemplate/3.png?raw=true" alt="#NAME#会被命名替换。">   <figcaption>#NAME#会被命名替换。</figcaption> </figure>

* 模板删除：模板列表部分可对以生成模板进行删除。

* 模板添加后，可在右键菜单根据模板新建文件。

  <img src="https://github.com/ahd2/AHD2-DocsRepo/blob/main/AHD2_CustomTemplate/4.png?raw=true" style="zoom:50%;" />
