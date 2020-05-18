using System;

using Xunit;

using Sweety.Common.Extensions;


namespace Sweety.Common.Tests.Extensions
{
    public class DateTimeExtensionsTest
    {
        [Fact]
        public void NowUnixTimestamp_GET()
        {
            long expected = (DateTime.UtcNow.Ticks - DateTimeExtensions.UNIX_MIN_TIME.Ticks) / TimeSpan.TicksPerSecond;
            long actual = DateTimeExtensions.NowUnixTimestamp;
            if (expected != actual)
            {
                expected = (DateTime.UtcNow.Ticks - DateTimeExtensions.UNIX_MIN_TIME.Ticks) / TimeSpan.TicksPerSecond;
                actual = DateTimeExtensions.NowUnixTimestamp;
            }

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void FirstDayOfMonth()
        {
            DateTime dt = DateTime.Now;
            if (dt.Day == 1)
            {
                dt.AddDays(18D);
            }

            DateTime expected = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, dt.Kind);
            DateTime actual = dt.FirstDayOfMonth();

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void LastDayOfMonth()
        {
            Random rand = new Random();
            DateTime dt = new DateTime(2019, 1, 1, 8, 30, 20);
            
            for (int i = 0; i < 24; i++) //24个月，从2019年1月开始到2020年12月，2020年刚好是闰年。
            {
                DateTime expected = dt.AddMonths(1).AddDays(-(dt.Day));

                DateTime actual = dt.LastDayOfMonth();

                Assert.NotEqual(expected, actual);

                Assert.Equal(expected.Date, actual);

                dt = actual.AddDays(rand.Next(1, 27)).AddSeconds(rand.Next(1,8600));
            }
        }


        [Fact]
        public void UnixTimestamp_BY_datetime()
        {
            DateTime dt = new DateTime(2020, 3, 2, 16, 16, 17, DateTimeKind.Local);
            long expected = 1583136977L;
            long actual = dt.UnixTimestamp();
            Assert.Equal(expected, actual);

            DateTime utcDt = dt.ToUniversalTime();
            actual = utcDt.UnixTimestamp();
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void UnixTimestamp_BY_datetime_offsetSeconds()
        {
            const long offsetSeconds = 825L;
            DateTime dt = new DateTime(2020, 3, 2, 16, 16, 17, DateTimeKind.Local);
            long expected = 1583136977L + offsetSeconds;
            long actual = dt.UnixTimestamp(offsetSeconds);
            Assert.Equal(expected, actual);

            DateTime utcDt = dt.ToUniversalTime();
            actual = dt.UnixTimestamp(offsetSeconds);
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void UnixTimestamp_BY_datetime_offsetTime()
        {
            const long offsetSeconds = 825L;
            TimeSpan offsetTime = TimeSpan.FromSeconds(offsetSeconds);
            DateTime dt = new DateTime(2020, 3, 2, 16, 16, 17, DateTimeKind.Local);
            long expected = 1583136977L + offsetSeconds;
            long actual = dt.UnixTimestamp(offsetTime);
            Assert.Equal(expected, actual);

            DateTime utcDt = dt.ToUniversalTime();
            actual = dt.UnixTimestamp(offsetTime);
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData("2020-01-25", 1)]
        [InlineData("2020-02-25", 1)]
        [InlineData("2020-03-25", 1)]
        [InlineData("2020-04-25", 2)]
        [InlineData("2020-05-25", 2)]
        [InlineData("2020-06-25", 2)]
        [InlineData("2020-07-25", 3)]
        [InlineData("2020-08-25", 3)]
        [InlineData("2020-09-25", 3)]
        [InlineData("2020-10-25", 4)]
        [InlineData("2020-11-25", 4)]
        [InlineData("2020-12-25", 4)]
        public void GetQuarter_BY_date(string strDate, int expected)
        {
            DateTime date = DateTime.Parse(strDate); new DateTime(2020, 1, 25);
            int actual = date.GetQuarter();
            Assert.Equal(expected, actual);
        }
    }
}
