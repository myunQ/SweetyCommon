using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Globalization;


using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

using Sweety.Common.Verification;
using Sweety.Common.DataProvider;
using Sweety.Common.DataProvider.SqlServer;
using System.Net.Configuration;
using Microsoft.Diagnostics.Runtime;
using Dragon.GroupService.PerformanceTest;
using System.Reflection.Metadata;
using System.Data.SqlClient;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics.Eventing.Reader;
using System.Net;

namespace Sweety.Common.PerformanceTest
{
    
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

            var defaultColor = Console.ForegroundColor;
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            if (__workerThreads == workerThreads && __completionPortThreads == completionPortThreads)
            {
                //Console.WriteLine($"{Environment.NewLine}#{DateTime.Now.ToLongTimeString()}# ({Thread.CurrentThread.ManagedThreadId}) AvailableThreads： workerThreads:{workerThreads}\t\t completionPortThreads:{completionPortThreads}{Environment.NewLine}");
            }
            else
            {
                Console.Write($"{Environment.NewLine}#{DateTime.Now.ToLongTimeString()}# ({Thread.CurrentThread.ManagedThreadId}) AvailableThreads： workerThreads:");
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

        static async Task Main(string[] args)
        {
            CodeTimer.Initialize();

            SqlServerUtility.Initialize(new string[]
            {
                ConfigurationManager.ConnectionStrings["db-b"].ConnectionString
            }, new string[]{
                ConfigurationManager.ConnectionStrings["db-a"].ConnectionString
            });

            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        PrintAvailableThreads();
            //        Thread.Sleep(10);
            //    }
            //});


            try
            {
                //InsertToDb();

                Guid id = Guid.Parse("F880ED06-8C51-4BE5-B4F7-AB3DF2599D73");

                //ScalarToInt<int>(id);
                //ScalarToInt<int?>(id);

                //ScalarToGuid<Guid>(id);
                //ScalarToGuid<Guid?>(id);
                //ScalarToGuidForString<Guid>(id);
                //ScalarToGuidForString<Guid?>(id);

                //ScalarToString(id);
                //ScalarToStringForGuid(id);

                //ScalarToDateTime<DateTime>(id);
                //ScalarToDateTime<DateTime?>(id);

                //ScalarToBytes(id);
                //ScalarToIPAddressForBytes(id);
                //ScalarToIPAddressForString(id);

                //GetSingle<byte[]>(id);
                //GetSingle<IPAddress>(id);

                //GetList<byte[]>(id, Guid.Parse("7019E75A-1663-409B-A02C-A35F465D6711"), Guid.Parse("C2E35113-89A5-4849-8568-AB1BBE617D3E"), Guid.Parse("C2E35113-89A5-4849-8568-AB1BBE617D3E"), Guid.Parse("6FD1E71A-BC0F-4E9E-AD31-AB5F63A4C493"));
                //GetList<IPAddress>(id, Guid.Parse("7019E75A-1663-409B-A02C-A35F465D6711"), Guid.Parse("C2E35113-89A5-4849-8568-AB1BBE617D3E"), Guid.Parse("C2E35113-89A5-4849-8568-AB1BBE617D3E"), Guid.Parse("6FD1E71A-BC0F-4E9E-AD31-AB5F63A4C493"));

                //GetSet<byte[]>(id, Guid.Parse("7019E75A-1663-409B-A02C-A35F465D6711"), Guid.Parse("C2E35113-89A5-4849-8568-AB1BBE617D3E"), Guid.Parse("C2E35113-89A5-4849-8568-AB1BBE617D3E"), Guid.Parse("6FD1E71A-BC0F-4E9E-AD31-AB5F63A4C493"));
                //GetSet<IPAddress>(id, Guid.Parse("7019E75A-1663-409B-A02C-A35F465D6711"), Guid.Parse("C2E35113-89A5-4849-8568-AB1BBE617D3E"), Guid.Parse("C2E35113-89A5-4849-8568-AB1BBE617D3E"), Guid.Parse("6FD1E71A-BC0F-4E9E-AD31-AB5F63A4C493"));

                GetCollection<byte[]>(id, Guid.Parse("7019E75A-1663-409B-A02C-A35F465D6711"), Guid.Parse("C2E35113-89A5-4849-8568-AB1BBE617D3E"), Guid.Parse("C2E35113-89A5-4849-8568-AB1BBE617D3E"), Guid.Parse("6FD1E71A-BC0F-4E9E-AD31-AB5F63A4C493"));
                GetCollection<IPAddress>(id, Guid.Parse("7019E75A-1663-409B-A02C-A35F465D6711"), Guid.Parse("C2E35113-89A5-4849-8568-AB1BBE617D3E"), Guid.Parse("C2E35113-89A5-4849-8568-AB1BBE617D3E"), Guid.Parse("6FD1E71A-BC0F-4E9E-AD31-AB5F63A4C493"));



                //ReadIntFromReader(id);
                //ReadGuidFromReader(id);
                //ReadStringFromReader(id);
                //ReadDateTimeFromReader(id);

                //ScalarToInt<int>(Guid.NewGuid());
                //ScalarToInt<int?>(Guid.NewGuid());

                //ScalarToGuid<Guid>(Guid.NewGuid());
                //ScalarToGuid<Guid?>(Guid.NewGuid());
                //ScalarToGuidForString<Guid>(Guid.NewGuid());
                //ScalarToGuidForString<Guid?>(Guid.NewGuid());



                //ScalarToBytes(Guid.NewGuid());

                //ScalarToString(Guid.NewGuid());
                //ScalarToStringForGuid(Guid.NewGuid());

                //ScalarToDateTime<DateTime>(Guid.NewGuid());
                //ScalarToDateTime<DateTime?>(Guid.NewGuid());

                //ScalarToBytes(Guid.NewGuid());
                //ScalarToIPAddressForBytes(Guid.NewGuid());
                //ScalarToIPAddressForString(Guid.NewGuid());



                //GetSingle<byte[]>(Guid.NewGuid());
                //GetSingle<IPAddress>(Guid.NewGuid());

                //GetList<byte[]>(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
                //GetList<IPAddress>(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

                //GetSet<byte[]>(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
                //GetSet<IPAddress>(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

                GetCollection<byte[]>(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
                GetCollection<IPAddress>(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

                /*
                var syncTask = Task.Run(() => UpdateForIp());
                var asyncTask = Task.Run(async () => await UpdateForIpAsync());
                await Task.WhenAll(syncTask, asyncTask);
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



            //var summary = BenchmarkRunner.Run<TestMemory>();



            //var summary = BenchmarkRunner.Run<TestDataReaderReadInt>();


            Console.WriteLine("*结束了...");
            Console.ReadLine();
        }

        private static void InsertToDb()
        {
            CodeTimer.Time("Async", 1, () =>
            {
                Task<int>[] tasks = new Task<int>[10000];
                SqlServerUtility db = new SqlServerUtility();
                db.TargetRole = DatabaseServerRole.Slave;
                for (int i = 1; i < 10001; i++)
                {
                    Guid id = Guid.NewGuid();
                    tasks[i - 1] = db.ExecuteNonQueryAsync("INSERT INTO [TA] VALUES (@F1, @F2, @F3, @F4, @F5)", CommandType.Text, null,
                        db.BuildParameter("@F1", id),
                        db.BuildParameter("@F2", i),
                        db.BuildParameter("@F3", id.ToString()),
                        db.BuildParameter("@F4", Convert.ToBase64String(id.ToByteArray()).TrimEnd('=')),
                        db.BuildParameter("@F5", DateTime.Now)).AsTask();
                }

                Task.WaitAll(tasks);
                Console.WriteLine("入库完毕。\n开始结算入库数量：");
                int count = 0;
                for(int i=0;i< tasks.Length; i++)
                {
                    count += tasks[i].Result;
                }
                Console.WriteLine("入库数量：" + count);
            });



            CodeTimer.Time("Sync", 1, () =>
            {
                SqlServerUtility db = new SqlServerUtility();
                db.TargetRole = DatabaseServerRole.Master;
                int count = 0;
                for (int i = 0; i < 10000; i++)
                {
                    Guid id = Guid.NewGuid();
                    count += db.ExecuteNonQuery("INSERT INTO [TA] VALUES (@F1, @F2, @F3, @F4, @F5)", CommandType.Text,
                        db.BuildParameter("@F1", id),
                        db.BuildParameter("@F2", i),
                        db.BuildParameter("@F3", id.ToString()),
                        db.BuildParameter("@F4", Convert.ToBase64String(id.ToByteArray()).TrimEnd('=')),
                        db.BuildParameter("@F5", DateTime.Now));
                }

                Console.WriteLine("入库完毕。\n开始结算入库数量：" + count);
            });


            //CodeTimer.Time("Async1", 1, async () =>
            //{
            //    ValueTask<int>[] tasks = new ValueTask<int>[10000];
            //    SqlServerUtility db = new SqlServerUtility();
            //    db.TargetRole = DatabaseServerRole.Slave;
            //    for (int i = 1; i < 10001; i++)
            //    {
            //        Guid id = Guid.NewGuid();
            //        tasks[i - 1] = db.ExecuteNonQueryAsync("INSERT INTO [TA] VALUES (@F1, @F2, @F3, @F4, @F5)", CommandType.Text, null,
            //            db.BuildParameter("@F1", id),
            //            db.BuildParameter("@F2", i),
            //            db.BuildParameter("@F3", id.ToString()),
            //            db.BuildParameter("@F4", Convert.ToBase64String(id.ToByteArray()).TrimEnd('=')),
            //            db.BuildParameter("@F5", DateTime.Now));
            //    }

            //    int count = 0;
            //    for (int i = 0; i < tasks.Length; i++)
            //    {
            //        count += await tasks[i];
            //    }

            //    Console.WriteLine("入库完毕。\n开始结算入库数量：" + count);
            //});
        }


        private static async ValueTask UpdateForIpAsync()
        {
            Console.WriteLine($"{DateTime.Now:mm:ss.fffffff}# 异步开始入库：");
            int count = 0;
            byte[] ip = new byte[4];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                SqlServerUtility db = new SqlServerUtility();
                var reader = (SqlDataReader)await db.GetReaderAsync("SELECT [Guid] FROM [dbo].[TA]");

                while(await reader.ReadAsync())
                {
                    rngCsp.GetBytes(ip);
                    
                    count += await db.ExecuteNonQueryAsync("UPDATE [dbo].[TA] SET [IP] = @IP, [StringIP] = @StringIP WHERE [Guid] = @Guid", CommandType.Text, null,
                        db.BuildParameter("@Guid", reader.GetGuid(0)),
                        db.BuildParameter("@IP", ip),
                        db.BuildParameter("@StringIP", String.Join(".", ip)));
                }
                
                Console.WriteLine($"{DateTime.Now:mm:ss.fffffff}# 异步入库完毕。入库数量：{count}");
            }
        }

        private static void UpdateForIp()
        {
            Console.WriteLine($"{DateTime.Now:mm:ss.fffffff}# 同步开始入库：");
            int count = 0;
            byte[] ip = new byte[4];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                SqlServerUtility db = new SqlServerUtility();
                db.TargetRole = DatabaseServerRole.Slave;
                var reader = (SqlDataReader)db.GetReader("SELECT [Guid] FROM [dbo].[TA]");


                while (reader.Read())
                {
                    rngCsp.GetBytes(ip);

                    count += db.ExecuteNonQuery("UPDATE [dbo].[TA] SET [IP] = @IP, [StringIP] = @StringIP WHERE [Guid] = @Guid", CommandType.Text, null,
                        db.BuildParameter("@Guid", reader.GetGuid(0)),
                        db.BuildParameter("@IP", ip),
                        db.BuildParameter("@StringIP", String.Join(".", ip)));
                }

                Console.WriteLine($"{DateTime.Now:mm:ss.fffffff}# 同步入库完毕。入库数量：{count}");
            }
        }



        private static void ReadIntFromReader(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();
            var reader = (SqlDataReader)db.GetReader("SELECT [Int] FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", id));

            reader.Read();

            Console.WriteLine($"GetInt32:" + reader.GetInt32(0));
            Console.WriteLine($"this[]:" + reader[0]);
            Console.WriteLine($"GetValue" + reader.GetValue(0));
            Console.WriteLine($"GetFieldValue<int>:" + reader.GetFieldValue<int>(0));

            int execCount = 10000000;
            CodeTimer.Time("GetInt32", execCount, () =>
            {
                Type t = reader.GetFieldType(0);

                if (Object.ReferenceEquals(t, __intType) || Object.ReferenceEquals(t, __uintType))
                {
                    _ = reader.GetInt32(0);
                }
                else if (Object.ReferenceEquals(t, __byteType) || Object.ReferenceEquals(t, __sbyteType))
                {
                    _ = reader.GetByte(0);
                }
                else if (Object.ReferenceEquals(t, __bytesType))
                {
                    _ = reader.GetBytes(0, 0, new byte[0], 0, 100000);
                }
                else if (Object.ReferenceEquals(t,  __charType))
                {
                    _ = reader.GetChar(0);
                }
                else if (Object.ReferenceEquals(t, __charsType))
                {
                    _ = reader.GetChars(0, 0, new char[0], 0, 100000);
                }
                else if (Object.ReferenceEquals(t, __dateTimeType))
                {
                    _ = reader.GetDateTime(0);
                }
                else if (Object.ReferenceEquals(t, __dateTimeOffsetType))
                {
                    _ = reader.GetDateTimeOffset(0);
                }
                else if (Object.ReferenceEquals(t, __decimalType))
                {
                    _ = reader.GetDecimal(0);
                }
                else if (Object.ReferenceEquals(t, __doubleType))
                {
                    _ = reader.GetDouble(0);
                }
                else if (Object.ReferenceEquals(t, __floatType))
                {
                    _ = reader.GetFloat(0);
                }
                else if (Object.ReferenceEquals(t, __guidType))
                {
                    _ = reader.GetGuid(0);
                }
                else if (Object.ReferenceEquals(t, __stringType))
                {
                    _ = reader.GetString(0);
                }
                else if (Object.ReferenceEquals(t, __boolType))
                {
                    _ = reader.GetBoolean(0);
                }
                else if (Object.ReferenceEquals(t, __shortType) || Object.ReferenceEquals(t, __ushortType))
                {
                    _ = reader.GetInt16(0);
                }
                else if (Object.ReferenceEquals(t, __longType) || Object.ReferenceEquals(t, __ulongType))
                {
                    _ = reader.GetInt64(0);
                }
                else if (Object.ReferenceEquals(t, __timeSpanType))
                {
                    _ = reader.GetTimeSpan(0);
                }
                
                else
                {
                    //reader[0]
                }
                
            });

            CodeTimer.Time("this[]", execCount, () =>
            {
                _ = (int)reader[0];
            });

            CodeTimer.Time("GetValue", execCount, () =>
            {
                _ = (int)reader.GetValue(0);
            });

            CodeTimer.Time("GetFieldValue<int>", execCount, () =>
            {
                _ = reader.GetFieldValue<int>(0);
            });

            reader.Close();
        }

        private static void ReadGuidFromReader(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();
            var reader = (SqlDataReader)db.GetReader("SELECT [Guid] FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", id));

            reader.Read();

            Console.WriteLine($"GetGuid:" + reader.GetGuid(0));
            Console.WriteLine($"this[]:" + reader[0]);
            Console.WriteLine($"GetValue" + reader.GetValue(0));
            Console.WriteLine($"GetFieldValue<Guid>:" + reader.GetFieldValue<Guid>(0));

            int execCount = 10000000;
            CodeTimer.Time("GetGuid", execCount, () =>
            {
                Type t = typeof(Guid);


                if (Object.ReferenceEquals(t, __intType) || Object.ReferenceEquals(t, __uintType))
                {
                    _ = reader.GetInt32(0);
                }
                else if (Object.ReferenceEquals(t, __guidType))
                {
                    _ = reader.GetGuid(0);
                }
                else if (Object.ReferenceEquals(t, __byteType) || Object.ReferenceEquals(t, __sbyteType))
                {
                    _ = reader.GetByte(0);
                }
                else if (Object.ReferenceEquals(t, __bytesType))
                {
                    _ = reader.GetBytes(0, 0, new byte[0], 0, 100000);
                }
                else if (Object.ReferenceEquals(t, __charType))
                {
                    _ = reader.GetChar(0);
                }
                else if (Object.ReferenceEquals(t, __charsType))
                {
                    _ = reader.GetChars(0, 0, new char[0], 0, 100000);
                }
                else if (Object.ReferenceEquals(t, __dateTimeType))
                {
                    _ = reader.GetDateTime(0);
                }
                else if (Object.ReferenceEquals(t, __dateTimeOffsetType))
                {
                    _ = reader.GetDateTimeOffset(0);
                }
                else if (Object.ReferenceEquals(t, __decimalType))
                {
                    _ = reader.GetDecimal(0);
                }
                else if (Object.ReferenceEquals(t, __doubleType))
                {
                    _ = reader.GetDouble(0);
                }
                else if (Object.ReferenceEquals(t, __floatType))
                {
                    _ = reader.GetFloat(0);
                }
                else if (Object.ReferenceEquals(t, __stringType))
                {
                    _ = reader.GetString(0);
                }
                else if (Object.ReferenceEquals(t, __boolType))
                {
                    _ = reader.GetBoolean(0);
                }
                else if (Object.ReferenceEquals(t, __shortType) || Object.ReferenceEquals(t, __ushortType))
                {
                    _ = reader.GetInt16(0);
                }
                else if (Object.ReferenceEquals(t, __longType) || Object.ReferenceEquals(t, __ulongType))
                {
                    _ = reader.GetInt64(0);
                }
                else if (Object.ReferenceEquals(t, __timeSpanType))
                {
                    _ = reader.GetTimeSpan(0);
                }

                else
                {
                    //reader[0]
                }
            });

            CodeTimer.Time("this[]", execCount, () =>
            {
                _ = (Guid)reader[0];
            });

            CodeTimer.Time("GetValue", execCount, () =>
            {
                _ = (Guid)reader.GetValue(0);
            });

            CodeTimer.Time("GetFieldValue<Guid>", execCount, () =>
            {
                _ = reader.GetFieldValue<Guid>(0);
            });

            reader.Close();
        }

        private static void ReadStringFromReader(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();
            var reader = (SqlDataReader)db.GetReader("SELECT [StringGuid] FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", id));

            reader.Read();

            Console.WriteLine($"GetString:" + reader.GetString(0));
            Console.WriteLine($"this[]:" + reader[0]);
            Console.WriteLine($"GetValue" + reader.GetValue(0));
            Console.WriteLine($"GetFieldValue<string>:" + reader.GetFieldValue<string>(0));

            int execCount = 10000000;
            CodeTimer.Time("GetString", execCount, () =>
            {
                Type t = typeof(String);


                if (Object.ReferenceEquals(t, __intType) || Object.ReferenceEquals(t, __uintType))
                {
                    _ = reader.GetInt32(0);
                }
                else if (Object.ReferenceEquals(t, __guidType))
                {
                    _ = reader.GetGuid(0);
                }
                else if (Object.ReferenceEquals(t, __byteType) || Object.ReferenceEquals(t, __sbyteType))
                {
                    _ = reader.GetByte(0);
                }
                else if (Object.ReferenceEquals(t, __bytesType))
                {
                    _ = reader.GetBytes(0, 0, new byte[0], 0, 100000);
                }
                else if (Object.ReferenceEquals(t, __charType))
                {
                    _ = reader.GetChar(0);
                }
                else if (Object.ReferenceEquals(t, __charsType))
                {
                    _ = reader.GetChars(0, 0, new char[0], 0, 100000);
                }
                else if (Object.ReferenceEquals(t, __dateTimeType))
                {
                    _ = reader.GetDateTime(0);
                }
                else if (Object.ReferenceEquals(t, __dateTimeOffsetType))
                {
                    _ = reader.GetDateTimeOffset(0);
                }
                else if (Object.ReferenceEquals(t, __decimalType))
                {
                    _ = reader.GetDecimal(0);
                }
                else if (Object.ReferenceEquals(t, __doubleType))
                {
                    _ = reader.GetDouble(0);
                }
                else if (Object.ReferenceEquals(t, __floatType))
                {
                    _ = reader.GetFloat(0);
                }
                else if (Object.ReferenceEquals(t, __stringType))
                {
                    _ = reader.GetString(0);
                }
                else if (Object.ReferenceEquals(t, __boolType))
                {
                    _ = reader.GetBoolean(0);
                }
                else if (Object.ReferenceEquals(t, __shortType) || Object.ReferenceEquals(t, __ushortType))
                {
                    _ = reader.GetInt16(0);
                }
                else if (Object.ReferenceEquals(t, __longType) || Object.ReferenceEquals(t, __ulongType))
                {
                    _ = reader.GetInt64(0);
                }
                else if (Object.ReferenceEquals(t, __timeSpanType))
                {
                    _ = reader.GetTimeSpan(0);
                }

                else
                {
                    //reader[0]
                }

            });

            CodeTimer.Time("this[]", execCount, () =>
            {
                _ = (string)reader[0];
            });

            CodeTimer.Time("GetValue", execCount, () =>
            {
                _ = (string)reader.GetValue(0);
            });

            CodeTimer.Time("GetFieldValue<string>", execCount, () =>
            {
                _ = reader.GetFieldValue<string>(0);
            });

            reader.Close();
        }

        private static void ReadDateTimeFromReader(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();
            var reader = (SqlDataReader)db.GetReader("SELECT [CreatedTime] FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", id));

            reader.Read();

            Console.WriteLine($"GetDateTime:" + reader.GetDateTime(0));
            Console.WriteLine($"this[]:" + reader[0]);
            Console.WriteLine($"GetValue" + reader.GetValue(0));
            Console.WriteLine($"GetFieldValue<string>:" + reader.GetFieldValue<DateTime>(0));

            int execCount = 10000000;
            CodeTimer.Time("GetDateTime", execCount, () =>
            {
                Type t = typeof(DateTime);


                if (Object.ReferenceEquals(t, __intType) || Object.ReferenceEquals(t, __uintType))
                {
                    _ = reader.GetInt32(0);
                }
                else if (Object.ReferenceEquals(t, __guidType))
                {
                    _ = reader.GetGuid(0);
                }
                else if (Object.ReferenceEquals(t, __byteType) || Object.ReferenceEquals(t, __sbyteType))
                {
                    _ = reader.GetByte(0);
                }
                else if (Object.ReferenceEquals(t, __bytesType))
                {
                    _ = reader.GetBytes(0, 0, new byte[0], 0, 100000);
                }
                else if (Object.ReferenceEquals(t, __charType))
                {
                    _ = reader.GetChar(0);
                }
                else if (Object.ReferenceEquals(t, __charsType))
                {
                    _ = reader.GetChars(0, 0, new char[0], 0, 100000);
                }
                else if (Object.ReferenceEquals(t, __dateTimeType))
                {
                    _ = reader.GetDateTime(0);
                }
                else if (Object.ReferenceEquals(t, __dateTimeOffsetType))
                {
                    _ = reader.GetDateTimeOffset(0);
                }
                else if (Object.ReferenceEquals(t, __decimalType))
                {
                    _ = reader.GetDecimal(0);
                }
                else if (Object.ReferenceEquals(t, __doubleType))
                {
                    _ = reader.GetDouble(0);
                }
                else if (Object.ReferenceEquals(t, __floatType))
                {
                    _ = reader.GetFloat(0);
                }
                else if (Object.ReferenceEquals(t, __stringType))
                {
                    _ = reader.GetString(0);
                }
                else if (Object.ReferenceEquals(t, __boolType))
                {
                    _ = reader.GetBoolean(0);
                }
                else if (Object.ReferenceEquals(t, __shortType) || Object.ReferenceEquals(t, __ushortType))
                {
                    _ = reader.GetInt16(0);
                }
                else if (Object.ReferenceEquals(t, __longType) || Object.ReferenceEquals(t, __ulongType))
                {
                    _ = reader.GetInt64(0);
                }
                else if (Object.ReferenceEquals(t, __timeSpanType))
                {
                    _ = reader.GetTimeSpan(0);
                }

                else
                {
                    //reader[0]
                }
            });

            CodeTimer.Time("this[]", execCount, () =>
            {
                _ = (DateTime)reader[0];
            });

            CodeTimer.Time("GetValue", execCount, () =>
            {
                _ = (DateTime)reader.GetValue(0);
            });

            CodeTimer.Time("GetFieldValue<string>", execCount, () =>
            {
                _ = reader.GetFieldValue<DateTime>(0);
            });

            reader.Close();
        }

        private static void ScalarToInt<T>(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();

            var task = db.GetScalarAsync<T>("SELECT [Int] FROM [TA] WHERE [Guid] = @F1", CommandType.Text, null,
                db.BuildParameter("@F1", id));
            var syncValue = db.GetScalar<T>("SELECT [Int] FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", id));
            var asyncValue = task.Result;

            Console.WriteLine($"AsyncValue:{asyncValue}，syncValue:{syncValue}，result：{(asyncValue.Equals(syncValue) ? "equals": "not equals")}.");
        }

        
        private static void ScalarToGuid<T>(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();
            
            var task = db.GetScalarAsync<T>("SELECT [Guid] FROM [TA] WHERE [Guid] = @F1", CommandType.Text, null,
                db.BuildParameter("@F1", id));

            var syncValue = db.GetScalar<T>("SELECT [Guid] FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", id));

            var asyncValue = task.Result;

            Console.WriteLine($"AsyncValue:{asyncValue}，syncValue:{syncValue}，result：{(asyncValue.Equals(syncValue) ? "equals" : "not equals")}.");
        }

        private static void ScalarToGuidForString<T>(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();
            var p = db.BuildParameter("@F1", id);



            var syncValue = db.GetScalar<T>("SELECT [StringGuid] FROM [TA] WHERE [Guid] = @F1", CommandType.Text, p);

            var task = db.GetScalarAsync<T>("SELECT [StringGuid] FROM [TA] WHERE [Guid] = @F1", CommandType.Text, null,
                p);
            var asyncValue = task.Result;

            Console.WriteLine($"AsyncValue:{asyncValue}，syncValue:{syncValue}，result：{(asyncValue.Equals(syncValue) ? "equals" : "not equals")}.");
        }


        private static void ScalarToString(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();

            var task = db.GetScalarAsync<string>("SELECT [StringGuid] FROM [TA] WHERE [Guid] = @F1", CommandType.Text, null,
                db.BuildParameter("@F1", id));

            var syncValue = db.GetScalar<string>("SELECT [StringGuid] FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", id));

            var asyncValue = task.Result;

            Console.WriteLine($"AsyncValue:{asyncValue}，syncValue:{syncValue}，result：{(String.Equals(asyncValue,syncValue) ? "equals" : "not equals")}.");
        }

        private static void ScalarToStringForGuid(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();

            var task = db.GetScalarAsync<string>("SELECT [Guid] FROM [TA] WHERE [Guid] = @F1", CommandType.Text, null,
                db.BuildParameter("@F1", id));

            var syncValue = db.GetScalar<string>("SELECT [Guid] FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", id));

            var asyncValue = task.Result;

            Console.WriteLine($"AsyncValue:{asyncValue}，syncValue:{syncValue}，result：{(String.Equals(asyncValue, syncValue) ? "equals" : "not equals")}.");
        }




        private static void ScalarToDateTime<T>(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();

            var task = db.GetScalarAsync<T>("SELECT [CreatedTime] FROM [TA] WHERE [Guid] = @F1", CommandType.Text, null,
                db.BuildParameter("@F1", id));

            var syncValue = db.GetScalar<T>("SELECT [CreatedTime] FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", id));

            var asyncValue = task.Result;

            Console.WriteLine($"AsyncValue:{asyncValue}，syncValue:{syncValue}，result：{(asyncValue.Equals(syncValue) ? "equals" : "not equals")}.");
        }



        private static void ScalarToBytes(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();
            var p = db.BuildParameter("@F1", id);



            var syncValue = db.GetScalar<byte[]>("SELECT [IP] FROM [TA] WHERE [Guid] = @F1", CommandType.Text, p);

            var task = db.GetScalarAsync<byte[]>("SELECT [IP] FROM [TA] WHERE [Guid] = @F1", CommandType.Text, null,
                p);
            var asyncValue = task.Result;

            if (asyncValue == null) asyncValue = new byte[0];
            if (syncValue == null) syncValue = new byte[0];

            Console.WriteLine($"AsyncValue:{String.Join(".", asyncValue)}，syncValue:{String.Join(".", syncValue)}，result：{(String.Join(".", asyncValue).Equals(String.Join(".", syncValue)) ? "equals" : "not equals")}.");
        }

        private static void ScalarToIPAddressForBytes(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();

            var task = db.GetScalarAsync<IPAddress>("SELECT [IP] FROM [TA] WHERE [Guid] = @F1", CommandType.Text, null,
                db.BuildParameter("@F1", id));

            var syncValue = db.GetScalar<IPAddress>("SELECT [IP] FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", id));

            var asyncValue = task.Result;

            if (asyncValue == null) asyncValue = IPAddress.Loopback;
            if (syncValue == null) syncValue = IPAddress.Loopback;

            Console.WriteLine($"AsyncValue:{asyncValue}，syncValue:{syncValue}，result：{(asyncValue.Equals(syncValue) ? "equals" : "not equals")}.");
        }


        private static void ScalarToIPAddressForString(Guid id)
        {
            SqlServerUtility db = new SqlServerUtility();

            var task = db.GetScalarAsync<IPAddress>("SELECT [StringIP] FROM [TA] WHERE [Guid] = @F1", CommandType.Text, null,
                db.BuildParameter("@F1", id));

            var syncValue = db.GetScalar<IPAddress>("SELECT [StringIP] FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", id));

            var asyncValue = task.Result;

            if (asyncValue == null) asyncValue = IPAddress.Loopback;
            if (syncValue == null) syncValue = IPAddress.Loopback;

            Console.WriteLine($"AsyncValue:{asyncValue}，syncValue:{syncValue}，result：{(asyncValue.Equals(syncValue) ? "equals" : "not equals")}.");
        }


        private static void GetSingle<TIP>(Guid id) where TIP : class
        {
            SqlServerUtility db = new SqlServerUtility();

            var task = db.GetSingleAsync<TestModel<TIP>>("SELECT * FROM [TA] WHERE [Guid] = @F1", CommandType.Text, null,
                db.BuildParameter("@F1", id));

            var syncValue = db.GetSingle<TestModel<TIP>>("SELECT * FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", id));

            var asyncValue = task.Result;

            if (asyncValue == null)
            {
                Console.WriteLine($"异步请求{nameof(GetSingle)}为 null。");
            }
            else
            {
                Console.Write($"AsyncValue:{asyncValue}");
            }

            if (syncValue == null)
            {
                Console.Write($"同步请求{nameof(GetSingle)}为 null。");
            }
            else
            {
                Console.WriteLine($"syncValue:{syncValue}");
            }

            if (asyncValue != null && syncValue != null)
            {
                Console.Write($"result：{(asyncValue.Equals(syncValue) ? "equals" : "not equals")}.");
            }
            Console.WriteLine();
        }

        private static void GetList<TIP>(params Guid[] id) where TIP : class
        {
            SqlServerUtility db = new SqlServerUtility();
            var idGroup = db.GetSqlInExpressions(id);

            var task = db.GetListAsync<TestModel<TIP>>("SELECT * FROM [TA] WHERE [Guid] IN (" + idGroup[0] + ")");

            var syncValue = db.GetList<TestModel<TIP>>("SELECT * FROM [TA] WHERE [Guid] IN (" + idGroup[0] + ")");

            var asyncValue = task.Result;

            if (asyncValue == null)
            {
                Console.WriteLine($"异步请求{nameof(GetList)}为 null。");
            }
            else
            {
                Console.WriteLine("AsyncValue:");
                Console.WriteLine(asyncValue.GetType());
                for (int i=0;i< asyncValue.Count; i++)
                {
                    Console.Write($"[{i}] {asyncValue[i]}");
                }
            }

            if (syncValue == null)
            {
                Console.Write($"同步请求{nameof(GetList)}为 null。");
            }
            else
            {
                Console.WriteLine("SyncValue:");
                Console.WriteLine(syncValue.GetType());
                for (int i = 0; i < syncValue.Count; i++)
                {
                    Console.Write($"[{i}] {syncValue[i]}");
                }
            }

            Console.WriteLine();
        }

        private static void GetSet<TIP>(params Guid[] id) where TIP : class
        {
            SqlServerUtility db = new SqlServerUtility();
            var idGroup = db.GetSqlInExpressions(id);

            var task = db.GetSetAsync<TestModel<TIP>>("SELECT * FROM [TA] WHERE [Guid] IN (" + idGroup[0] + ")");

            var syncValue = db.GetSet<TestModel<TIP>>("SELECT * FROM [TA] WHERE [Guid] IN (" + idGroup[0] + ")");

            var asyncValue = task.Result;

            if (asyncValue == null)
            {
                Console.WriteLine($"异步请求{nameof(GetSet)}为 null。");
            }
            else
            {
                Console.WriteLine("AsyncValue:");
                Console.WriteLine(asyncValue.GetType());
                int i = 0;
                foreach (var item in asyncValue)
                {
                    Console.Write($"[{i++}] {item}");
                }
            }

            if (syncValue == null)
            {
                Console.Write($"同步请求{nameof(GetSet)}为 null。");
            }
            else
            {
                Console.WriteLine("SyncValue:");
                Console.WriteLine(syncValue.GetType());
                int i = 0;
                foreach (var item in syncValue)
                {
                    Console.Write($"[{i++}] {item}");
                }
            }

            Console.WriteLine();
        }

        private static void GetCollection<TIP>(params Guid[] id) where TIP : class
        {
            SqlServerUtility db = new SqlServerUtility();
            var idGroup = db.GetSqlInExpressions(id);

            var task = db.GetCollectionAsync<TestModel<TIP>>("SELECT * FROM [TA] WHERE [Guid] IN (" + idGroup[0] + ")");

            var syncValue = db.GetCollection<TestModel<TIP>>("SELECT * FROM [TA] WHERE [Guid] IN (" + idGroup[0] + ")");

            var asyncValue = task.Result;

            if (asyncValue == null)
            {
                Console.WriteLine($"异步请求{nameof(GetCollection)}为 null。");
            }
            else
            {
                Console.WriteLine("AsyncValue:");
                Console.WriteLine(asyncValue.GetType());
                int i = 0;
                foreach (var item in asyncValue)
                {
                    Console.Write($"[{i++}] {item}");
                }
            }

            if (syncValue == null)
            {
                Console.Write($"同步请求{nameof(GetCollection)}为 null。");
            }
            else
            {
                Console.WriteLine("SyncValue:");
                Console.WriteLine(syncValue.GetType());
                int i = 0;
                foreach(var item in syncValue)
                {
                    Console.Write($"[{i++}] {item}");
                }
            }

            Console.WriteLine();
        }



        static Type __intType = typeof(int);
        static Type __uintType = typeof(uint);
        static Type __byteType = typeof(byte);
        static Type __sbyteType = typeof(sbyte);
        static Type __bytesType = typeof(byte[]);
        static Type __charType = typeof(char);
        static Type __charsType = typeof(char[]);
        static Type __dateTimeType = typeof(DateTime);
        static Type __dateTimeOffsetType = typeof(DateTimeOffset);
        static Type __decimalType = typeof(decimal);
        static Type __doubleType = typeof(double);
        static Type __floatType = typeof(float);
        static Type __guidType = typeof(Guid);
        static Type __stringType = typeof(string);
        static Type __boolType = typeof(bool);
        static Type __shortType = typeof(short);
        static Type __ushortType = typeof(ushort);
        static Type __longType = typeof(long);
        static Type __ulongType = typeof(ulong);
        static Type __timeSpanType = typeof(TimeSpan);
    }


    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class TestDataReaderReadInt
    {
        public System.Data.Common.DbDataReader _reader;

        [GlobalSetup]
        public void GlobalSetup()
        {
            SqlServerUtility.Initialize(new string[]
            {
                ConfigurationManager.ConnectionStrings["db-b"].ConnectionString
            }, new string[]{
                ConfigurationManager.ConnectionStrings["db-a"].ConnectionString
            });

            SqlServerUtility db = new SqlServerUtility();
            _reader = (SqlDataReader)db.GetReader("SELECT [Int],[Guid],[StringGuid],[CreatedTime] FROM [TA] WHERE [Guid] = @F1", CommandType.Text,
                db.BuildParameter("@F1", Guid.Parse("F880ED06-8C51-4BE5-B4F7-AB3DF2599D73")));
            _reader.Read();

            Console.WriteLine("初始化完成。");
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _reader.Close();
            Console.WriteLine("清扫工作完成。");
        }



        [Benchmark]
        public void GetInt32()
        {
            _ = _reader.GetInt32(0);
        }

        [Benchmark]
        public void GetFieldValue_Int()
        {
            _ = _reader.GetFieldValue<int>(0);
        }

        [Benchmark]
        public void GetValue_Int()
        {
            _ = (int)_reader.GetValue(0);
        }

        [Benchmark]
        public void This_Int()
        {
            _ = (int)_reader[0];
        }



        [Benchmark]
        public void GetGuid()
        {
            _ = _reader.GetGuid(1);
        }

        [Benchmark]
        public void GetFieldValue_Guid()
        {
            _ = _reader.GetFieldValue<Guid>(1);
        }

        [Benchmark]
        public void GetValue_Guid()
        {
            _ = (Guid)_reader.GetValue(1);
        }

        [Benchmark]
        public void This_Guid()
        {
            _ = (Guid)_reader[1];
        }



        [Benchmark]
        public void GetString()
        {
            _ = _reader.GetString(2);
        }

        [Benchmark]
        public void GetFieldValue_String()
        {
            _ = _reader.GetFieldValue<string>(2);
        }

        [Benchmark]
        public void GetValue_String()
        {
            _ = (string)_reader.GetValue(2);
        }

        [Benchmark]
        public void This_String()
        {
            _ = (string)_reader[2];
        }




        [Benchmark]
        public void GetDateTime()
        {
            _ = _reader.GetDateTime(3);
        }

        [Benchmark]
        public void GetFieldValue_DateTime()
        {
            _ = _reader.GetFieldValue<DateTime>(3);
        }

        [Benchmark]
        public void GetValue_DateTime()
        {
            _ = (DateTime)_reader.GetValue(3);
        }

        [Benchmark]
        public void This_DateTime()
        {
            _ = (DateTime)_reader[3];
        }
    }


    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class TestMemory
    {
        [Benchmark]
        public object NewObj()
        {
            return new object();
        }

        [Benchmark]
        public object Box()
        {
            return 1024;
        }

        [Benchmark]
        public int Value()
        {
            return 1024;
        }
    }

    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class TestObject
    {
        TestClass<Guid> _test;
        IList<int> _list;
        Func<Guid> _fun;

        public TestObject()
        {
            _test = new TestClass<Guid>();
            _list = new List<int>();
            _fun = () => Guid.NewGuid();
        }

        
        [Benchmark]
        public void Definite()
        {
            //_test.SetInstance(_list);

            //_test.UseDefinite();

            _test.SetFunc(_fun);

            _test.CallDefinite();
        }

        [Benchmark]
        public void Dynamic()
        {
            //_test.SetInstance<int>(_list);

            //_test.UseDynamic();

            _test.SetFunc<Guid>(_fun);

            _test.CallDynamic();
        }

        [Benchmark]
        public void Object()
        {
            //_test.SetInstanceObject(_list);7

            //_test.UseObject();

            _test.SetFuncObject(_fun);

            _test.CallObject();
        }

        [Benchmark]
        public void Direct()
        {
            //_test.UseDirect(_list);

            _test.CallDirect(() => Guid.NewGuid());
        }
    }

    public class TestClass<T>
    {
        Func<T> _fun;
        dynamic _funDynamic;
        object _funObject;

        IList<T> _listInstance;
        dynamic _dynamicInstance;
        object _objectInstance;


        public void SetInstance(IList<T> list) => _listInstance = list;

        public void SetInstance<T1>(IList<T1> list) => _dynamicInstance = list;

        public void SetInstanceObject<T1>(IList<T1> list) => _objectInstance = list;



        public void SetFunc(Func<T> fun) => _fun = fun;

        public void SetFunc<T1>(Func<T1> fun) => _funDynamic = fun;

        public void SetFuncObject<T1>(Func<T1> fun) => _funObject = fun;


        public int UseDefinite() => _listInstance.Count;

        public int UseDynamic() => _dynamicInstance.Count;

        public int UseObject() => ((IList<T>)_objectInstance).Count;

        public int UseDirect(IList<T> list) => list.Count;



        public T CallDefinite() => _fun();

        public T CallDynamic() => _funDynamic();

        public T CallObject() => ((Func<T>)_funObject)();

        public T CallDirect(Func<T> fun) => fun();

    }


    public class TestModel<TIP> where TIP : class
    {
        public Guid Guid { get; set; }

        public int Int { get; set; }

        public string StringGuid { get; set; }

        public string ShortGuid { get; set; }

        public TIP IP { get; set; }

        public string StringIP { get; set; }

        public DateTime CreatedTime { get; set; }

        public bool Equals(TestModel<TIP> model)
        {
            return this.Guid == model.Guid
                && this.Int == model.Int
                && this.StringGuid == model.StringGuid
                && this.ShortGuid == model.ShortGuid
                && this.IP == this.IP
                && this.StringIP == model.StringIP
                && this.CreatedTime == model.CreatedTime;
        }

        public override string ToString()
        {
            if (typeof(TIP) == typeof(byte[]))
            {
                return $"\n{{\n\tGuid:{Guid},\n\tInt:{Int},\n\tStringGuid:{StringGuid},\n\tShortGuid:{ShortGuid},\n\tIP:{String.Join(".",(byte[])(object)IP)},\n\tStringIP:{StringIP},\n\tCreateTime:{CreatedTime}\n}}\n";
            }
            else
            {
                return $"\n{{\n\tGuid:{Guid},\n\tInt:{Int},\n\tStringGuid:{StringGuid},\n\tShortGuid:{ShortGuid},\n\tIP:{IP},\n\tStringIP:{StringIP},\n\tCreateTime:{CreatedTime}\n}}\n";
            }
        }
    }
}
