using Sweety.Common.DataProvider;
using Sweety.Common.DataProvider.PostgreSQL;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {

            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //Encoding.GetEncoding("GB18030")

            Npgsql.NpgsqlConnectionStringBuilder connStrBuilder = new Npgsql.NpgsqlConnectionStringBuilder
            {
                Host = "192.168.1.126",
                Port = 15432,
                Username = "postgres",
                Password = "myundb",
                //Database = "oidc",
                Database = "organization",
                Pooling = true,
                MinPoolSize = 0,
                MaxPoolSize = 100,
                //ClientEncoding = Encoding.Unicode.EncodingName,
                //Encoding = System.Text.Encoding.ASCII.EncodingName
            };

            string connStr = connStrBuilder.ConnectionString;

            RelationalDBUtilityBase.Initialize(new string[] {
                //"Host=192.168.1.126;Port=15432;Username=postgres;Password=myundb;Database=oidc;Pooling=True;Minimum Pool Size=0;Maximum Pool Size=100;Client Encoding=UNICODE;Encoding=UTF-8"
                
                connStr
            });

            byte[] password = new byte[] { 0x00, 0xDE, 0xE0, 0xFB, 0xA3, 0x1D, 0x8C, 0x35, 0x30, 0xDE, 0x47, 0x70, 0x29, 0x92, 0xC4, 0x71, 0x1C, 0x74, 0xDB, 0x43, 0xD1, 0x56, 0x66, 0x6B, 0x79, 0x64, 0x77, 0x4A, 0x35, 0x62, 0xA1, 0x7C, 0xA1, 0xE7, 0xE4, 0x47, 0x05, 0x65, 0x35, 0x42, 0x4D, 0x35, 0x4B, 0x1A, 0xCF, 0x27, 0x0E, 0x52, 0x7A, 0x1B, 0x30, 0xBA, 0x9B, 0xB2, 0x6C, 0x8F, 0x80, 0x44, 0x01, 0x00, 0xEC, 0x06, 0x96, 0x8E, 0x60, 0xF5, 0x92, 0x91, 0xD5, 0xF3, 0xD5, 0x6C, 0x11, 0xE2, 0x6F, 0xDB, 0x11, 0x3E, 0xFA, 0x7E, 0xEF, 0xA6, 0x78, 0x52, 0xDB, 0xE7, 0x3E, 0x72, 0xE8, 0x58, 0x28, 0xAA, 0x36, 0x4D, 0xF9, 0x46, 0xC7 };

            RelationalDBUtilityBase dbUtility = new PostgreSQLUtility();

            /*const string sql = "INSERT INTO public.\"UserExtension\" (\"UserId\", \"Item\", \"Value\", \"UpdatedAt\", \"CreatedAt\") VALUES (@UserId, @Item0, @Value0, @CreatedAt, @CreatedAt),(@UserId, @Item1, @Value1, @CreatedAt, @CreatedAt) ";

            var paramArr = new IDbDataParameter[]
            {
                dbUtility.BuildParameter("UserId", new byte[]{ 1,2,3,4,5,6,7,8,9,10,11,12}),
                dbUtility.BuildParameter("createdAt", DateTime.UtcNow),
                dbUtility.BuildParameter("Item0", (short)1),
                dbUtility.BuildParameter("value0", "this is #value0"),
                dbUtility.BuildParameter("item1", (short)2),
                dbUtility.BuildParameter("value1", "this is #value1"),
                null,
                null,
                null
            };

            var tran = dbUtility.BuildTransaction();
            var itemIndex = await dbUtility.ExecuteNonQueryAsync(tran, sql, parameters: paramArr);
            var conn = tran.Connection!;
            tran.Commit();
            conn.Dispose();*/

            var param1 = dbUtility.BuildParameter("@arg2", Array.Empty<byte>());
            var param2 = dbUtility.BuildParameter("@arg3", String.Empty);
            param1.Direction = param2.Direction = ParameterDirection.Output;
            int iiii = await dbUtility.ExecuteNonQueryAsync("public.outArg", CommandType.StoredProcedure, parameters: new IDataParameter[] { param1, param2 });
            if (iiii > 0)
            {

            }
            if (param1.Value != DBNull.Value || param2.Value != DBNull.Value)
            {

            }



            int result = 0;

            #region User
            /*
            result = await dbUtility.ExecuteNonQueryAsync("DELETE FROM \"User\" WHERE \"Id\"=@Id", CommandType.Text, null,
                dbUtility.BuildParameter("@Id", "U6374489679292883323"));

            if (result != 1)
            {
                Console.WriteLine($"删除数据结果：{result}。");
            }
            

            result = await dbUtility.ExecuteNonQueryAsync("INSERT INTO \"User\" VALUES (@Id, @LoginName, @Email, @Mobile, @Password, @Nickname, @Avatar, @LoginTimes, @LastLoginIp, @LastLoginTime, @State, @From, @UnitId, @SignUpIp, @UpdatedAt, @CreatedAt)"
                , CommandType.Text
                , null
                , dbUtility.BuildParameter("@Id", "U6374489679292883323")
                , dbUtility.BuildParameter("@LoginName", "qinmingyun")
                , dbUtility.BuildParameter("@Email", "myun18@qq.com")
                , dbUtility.BuildParameter("@Mobile", "18610294377")
                , dbUtility.BuildParameter("@Password", password)
                , dbUtility.BuildParameter("@Nickname", "✓ MYUN✔")
                , dbUtility.BuildParameter("@Avatar", "https://uploadfile.qikan.com/files/b2b/user/2018/06/04/24123fcf-a086-4701-bf9f-250abc541d69.jpg")
                , dbUtility.BuildParameter("@LoginTimes", 64)
                , dbUtility.BuildParameter("@LastLoginIp", "221.213.134.21")
                , dbUtility.BuildParameter("@LastLoginTime", new DateTime(2020, 12, 15, 12, 20, 37, DateTimeKind.Local))
                , dbUtility.BuildParameter("@State", 1L)
                , dbUtility.BuildParameter("@From", (byte)1)
                , dbUtility.BuildParameter("@UnitId", "500001")
                , dbUtility.BuildParameter("@SignUpIp", "221.213.134.21")
                , dbUtility.BuildParameter("@UpdatedAt", new DateTime(2018, 06, 12, 14, 05, 50, DateTimeKind.Local))
                , dbUtility.BuildParameter("@CreatedAt", new DateTime(2017, 04, 11, 15, 30, 55, DateTimeKind.Local)));

            if (result != 1)
            {
                Console.WriteLine($"插入数据结果：{result}。");
            }
            

            result = await dbUtility.ExecuteNonQueryAsync("UPDATE public.\"User\" SET \"Nickname\"=@Nickname WHERE \"Id\"=@Id", CommandType.Text, null
                , dbUtility.BuildParameter("@Nickname", "✓ MYUN✔")
                , dbUtility.BuildParameter("@Id", "U6374489679292883323"));

            if (result != 1)
            {
                Console.WriteLine($"修改数据结果：{result}。");
            }
            */
            #endregion


            /*UserEntity2 entity = await dbUtility.GetSingleAsync<UserEntity2>("SELECT * FROM \"User\" WHERE \"Id\"=@Id", CommandType.Text, null,
                dbUtility.BuildParameter("@Id", "U6374489679292883323"));

            if (entity != null)
            {
                if (password.SlowEquals(entity.Password))
                {
                    Console.WriteLine($"{entity.Nickname}，密码存取一致。");
                }
            }*/

            //await TestMore(dbUtility);

            Console.WriteLine("Hello World!");
            Console.Read();
        }

        static async ValueTask TestMore(IRelationalDBUtility dbUtility)
        {
            int result;
            /*
                        Guid uuid = Guid.NewGuid();
                        DateTime now = DateTime.Now;
                        string text = "阿尔卑斯山";
                        result = await dbUtility.ExecuteNonQueryAsync("INSERT INTO public.\"TestUUID\" (\"Id\", \"Time\", \"Timez\", \"Date\", \"DateTime\", \"DateTimez\", \"VarcharDefault\", \"VarcharZhHansCNxIcu\") VALUES (@Id, @Time, @Timez, @now, @now, @DateTimez, @text, @text);", CommandType.Text, null
                            , dbUtility.BuildParameter("@Id", uuid)
                            , dbUtility.BuildParameter("@time", now)
                            , dbUtility.BuildParameter("@now", now)
                            , new Npgsql.NpgsqlParameter("@Timez", NpgsqlTypes.NpgsqlDbType.TimeTz) { Value = now }
                            , new Npgsql.NpgsqlParameter("@DateTimez", NpgsqlTypes.NpgsqlDbType.TimestampTz) { Value = now }
                            , dbUtility.BuildParameter("@text", text));

                        if (result != 1)
                        {
                            Console.WriteLine($"插入数据结果：{result}。");
                        }
                        else
                        {
                            Console.WriteLine($"uuid:{uuid}\tnow:{now:yyyy-MM-dd HH:mm:ss.fffffff}\ttext:{text}");
                        }*/


            Guid uuid = Guid.Parse("44979848-3b84-4d3c-8541-b0db6e79707d");
            TestUUIDEntity entity = await dbUtility.GetSingleAsync<TestUUIDEntity>("SELECT * FROM public.\"TestUUID\" WHERE \"Id\"=@Id", CommandType.Text, null, dbUtility.BuildParameter("@Id", uuid));
            if (entity != null)
            {
                if (entity.Id == uuid)
                {
                    Console.WriteLine($"{entity.Id}，UUID存取一致。");
                }

                Console.WriteLine($"Serial:{entity.Serial}\t SmallSerial:{entity.SmallSerial}\t Serial2:{entity.Serial2}\t Serial8:{entity.Serial8}");
                Console.WriteLine($"Time:{entity.Time:G}\t Timez:{entity.Timez:yyyy-MM-dd HH:mm:ss.fffffff}");
                Console.WriteLine($"Date:{entity.Date:yyyy-MM-dd HH:mm:ss.fffffff}\t DateTimez:{entity.DateTimez:yyyy-MM-dd HH:mm:ss.fffffff}\t DateTime:{entity.DateTime:yyyy-MM-dd HH:mm:ss.fffffff}");
            }
        }
    }

    public class TestUUIDEntity
    {
        public Guid Id { get; set; }

        public TimeSpan Time { get; set; }

        public DateTimeOffset Timez { get; set; }

        public DateTime Date { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime DateTimez { get; set; }

        public int Serial { get; set; }

        public short SmallSerial { get; set; }

        public short Serial2 { get; set; }

        public long Serial8 { get; set; }

        public string VarcharDefault { get; set; }

        public string VarcharZhHansCNxIcu { get; set; }
    }

    public class UserEntity
    {
        public string Id { get; set; }

        public string LoginName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public byte[] Password { get; set; }

        public string Nickname { get; set; }

        public string Avatar { get; set; }

        public int LoginTimes { get; set; }

        public string LastLoginIp { get; set; }

        public DateTime LastLoginTime { get; set; }

        public long State { get; set; }

        public byte From { get; set; }

        public string UnitId { get; set; }

        public string SignUpIp { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class UserEntity2
    {
        public string Id { get; set; }

        public string LoginName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public byte[] Password { get; set; }

        public string Nickname { get; set; }

        public string Avatar { get; set; }

        public int LoginTimes { get; set; }

        public string LastLoginIp { get; set; }

        public DateTime LastLoginTime { get; set; }

        public UserState State { get; set; }

        public UserFromType From { get; set; }

        public string UnitId { get; set; }

        public string SignUpIp { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 用户状态
    /// </summary>
    [Flags]
    public enum UserState : byte
    {
        不限 = 0x00,
        正常 = 0x01,
        禁用 = 0x02,
        不允许自己修改密码 = 0x04,

        全部 = 正常 | 禁用 | 不允许自己修改密码
    }

    public enum UserFromType : byte
    {
        注册用户 = 1,
        统一认证 = 2,
        QQ用户 = 3,
        微信用户 = 4,
        微博用户 = 5,
        管理员创建用户 = 6,
        原库移植用户 = 7
    }
}
