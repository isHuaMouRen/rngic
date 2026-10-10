# Rngic

**Rngic** *(Random Number Generator based Image Confuser)* 是一款基于[随机数生成器](https://zh.wikipedia.org/wiki/%E9%9A%8F%E6%9C%BA%E6%95%B0%E7%94%9F%E6%88%90)的图像混淆工具，可以将您的图片打乱为看似随机的像素排列，使他人无法直接窥探图片的原始内容。即使他人拥有相同的混淆工具，在不知道正确密钥的情况下，也难以还原原始图片，从而为您的图片提供额外的隐私保护。

## 原理

[伪随机数生成器](https://zh.wikipedia.org/wiki/%E4%BC%AA%E9%9A%8F%E6%9C%BA%E6%95%B0%E7%94%9F%E6%88%90%E5%99%A8)在使用相同算法和相同种子时，会生成完全一致的伪随机数序列。Rngic 正是基于这一特性实现图像混淆。

您只需设置一个密钥，即可使用 Rngic 对图片进行混淆。只要在还原时使用相同的密钥，就能按照相同的规则恢复图片；而使用不同的密钥，则会得到不同的像素排列。

## 效果

![original](/docs/images/original.png)

*(原始图像)*

![confused](/docs/images/confused.png)

*(使用123作为秘钥的混淆图)*
