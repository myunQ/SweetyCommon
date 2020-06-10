using System;
using System.Collections.Generic;

using Xunit;

using Sweety.Common.Comparer;

namespace Sweety.Common.Tests.Comparer
{
    public class TextComparerTest
    {
        [Theory]
        [InlineData("差异是一个汉语词语，拼音为chā yì，意思是指区别，不同。也指统一体内在的差异，即事物内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”"
            , "差异是一个汉语词语，拼音为chā yì，意   思是指区别，不同。也指统一体内在的差异，即内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”"
            , 2
            , "差异是一个汉语词语，拼音为chā yì，意<lack>   </lack>思是指区别，不同。也指统一体内在的差异，即<extra>事物</extra>内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”\r\n差异是一个汉语词语，拼音为chā yì，意<extra>   </extra>思是指区别，不同。也指统一体内在的差异，即<lack>事物</lack>内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”")]
        [InlineData("差异是一个汉语词语，拼音为chā yì，意   思是指区别，不同。也指统一体内在的差异，即内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”"
            , "差异是一个汉语词语，拼音为chā yì，意思是指区别，不同。也指统一体内在的差异，即事物内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”"
            , 2
            , "差异是一个汉语词语，拼音为chā yì，意<extra>   </extra>思是指区别，不同。也指统一体内在的差异，即<lack>事物</lack>内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”\r\n差异是一个汉语词语，拼音为chā yì，意<lack>   </lack>思是指区别，不同。也指统一体内在的差异，即<extra>事物</extra>内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”")]
        [InlineData("差异是一个汉语词语，拼音为cha yi，意   思是指区别，不同。也指统一体内在的差异，即内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”"
            , "差异是一个汉语词语，拼音为chā yì，意思是指区别，不同。也指统一体内在的差异，即事物内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”"
            , 2
            , "差异是一个汉语词语，拼音为ch<diff>a</diff> y<diff>i</diff>，意<extra>   </extra>思是指区别，不同。也指统一体内在的差异，即<lack>事物</lack>内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”\r\n差异是一个汉语词语，拼音为ch<diff>ā</diff> y<diff>ì</diff>，意<lack>   </lack>思是指区别，不同。也指统一体内在的差异，即<extra>事物</extra>内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”")]
        [InlineData("差异是一个汉语词语，拼音为chā yì，意思是指区别，不同。也指统一体内在的差异，即事物内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”"
            , "“差”是一个中文单词，拼音是“差”，意思是“差”和“差”。它也指统一的内部差异，即事物内部没有激化的矛盾。资料来源于齐、所著的《三国演义》第一卷第一七七二页：“整个形象是一个能守义守义的战士。孩子应该与众不同。”"
            , 2
            , "<diff>差异</diff>是一个<diff>汉语词语</diff>，拼音<diff>为chā yì</diff>，意思是<diff>指区别，不同。</diff>也指统一<diff>体内在的</diff>差异，即事物内部<extra>包含着的</extra>没有激化的矛盾。<diff>出处是</diff>《三国<diff>志·魏志·齐王芳传》</diff>：“<diff> 整像为兵，</diff>能守义<diff>执节，子弟宜有差异</diff>。”\r\n<diff>“差”</diff>是一个<diff>中文单词</diff>，拼音<diff>是“差”</diff>，意思是<diff>“差”和“差”。它</diff>也指统一<diff>的内部</diff>差异，即事物内部<lack>包含着的</lack>没有激化的矛盾。<diff>资料来源于齐、所著的</diff>《三国<diff>演义》第一卷第一七七二页</diff>：“<diff>整个形象是一个</diff>能守义<diff>守义的战士。孩子应该与众不同</diff>。”")]
        [InlineData("我们一起出去吹吹风。"
            , "我们干了许多很了不起的事。"
            , 2
            , "我们<diff>一起出去吹吹风。</diff>\r\n我们<diff>干了许多很了不起的事。</diff>")]
        [InlineData("有些爱注定悲剧。"
            , "上天早已安排今生今世我们注定要在一起。"
            , 2
            , "<diff>有些爱</diff>注定<diff>悲剧。</diff>\r\n<diff>上天早已安排今生今世我们</diff>注定<diff>要在一起。</diff>")]
        [InlineData("冷风吹，雪花飞。"
            , "多穿点衣服今天冷风吹个不停呢。"
            , 2
            , "<lack>多穿点衣服今天</lack>冷风吹<diff>，雪花飞。</diff>\r\n<extra>多穿点衣服今天</extra>冷风吹<diff>个不停呢。</diff>")]
        [InlineData("我自关山点酒，千秋皆入喉。"
            , "千秋霸业，唯我独尊。"
            , 2
            , "<extra>我自关山点酒，</extra>千秋<diff>皆入喉。</diff>\r\n<lack>我自关山点酒，</lack>千秋<diff>霸业，唯我独尊。</diff>")]
        [InlineData("这件衣服真难看，尽然还卖这么贵。"
            , "你卖的什么鬼东西，居然卖的这么贵。"
            , 2
            , "<diff>这件衣服真难看，尽然还卖</diff>这么贵。\r\n<diff>你卖的什么鬼东西，居然卖的</diff>这么贵。")]
        [InlineData("雾里看花"
            , "歌词：雾里看花水中望月"
            , 2
            , "<lack>歌词：</lack>雾里看花<lack>水中望月</lack>\r\n<extra>歌词：</extra>雾里看花<extra>水中望月</extra>")]
        [InlineData("七七七八八八九九九十十十一一一二二二三三三四四四五五五六六六"
            ,"一一一二二二三三三四四四五五五六六六七七七八八八九九九十十十"
            , 2
            , "<extra>七七七八八八九九九十十十</extra>一一一二二二三三三四四四五五五六六六<lack>七七七八八八九九九十十十</lack>\r\n<lack>七七七八八八九九九十十十</lack>一一一二二二三三三四四四五五五六六六<extra>七七七八八八九九九十十十</extra>")]
        [InlineData("十九八七六五四三二一二三四五六七八九十"
            , "一二三四五六七八九十九八七六五四三二一"
            , 2
            , "<lack>一二三四五六七八九</lack>十九八七六五四三二一<extra>二三四五六七八九十</extra>\r\n<extra>一二三四五六七八九</extra>十九八七六五四三二一<lack>二三四五六七八九十</lack>")]
        [InlineData("六六六八八八十十十一一一二二二三三三四四四五五五七七七九九九"
            , "一一一二二二三三三四四四五五五六六六七七七八八八九九九十十十"
            , 2
            , "<extra>六六六八八八十十十</extra>一一一二二二三三三四四四五五五<lack>六六六</lack>七七七<lack>八八八</lack>九九九<lack>十十十</lack>\r\n<lack>六六六八八八十十十</lack>一一一二二二三三三四四四五五五<extra>六六六</extra>七七七<extra>八八八</extra>九九九<extra>十十十</extra>")]
        [InlineData("六六八八十十一一二二三三四四五五七七九九"
            , "信息方法嗯嗯口味一一二二三三四四五五六六七七八八九九十十"
            , 2
            , "<diff>六六八八十十</diff>一一二二三三四四五五<lack>六六</lack>七七<lack>八八</lack>九九<lack>十十</lack>\r\n<diff>信息方法嗯嗯口味</diff>一一二二三三四四五五<extra>六六</extra>七七<extra>八八</extra>九九<extra>十十</extra>")]
        [InlineData("cha yiabeuioe"
            , "chā yìjkw463897u4683484fwiegweuioe"
            , 1
            , "ch<diff>a</diff> y<diff>iab</diff>euioe\r\nch<diff>ā</diff> y<diff>ìjkw463897u4683484fwiegw</diff>euioe")]
        [InlineData("abcd"
            , "efghijk"
            , 1
            , "<diff>abcd</diff>\r\n<diff>efghijk</diff>")]
        [InlineData("chā yìjkw463897x4683484fwnjgw"
            , "cha yiab"
            , 1
            , "ch<diff>ā</diff> y<diff>ìjkw463897x4683484fwnjgw</diff>\r\nch<diff>a</diff> y<diff>iab</diff>")]
        [InlineData("chā yìjkw463897x4683484fwnjgweuioe"
            , "cha yiabeuioe"
            , 1
            , "ch<diff>ā</diff> y<diff>ìjkw463897x4683484fwnjgw</diff>euioe\r\nch<diff>a</diff> y<diff>iab</diff>euioe")]
        [InlineData("cha yiabeuioe"
            , "chā yìjkw463897x4683484fwnjgweuioe"
            , 1
            , "ch<diff>a</diff> y<diff>iab</diff>euioe\r\nch<diff>ā</diff> y<diff>ìjkw463897x4683484fwnjgw</diff>euioe")]
        [InlineData("abcd"
            , ""
            , 1
            , "<extra>abcd</extra>\r\n<lack>abcd</lack>")]
        [InlineData("abcd"
            , "defg"
            , 1
            , "<extra>abc</extra>d<lack>efg</lack>\r\n<lack>abc</lack>d<extra>efg</extra>")]
        [InlineData("1abc"
            , "def1"
            , 1
            , "<lack>def</lack>1<extra>abc</extra>\r\n<extra>def</extra>1<lack>abc</lack>")]
        [InlineData("abcd"
            , "abcdefg"
            , 1
            , "abcd<lack>efg</lack>\r\nabcd<extra>efg</extra>")]
        [InlineData("abcdefg"
            , "12345fg"
            , 1
            , "<diff>abcde</diff>fg\r\n<diff>12345</diff>fg")]
        [InlineData("abcdefgh"
            , "12345fg"
            , 1
            , "<diff>abcde</diff>fg<extra>h</extra>\r\n<diff>12345</diff>fg<lack>h</lack>")]
        [InlineData(""
            , ""
            , 1
            , "\r\n")]
        public void ExtractDifferenceParts(string a, string b, int affirmSameMinLength, string expected)
        {
            TextDifferenceResultProcessing resultProcessing = new TextDifferenceResultProcessing();
            TextComparer textComparer = new TextComparer();
            textComparer.AffirmSameMinLength = affirmSameMinLength;
            var differences = textComparer.ExtractDifferenceParts(a, b);
            resultProcessing.TryMarkDifferences(differences, a, b, out var aDiff, out var bDiff);
            string actual = $"{aDiff}\r\n{bDiff}";
            Assert.Equal(expected, actual);
        }
    }
}
