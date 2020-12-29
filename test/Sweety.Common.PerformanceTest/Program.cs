using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Globalization;
using Microsoft.Extensions.Localization;


using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

using Sweety.Common.Comparer;
using Sweety.Common.Cryptography;
using Sweety.Common.Extensions;
using Sweety.Common.Verification;

namespace Sweety.Common.PerformanceTest
{
    public class CacheObjectModel
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public IList<CacheObjectModel> Children { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (Object.ReferenceEquals(this, obj)) return true;
            if (!(obj is CacheObjectModel model)) return false;

            
            bool result = ID == model.ID
                && Name == model.Name;

            if (result)
            {
                if (Children != null && model.Children != null)
                {
                    if (Children.Count == model.Children.Count)
                    {
                        for (int i = 0; i < Children.Count; i++)
                        {
                            if (!Children[i].Equals(model.Children[i]))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (!(Children == null && model.Children == null))
                {
                    return false;
                }
            }

            return result;
        }
    }
    class Program
    {
        static int __workerThreads = 0;
        static int __completionPortThreads = 0;
        static bool first = true;

        static void PrintAvailableThreads()
        {
            int workerThreads, completionPortThreads;

            if (first)
            {
                first = false;

                ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
                Console.WriteLine($"#{DateTime.Now.ToLongTimeString()}# MinThreads workerThreads:{workerThreads}\t\t completionPortThreads:{completionPortThreads}");

                ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
                Console.WriteLine($"#{DateTime.Now.ToLongTimeString()}# MaxThreads workerThreads:{workerThreads}\t\t completionPortThreads:{completionPortThreads}");
            }
            else
            {
                Console.WriteLine();
            }

            var defaultColor = Console.ForegroundColor;
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            if (__workerThreads == workerThreads && __completionPortThreads == completionPortThreads)
            {
                Console.WriteLine($"#{DateTime.Now.ToLongTimeString()}# ({Thread.CurrentThread.ManagedThreadId}) AvailableThreads： workerThreads:{workerThreads}\t\t completionPortThreads:{completionPortThreads}{Environment.NewLine}");
            }
            else
            {
                Console.Write($"#{DateTime.Now.ToLongTimeString()}# ({Thread.CurrentThread.ManagedThreadId}) AvailableThreads： workerThreads:");
                if (__workerThreads == workerThreads)
                {
                    Console.Write(workerThreads);
                }
                else
                {
                    __workerThreads = workerThreads;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(workerThreads);
                    Console.ForegroundColor = defaultColor;
                }

                Console.Write($"\t\t completionPortThreads:");
                if (__completionPortThreads == completionPortThreads)
                {
                    Console.WriteLine($"{completionPortThreads}{Environment.NewLine}");
                }
                else
                {
                    __completionPortThreads = completionPortThreads;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"{completionPortThreads}{Environment.NewLine}");
                    Console.ForegroundColor = defaultColor;
                }

            }
        }

        static void Main(string[] args)
        {
            Sweety.Common.Cryptography.ISymmetricCryptography asc = Sweety.Common.Cryptography.SymmetricAlgorithmFactory.Create(SymmetricAlgorithmType.AES);
            string key = Convert.ToBase64String(asc.Key);
            string iv = Convert.ToBase64String(asc.IV);
            Console.WriteLine(key);
            Console.WriteLine(iv);


            string apptokenText = "AC304A1C-1C84-4D10-8373-8190CBC27EB6|41|1603780678000|41|2";
            asc.Key = Convert.FromBase64String("G/kbXx0s/KxGFm0zrQ1i9VeUcabJvX4HKGax4JXY0LM=");
            asc.IV = Convert.FromBase64String("YHdp1vRmM3lXY4SU4Ae7Mw==");
            string apptoken = Convert.ToBase64String( asc.Encrypt(Encoding.UTF8.GetBytes(apptokenText)));
            Console.WriteLine(apptoken);
            return;


            //using (FileStream fs = new FileStream())
            Console.WriteLine(AppContext.BaseDirectory);
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            byte[] secretkey = new Byte[64];
            //RNGCryptoServiceProvider is an implementation of a random number generator.
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                // The array is now filled with cryptographically strong random bytes.
                rng.GetBytes(secretkey);

            }

            Console.Write("new byte[] {");
            foreach (var keyb in secretkey)
            {
                Console.Write($"0x{keyb:x}, ");
            }
            Console.WriteLine("}");
            Console.Read();
            return;
            Report.GenReport1();

            ///new MigraDocCoreUsage();

            //_ = new SelectHtmlToPdfUsage();
            //_ = new FreeSpirePdfUsage();
            //_ = new PdfSharpCoreUsage();

            //_ = new PdfSharpCore_PdfSharpUsage();

            //iTextSharpUsage testPDF = new iTextSharpUsage();
            //testPDF.GeneratePDF();
            return;
            //testPDF.GenerateReport();
            //testPDF.Te();
            //testPDF.ExtractText();

            string a = "差异是一个汉语词语，拼音为chā yì，意思是指区别，不同。也指统一体内在的差异，即事物内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”";
            string b = "“差”是一个中文单词，拼音是“差”，意思是“差”和“差”。它也指统一的内部差异，即事物内部没有激化的矛盾。资料来源于齐、所著的《三国演义》第一卷第一七七二页：“整个形象是一个能守义守义的战士。孩子应该与众不同。”";

            //string a = "cha yiabeuioe";
            //string b = "chā yìjkw463897x4683484fwnjgweuioe";


            //string a = "cha yiab";
            //string b = "chā yìjkw463897x4683484fwnjgw";

            //string a = "cha yiabeuioe";
            //string b = "chā yìjkw463897u4683484fwiegweuioe";

            //string a = "cha yi";
            //string b = "chā yì";

            //string a = "ai";
            //string b = "āì";

            //string a = "1abc";
            //string b = "def1";


            TextDifferenceResultProcessing differenceResult = new TextDifferenceResultProcessing();

            a = "中国素有“室无瓷不雅观，人无瓷难尊”一说，瓷器（china）对于中国人（China）而言，有着深化骨血般的情感。18世纪初，波兰国王奥古斯都二世同样怀着对瓷器的挚爱，修建了欧洲第一个瓷窑，命名“麦森”。于是，这个为国王打造生活方式的品牌应运而生，阅历了300多年风雨进程的麦森现在已然成为德国甚至全欧洲的第一瓷器品牌，那些高雅的器物被人们赋予“白色黄金”之美誉。令人不可思议的是，这个阅历了11场战役、6个不同政党的品牌，现在仍然秉承着最初的理念，为人们打造精致、高雅的生活方式。";
            b = @"中国素有“室无瓷不雅，人无瓷难尊”一说，瓷器（china）对于中国人（China）而言，”简单的一句话，道出了麦森的品牌精神——坚持。正是因为创立者、波兰国王奥古斯都二世对艺术的爱和坚持，才让麦森成为整个欧洲乃至世界的瓷器艺术典范；令人难以想象的是，这个经历了11场战争、6个不同政党的品牌，如今依然秉承着最初的理念，为人们打造精致、优雅的生活方式。\r\n\r\n2008年底，麦森全球品牌首席执行官克里斯提安·库茨科博士（Dr. Christian Kurtzke）受命上任，这位去过42个国家，有着丰富经验的CEO给麦森带来了新的活力。在他的推动下，麦森完成了两位数的年增长率。麦森 Joaillerie、米兰的Villa 麦森、麦森 Fine Art、麦森 Art CAMPUS和麦森 Home等项目让人们看到了一个拥有悠久历史却依然不断创新的品牌，将其触角渗透到了珠宝、家居、艺术、校园等各个领域。虽然，通过开发新的产品，让这个优雅的、有着皇室血统的瓷器品牌给现代艺术界展现了它富有创新精神的另一面。然而，在库茨科看来，所谓的创新“不过是对麦森品牌做了一个回顾和规划整理而已”。“300多年前，麦森的使命是打造奥古斯都二世的生活方式。”克里斯提安·库茨科说：“从创立之初，麦森出品的瓷器就涵盖了所有这些领域，它不是从今天才开始多元化的，这些分支在品牌创立的时候就已经探索过了。”\r\n2014年11月中旬，麦森和中央美术学院签署了战略合作协议，建立紧密的合作关系，成立“麦森奖”。麦森的这一举动并不让人意外，对艺术家的依赖和重用是麦森不变的传统，在麦森的300多年历史中，有无数的艺术家为之添砖加瓦。克里斯提安·库茨科说：“麦森奖是一个长期的合作，我想可能会延续数百年吧。波兰国王奥古斯都二世同样怀着对瓷器的挚爱，修建了欧洲第一个瓷窑，命名“麦森”。于是，这个为国王打造生活方式的品牌应运而生，经历了300多年风雨历程的麦森如今已然成为德国乃至全欧洲的第一瓷器品牌，那些优雅的器物被人们赋予“白色黄金”之美誉。正是因为麦森的坚持，才能让这个蓝色的“交叉双剑”的商标一直使用至今；正是因为历届麦森CEO的坚持，才能让这个古老的瓷器品牌在纳粹、东西德等各个动荡的政府管制下，依然屹立不倒。\r\n纵观麦森的品牌历史，它记录的不仅仅是麦森自己300多年来关于瓷器艺术的发展，还承载了欧洲300多年的文化和艺术、政治和信仰。";
            TextComparer textComparer = new TextComparer();
            textComparer.AffirmSameMinLength = 7;
            //textComparer.ExtractSameParts(a, b);
            var zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze , a, b, out var aza, out var bza);
            string zeResult = a + "\r\n" + b + "\r\n\r\n" + aza + "\r\n" + bza;

            a = "七七七八八八九九九十十十一一一二二二三三三四四四五五五六六六";
            b = "一一一二二二三三三四四四五五五六六六七七七八八八九九九十十十";
            zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze, a, b, out aza, out bza);
            zeResult += "\r\n----------------------\r\n" + a + "\r\n" + b + "\r\n\r\n" + aza + "\r\n" + bza;


            a = "六六六八八八十十十一一一二二二三三三四四四五五五七七七九九九";
            b = "一一一二二二三三三四四四五五五六六六七七七八八八九九九十十十";
            zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze, a, b, out aza, out bza);
            zeResult += "\r\n----------------------\r\n" + a + "\r\n" + b + "\r\n\r\n" + aza + "\r\n" + bza;

            a = "十九八七六五四三二一二三四五六七八九十";
            b = "一二三四五六七八九十九八七六五四三二一";
            zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze, a, b, out aza, out bza);
            zeResult += "\r\n----------------------\r\n" + a + "\r\n" + b + "\r\n\r\n" + aza + "\r\n" + bza;

            a = "六六六八八八十十十一一一二二二三三三四四四五五五七七七九九九";
            b = "一一一二二二三三三四四四五五五六六六七七七八八八九九九十十十";
            zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze, a, b, out aza, out bza);
            zeResult += "\r\n----------------------\r\n" + a + "\r\n" + b + "\r\n\r\n" + aza + "\r\n" + bza;


            a = "六六八八十十一一额二二三三四四五五七七九九";
            b = "信息方法嗯嗯口味一一给二二三三四四五五六六七七八八九九十十";
            zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze, a, b, out aza, out bza);
            zeResult += "\r\n----------------------\r\n" + a + "\r\n" + b + "\r\n\r\n" + aza + "\r\n" + bza;

            a = "我们一起出去吹吹风。";
            b = "我们干了许多很了不起的事。";
            zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze, a, b, out aza, out bza);
            zeResult += "\r\n----------------------\r\n" + a + "\r\n" + b + "\r\n\r\n" + aza + "\r\n" + bza;

            a = "有些爱注定悲剧。";
            b = "上天早已安排今生今世我们注定要在一起。";
            zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze, a, b, out aza, out bza);
            zeResult += "\r\n----------------------\r\n" + a + "\r\n" + b + "\r\n\r\n" + aza + "\r\n" + bza;



            a = "冷风吹，雪花飞。";
            b = "多穿点衣服今天冷风吹个不停呢。";
            zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze, a, b, out aza, out bza);
            zeResult += "\r\n----------------------\r\n" + a + "\r\n" + b + "\r\n\r\n" + aza + "\r\n" + bza;

            a = "我自关山点酒，千秋皆入喉。";
            b = "千秋霸业，唯我独尊。";
            zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze, a, b, out aza, out bza);
            zeResult += "\r\n----------------------\r\n" + a + "\r\n" + b + "\r\n\r\n" + aza + "\r\n" + bza;


            a = "这件衣服真难看，尽然还卖这么贵。";
            b = "你卖的什么鬼东西，居然卖的这么贵。";
            zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze, a, b, out aza, out bza);
            zeResult += "\r\n----------------------\r\n" + a + "\r\n" + b + "\r\n\r\n" + aza + "\r\n" + bza;


            a = "雾里看花";
            b = "歌词：雾里看花水中望月";
            zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze, a, b, out aza, out bza);
            zeResult += "\r\n----------------------\r\n" + a + "\r\n" + b + "\r\n\r\n" + aza + "\r\n" + bza;

            if (zeResult == String.Empty)
            {

            }



            TextContrast textContrast = new TextContrast();
            textContrast.AffirmSameMinLength = 2;
            var differences = textContrast.Contrast(a, b);
            differenceResult.TryMarkDifferences(differences, a, b, out var adiff, out var bdiff);
            string diffResult = adiff + "\r\n" + bdiff;




            var reverseDifferences = textContrast.ReverseContrast(a, b); //Reverse
            differenceResult.TryMarkDifferences(reverseDifferences, a, b, out adiff, out bdiff);
            string reverseDiffResult = adiff + "\r\n" + bdiff;
            string duibiejieguo = $"{a}\n{b}\n\n{diffResult}\n\n{reverseDiffResult}";
            if (duibiejieguo == String.Empty) { }



            //StringBuilder buildA = new StringBuilder();
            //StringBuilder buildB = new StringBuilder();
            /*
            TextContrast2 textContrast2 = new TextContrast2();
            var differences = textContrast2.Contrast(a, b);
            if (differences.Count > 0)
            {
                var diff = differences[0];
                if (diff.AIndex > 0) buildA.Append(a.Substring(0, diff.AIndex));
                if (diff.BIndex > 0) buildB.Append(b.Substring(0, diff.BIndex));
            }

            for(int i=0; i< differences.Count;i++)
            {
                var diff = differences[i];
                if (i > 0)
                {
                    var prevDiff = differences[i - 1];
                    buildA.Append(a.Substring(prevDiff.AIndex + prevDiff.ALength, diff.AIndex - (prevDiff.AIndex + prevDiff.ALength)));
                    buildB.Append(b.Substring(prevDiff.BIndex + prevDiff.BLength, diff.BIndex - (prevDiff.BIndex + prevDiff.BLength)));
                }

                if (diff.ALength > 0 && diff.BLength > 0)
                {
                    buildA.Append("<diff>");
                    buildA.Append(a.Substring(diff.AIndex, diff.ALength));
                    buildA.Append("</diff>");

                    buildB.Append("<diff>");
                    buildB.Append(b.Substring(diff.BIndex, diff.BLength));
                    buildB.Append("</diff>");
                }
                else if (diff.ALength > 0)
                {
                    string con = a.Substring(diff.AIndex, diff.ALength);
                    buildA.Append("<extra>");
                    buildA.Append(con);
                    buildA.Append("</extra>");

                    buildB.Append("<lack>");
                    buildB.Append(con);
                    buildB.Append("</lack>");
                }
                else
                {
                    string con = b.Substring(diff.BIndex, diff.BLength);
                    buildA.Append("<lack>");
                    buildA.Append(con);
                    buildA.Append("</lack>");

                    buildB.Append("<extra>");
                    buildB.Append(con);
                    buildB.Append("</extra>");
                }
            }

            if (differences.Count > 0)
            {
                var diff = differences[^1];
                if (a.Length > (diff.AIndex + diff.ALength)) buildA.Append(a.Substring(diff.AIndex + diff.ALength));
                if (b.Length > (diff.BIndex + diff.BLength)) buildB.Append(b.Substring(diff.BIndex + diff.BLength));
            }
            */




            /*
            Sweety.Common.Caching.ICache cache = new Sweety.Common.Caching.MemcachedCache(new (string, int)[]
            {
                ("127.0.0.1", 11211)
            });

            
            Sweety.Common.Caching.ICache cache = new Sweety.Common.Caching.RedisCache("127.0.0.1:6379,connectTimeout=2000");

            DateTime now = DateTime.Now;
            string cacheKey = "shizhong";
            DateTimeOffset dateTimeOffset = DateTimeOffset.Now.AddSeconds(60);
            Console.WriteLine($"#{DateTime.Now:ss.fffffff}# added result : {cache.Add(cacheKey, now.Ticks)}");

            
            Task.Delay(1500).Wait();
            Console.WriteLine($"#{DateTime.Now:ss.fffffff}# contains result : {cache.Contains(cacheKey)}");

            //Task.Delay(800).Wait();
            Console.WriteLine($"#{DateTime.Now:ss.fffffff}# get: {cache.Get(cacheKey)}");

            Task.Delay(200).Wait();
            Console.WriteLine($"#{DateTime.Now:ss.fffffff}# get<T>: {cache.Get<long>(cacheKey)}");
            */

            /*
            using var s = SymmetricAlgorithmFactory.Create(SymmetricAlgorithmType.AES);
            Console.WriteLine("KEY:");
            foreach(byte b in s.Key)
            {
                Console.Write($"{b}, ");
            }

            Console.WriteLine("IV:");
            foreach (byte b in s.IV)
            {
                Console.Write($"{b}, ");
            }
            */

            /*
            IReadOnlyList<CacheObjectModel> OBJECT_VALUE = new List<CacheObjectModel>
            {
                new CacheObjectModel
                {
                    ID = "0001",
                    Name = "国内",
                    Children = new List<CacheObjectModel>
                    {
                        new CacheObjectModel
                        {
                            ID = "00010001",
                            Name = "经济",
                            Children = new List<CacheObjectModel>
                            {
                                new CacheObjectModel
                                {
                                    ID = "000100010001",
                                    Name = "宏观"
                                },
                                new CacheObjectModel
                                {
                                    ID = "000100010002",
                                    Name = "微观"
                                }
                            }
                        },
                        new CacheObjectModel
                        {
                            ID = "00010002",
                            Name = "政治"
                        },
                        new CacheObjectModel
                        {
                            ID = "00010003",
                            Name = "民生"
                        },
                        new CacheObjectModel
                        {
                            ID = "00010004",
                            Name = "社会"
                        },
                        new CacheObjectModel
                        {
                            ID = "00010005",
                            Name = "环境"
                        },
                        new CacheObjectModel
                        {
                            ID = "00010006",
                            Name = "其他"
                        }
                    }
                },
                new CacheObjectModel
                {
                    ID = "0002",
                    Name = "国际"
                },
                new CacheObjectModel
                {
                    ID = "0003",
                    Name = "数读"
                },
                new CacheObjectModel
                {
                    ID = "0004",
                    Name = "军事"
                },
                new CacheObjectModel
                {
                    ID = "0005",
                    Name = "航空"
                }

            }.AsReadOnly();

            Sweety.Common.Caching.ICache cache = new Sweety.Common.Caching.RedisCache("127.0.0.1:6379,connectTimeout=2000");
            */

            /*
            cache.Add("wegwe", OBJECT_VALUE);

            var sbc = cache.Get<IReadOnlyList<CacheObjectModel>>("wegwe");

            for(int i=0; i< OBJECT_VALUE.Count; i++)
            {
                if (!(OBJECT_VALUE[i].Equals(sbc[i])))
                {
                    Console.WriteLine($"OBJECT_VALUE [{i}] 不相同。");
                }
            }
            */


            /*
            cache.Set("object", OBJECT_VALUE);

            var sel = cache.GetValues(new string[] { "ab", "ef", "object", "ww" });
            Console.WriteLine(sel.Count);

            Console.WriteLine(cache.Get("object") == OBJECT_VALUE);
            cache.Remove("object");
            */

            /*
            Type modelType = typeof(Program);
            PropertyInfo[] properties = modelType.GetProperties();
            ParameterExpression pe_instance = Expression.Parameter(modelType);
            ParameterExpression pe_value = Expression.Parameter(typeof(Object), "value");
            foreach (PropertyInfo p in properties)
            {
                if (!p.CanWrite) continue;

                var expr_property = Expression.Property(pe_instance, p);
                var expr_assign = Expression.Assign(expr_property, Expression.Convert(pe_value, p.PropertyType));
                var expr_lambda = Expression.Lambda<AssTest<Program>>(expr_assign, pe_instance, pe_value);

                Console.WriteLine(expr_lambda.ToString());
            }
            */





            //var task = B_Method();

            //Console.Write($"#{DateTime.Now.ToLongTimeString()}# ({Thread.CurrentThread.ManagedThreadId}) 开始输出最终结果：");

            //Console.WriteLine(await task);

            /*
            Thread t = new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine($"#{DateTime.Now.ToLongTimeString()}# This is thread ({Thread.CurrentThread.ManagedThreadId})");
                    Thread.Sleep(500);
                }
            });
            t.Start();
            */

            //TestMemory test = new TestMemory();
            //var summary = BenchmarkRunner.Run<TestMemory>();

            //Console.WriteLine(typeof(StringComparison).IsEnum);
            /*
            TestObject test = new TestObject();
            test.Definite();
            test.Dynamic();
            test.Object();
            */
            //var summary = BenchmarkRunner.Run<TestObject>();



            //var summary = BenchmarkRunner.Run<TestCompression>();

            //var summary = BenchmarkRunner.Run<TestConvert>();

            //var summary = BenchmarkRunner.Run<TestIdentifyType>();


            _ = BenchmarkRunner.Run<TesTextComparer>();


            /*
            PrintCulture("Thread Culture", Thread.CurrentThread.CurrentCulture);
            Console.WriteLine();

            PrintCulture("CultureInfo Culture", CultureInfo.CurrentCulture);
            Console.WriteLine();

            PrintCulture("Thread UI Culture", Thread.CurrentThread.CurrentUICulture);
            Console.WriteLine();

            PrintCulture("CultureInfo UI Culture", CultureInfo.CurrentUICulture);
            Console.WriteLine();

            PrintCulture("Invariant Culture", CultureInfo.InvariantCulture);
            Console.WriteLine();

            PrintCulture("Invariant UI Culture", CultureInfo.InstalledUICulture);
            Console.WriteLine();
            */

            Console.WriteLine("*结束了...");
            Console.ReadLine();
        }
    }


    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class TesTextComparer
    {
        const string a = "差异是一个汉语词语，拼音为chā yì，意思是指区别，不同。也指统一体内在的差异，即事物内部包含着的没有激化的矛盾。出处是《三国志·魏志·齐王芳传》：“ 整像为兵，能守义执节，子弟宜有差异。”";
        const string b = "“差”是一个中文单词，拼音是“差”，意思是“差”和“差”。它也指统一的内部差异，即事物内部没有激化的矛盾。资料来源于齐、所著的《三国演义》第一卷第一七七二页：“整个形象是一个能守义守义的战士。孩子应该与众不同。”";

        [Benchmark]
        public void ExtractSameParts()
        {
            TextComparer textComparer = new TextComparer();
            textComparer.AffirmSameMinLength = 2;
            _ = textComparer.ExtractSameParts(a, b);
        }

        [Benchmark]
        public void ExtractDifferenceParts()
        {
            TextComparer textComparer = new TextComparer(2, false);
            _ = textComparer.ExtractDifferenceParts(a, b);
        }


        [Benchmark]
        public void ExtractDifferencePartsAndProcessing()
        {
            TextDifferenceResultProcessing differenceResult = new TextDifferenceResultProcessing();
            TextComparer textComparer = new TextComparer();
            textComparer.AffirmSameMinLength = 2;
            var zeze = textComparer.ExtractDifferenceParts(a, b);
            differenceResult.TryMarkDifferences(zeze, a, b, out var aza, out var bza);
        }
    }

    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class TestMemory
    {
        string[] arr;


        public TestMemory()
        {
            Random rand = new Random();
            arr = new string[100];
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (rand.Next(0, 100) % 2 == 0)
                {
                    arr[i] = Guid.NewGuid().ToString();
                    count++;
                }
            }

            Console.WriteLine($"总共赋值了：{count}个元素。");
        }

        
        //[Benchmark]
        public object NewObj()
        {
            return new object();
        }

        //[Benchmark]
        public object Box()
        {
            return 1024;
        }

        //[Benchmark]
        public int Value()
        {
            return 1024;
        }

        [Benchmark]
        public bool IsKeyword()
        {
            bool result = false;
            for(int i = 0; i < arr.Length; i++)
            {
                result |= arr[i] is null;
            }
            return result;
        }

        [Benchmark]
        public bool Reference()
        {
            bool result = false;
            for (int i = 0; i < arr.Length; i++)
            {
                result |= Object.ReferenceEquals(null, arr[i]);
            }
            return result;
        }

        [Benchmark]
        public bool Equalsss()
        {
            bool result = false;
            for (int i = 0; i < arr.Length; i++)
            {
                result |= null == arr[i];
            }
            return result;
        }
    }

    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class TestCompression
    {
        Stream input;

        Stream deflateOutput;
        Stream gzipOutput;
        Stream brotliOutput;

        public long FileLength { get; private set; }

        public TestCompression()
        {
            /*
            byte[] buffer = new byte[1024 * 1024 * 4];

            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(buffer);
            }

            input = new MemoryStream(buffer);
            */

            FileInfo file = new FileInfo("/Users/jess/myun/LICENSE.html");
            FileLength = file.Length;

            input = new MemoryStream((int)file.Length);

            using (var f = file.OpenRead())
            {
                f.CopyTo(input);
            }
            input.Seek(0L, SeekOrigin.Begin);
        }

        ~TestCompression()
        {
            input.Dispose();
        }


        [Benchmark]
        public long DeflateCompress()
        {
            Sweety.Common.Compression.ICompression c = new Common.Compression.Deflate();

            using (MemoryStream output = new MemoryStream((int)input.Length))
            {
                c.Compress(input, output);

                input.Position = 0L;

                return output.Length;
            }
        }


        [Benchmark]
        public long GZipCompress()
        {
            Sweety.Common.Compression.ICompression c = new Common.Compression.GZip();

            using (MemoryStream output = new MemoryStream((int)input.Length))
            {
                c.Compress(input, output);

                input.Position = 0L;

                return output.Length;
            }
        }




        [Benchmark]
        public long BrotliCompress()
        {
            Sweety.Common.Compression.ICompression c = new Common.Compression.Brotli();

            using (MemoryStream output = new MemoryStream((int)input.Length))
            {
                c.Compress(input, output);

                input.Position = 0L;

                return output.Length;
            }
        }

    }



    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class TestConvert
    {
        long _long;
        string _strLong;
        byte[] _arrLong;

        public TestConvert()
        {
            _long = DateTime.Now.Ticks;
            _strLong = _long.ToString();
            _arrLong = BitConverter.GetBytes(_long);
        }


        //[Benchmark]
        public string ToStr()
        {
            return _long.ToString();
        }

        //[Benchmark]
        public byte[] ToArr()
        {
            byte[] result = new byte[9];

            BitConverter.GetBytes(_long).CopyTo(result, 1);

            return result;
        }

        [Benchmark]
        public byte[] ToSpan()
        {
            Span<byte> span = stackalloc byte[9];
            
            return BitConverter.TryWriteBytes(span.Slice(1,8), _long) ? span.ToArray() : null;
        }

        //[Benchmark]
        public void ToSpan2()
        {
            Span<byte> span = stackalloc byte[9];

            BitConverter.TryWriteBytes(span.Slice(1,8), _long);
        }

        [Benchmark]
        public byte[] ToStream()
        {
            MemoryStream memoryStream = new MemoryStream(9);
            memoryStream.Position = 1L;

            memoryStream.Write(BitConverter.GetBytes(_long), 0, 8);

            return memoryStream.GetBuffer();
        }




        //[Benchmark]
        public long FromStr()
        {
            return long.TryParse(_strLong, out var result) ? result : default;
        }

        //[Benchmark]
        public long FromArr()
        {
            return BitConverter.ToInt64(_arrLong);
        }
    }


    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class TestIdentifyType
    {
        object _obj;
        
        public TestIdentifyType()
        {
            _obj = 19850825L;
        }

        private byte[] ToBytes(object data)
        { 
            switch (data)
            {
                case bool v: return BitConverter.GetBytes(v);
                case char v: return BitConverter.GetBytes(v);
                case float v: return BitConverter.GetBytes(v);
                case double v: return BitConverter.GetBytes(v);
                case decimal v: return new byte[0]; // Decimal.GetBits(v);


                case long v: return BitConverter.GetBytes(v);


                case byte v: return new byte[] { v };
                case sbyte v: return new byte[] { unchecked((byte)v) };
                case short v: return BitConverter.GetBytes(v);
                case ushort v: return BitConverter.GetBytes(v);
                case int v: return BitConverter.GetBytes(v);
                case uint v: return BitConverter.GetBytes(v);
                case ulong v: return BitConverter.GetBytes(v);
                default:
                    return null;
            }
        }


        private byte[] ToBytes2(object data)
        {
            switch (Type.GetTypeCode(data.GetType()))
            {
                case TypeCode.Boolean: return BitConverter.GetBytes((bool)data);
                case TypeCode.Char: return BitConverter.GetBytes((char)data);
                case TypeCode.Single: return BitConverter.GetBytes((float)data);
                case TypeCode.Double: return BitConverter.GetBytes((double)data);
                case TypeCode.Decimal: decimal dec= (decimal)data; return new byte[0];

                case TypeCode.Int64: return BitConverter.GetBytes((long)data);


                case TypeCode.Byte: return new byte[] { (byte)data };
                case TypeCode.SByte: return new byte[] { unchecked((byte)(sbyte)data) };
                case TypeCode.Int16: return BitConverter.GetBytes((short)data);
                case TypeCode.UInt16: return BitConverter.GetBytes((ushort)data);
                case TypeCode.Int32: return BitConverter.GetBytes((int)data);
                case TypeCode.UInt32: return BitConverter.GetBytes((uint)data);
                case TypeCode.UInt64: return BitConverter.GetBytes((ulong)data);
                //case TypeCode.DateTime: return BitConverter.GetBytes((ulong)data);
                default:
                    return null;
            }
        }

        private byte[] ToBytes3(object data)
        {
            Type t = data.GetType();

            if (Object.ReferenceEquals(t, ValueTypeConstants.BooleanType)) return BitConverter.GetBytes((bool)data);

            if (Object.ReferenceEquals(t, ValueTypeConstants.CharType)) return BitConverter.GetBytes((char)data);

            if (Object.ReferenceEquals(t, ValueTypeConstants.SingleType)) return BitConverter.GetBytes((float)data);

            if (Object.ReferenceEquals(t, ValueTypeConstants.DoubleType)) return BitConverter.GetBytes((double)data);
            
            if (Object.ReferenceEquals(t, ValueTypeConstants.DecimalType))
            {
                decimal fef = (decimal)data;
                if (fef == Decimal.One) { }
                return new byte[0];
            }


            if (Object.ReferenceEquals(t, ValueTypeConstants.Int64Type)) return BitConverter.GetBytes((long)data);
            if (Object.ReferenceEquals(t, ValueTypeConstants.UInt64Type)) return BitConverter.GetBytes((ulong)data);



            if (Object.ReferenceEquals(t, ValueTypeConstants.ByteType)) return new byte[] { (byte)data };
            if (Object.ReferenceEquals(t, ValueTypeConstants.SByteType)) return new byte[] { unchecked((byte)(sbyte)data) };

            if (Object.ReferenceEquals(t, ValueTypeConstants.Int16Type)) return BitConverter.GetBytes((short)data);
            if (Object.ReferenceEquals(t, ValueTypeConstants.UInt16Type)) return BitConverter.GetBytes((ushort)data);

            if (Object.ReferenceEquals(t, ValueTypeConstants.Int32Type)) return BitConverter.GetBytes((int)data);
            if (Object.ReferenceEquals(t, ValueTypeConstants.UInt32Type)) return BitConverter.GetBytes((uint)data);


            //case TypeCode.DateTime: return BitConverter.GetBytes((ulong)data);

            return null;
        }

        private byte[] ToBytes4(object data)
        {
            if (data is bool v) return BitConverter.GetBytes(v);
            if (data is char a) return BitConverter.GetBytes(a);
            if (data is float b) return BitConverter.GetBytes(b);
            if (data is double c) return BitConverter.GetBytes(c);
            if (data is decimal d) return new byte[0]; // Decimal.GetBits(v);

            if (data is long k) return BitConverter.GetBytes(k);
            if (data is ulong l) return BitConverter.GetBytes(l);

            if (data is byte e) return new byte[] { e };
            if (data is sbyte f) return new byte[] { unchecked((byte)f) };
            if (data is short g) return BitConverter.GetBytes(g);
            if (data is ushort h) return BitConverter.GetBytes(h);
            if (data is int i) return BitConverter.GetBytes(i);
            if (data is uint j) return BitConverter.GetBytes(j);


            return null;
        }


        [Benchmark]
        public byte[] SwitchType()
        {
            return ToBytes(_obj);
        }

        [Benchmark]
        public byte[] IsType()
        {
            return ToBytes4(_obj);
        }

        [Benchmark]
        public byte[] IsTypeCode()
        {
            return ToBytes2(_obj);
        }

        [Benchmark]
        public byte[] IsTypeCompare()
        {
            return ToBytes3(_obj);
        }
    }
}