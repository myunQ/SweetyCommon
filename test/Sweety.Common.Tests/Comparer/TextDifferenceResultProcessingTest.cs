using System;

using Xunit;

using Sweety.Common.Comparer;

namespace Sweety.Common.Tests.Comparer
{
    public class TextDifferenceResultProcessingTest
    {
        [Fact]
        public void ExtraStartLabel()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();

            string expected = "abc";
            textDifferenceResult.ExtraStartLabel = expected;
            Assert.Equal(expected, textDifferenceResult.ExtraStartLabel);

            expected = String.Empty;
            textDifferenceResult.ExtraStartLabel = expected;
            Assert.Equal(expected, textDifferenceResult.ExtraStartLabel);

            expected = null;
            textDifferenceResult.ExtraStartLabel = expected;
            Assert.Equal(expected, textDifferenceResult.ExtraStartLabel);
        }

        [Fact]
        public void ExtraEndLabel()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();

            string expected = "abc";
            textDifferenceResult.ExtraEndLabel = expected;
            Assert.Equal(expected, textDifferenceResult.ExtraEndLabel);

            expected = String.Empty;
            textDifferenceResult.ExtraEndLabel = expected;
            Assert.Equal(expected, textDifferenceResult.ExtraEndLabel);

            expected = null;
            textDifferenceResult.ExtraEndLabel = expected;
            Assert.Equal(expected, textDifferenceResult.ExtraEndLabel);
        }

        [Fact]
        public void LackingStartLabel()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();

            string expected = "abc";
            textDifferenceResult.LackingStartLabel = expected;
            Assert.Equal(expected, textDifferenceResult.LackingStartLabel);

            expected = String.Empty;
            textDifferenceResult.LackingStartLabel = expected;
            Assert.Equal(expected, textDifferenceResult.LackingStartLabel);

            expected = null;
            textDifferenceResult.LackingStartLabel = expected;
            Assert.Equal(expected, textDifferenceResult.LackingStartLabel);
        }

        [Fact]
        public void LackingEndLabel()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();

            string expected = "abc";
            textDifferenceResult.LackingEndLabel = expected;
            Assert.Equal(expected, textDifferenceResult.LackingEndLabel);

            expected = String.Empty;
            textDifferenceResult.LackingEndLabel = expected;
            Assert.Equal(expected, textDifferenceResult.LackingEndLabel);

            expected = null;
            textDifferenceResult.LackingEndLabel = expected;
            Assert.Equal(expected, textDifferenceResult.LackingEndLabel);
        }

        [Fact]
        public void DifferentStartLable()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();

            string expected = "abc";
            textDifferenceResult.DifferentStartLable = expected;
            Assert.Equal(expected, textDifferenceResult.DifferentStartLable);

            expected = String.Empty;
            textDifferenceResult.DifferentStartLable = expected;
            Assert.Equal(expected, textDifferenceResult.DifferentStartLable);

            expected = null;
            textDifferenceResult.DifferentStartLable = expected;
            Assert.Equal(expected, textDifferenceResult.DifferentStartLable);
        }

        [Fact]
        public void DifferentEndLable()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();

            string expected = "abc";
            textDifferenceResult.DifferentEndLable = expected;
            Assert.Equal(expected, textDifferenceResult.DifferentEndLable);

            expected = String.Empty;
            textDifferenceResult.DifferentEndLable = expected;
            Assert.Equal(expected, textDifferenceResult.DifferentEndLable);

            expected = null;
            textDifferenceResult.DifferentEndLable = expected;
            Assert.Equal(expected, textDifferenceResult.DifferentEndLable);
        }


        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_1()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            TextDifference[] differences = null;
            string a = "aaaa";
            string b = "abac";

            Assert.False(textDifferenceResult.TryMarkDifferences(differences, a, b, out var _));
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, null, b, out var _));
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, a, null, out var _));
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_2()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            TextDifference[] differences = new TextDifference[0];
            string a = "aaaa";
            string b = "abac";

            Assert.False(textDifferenceResult.TryMarkDifferences(differences, a, b, out var _));
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, null, b, out var _));
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, a, null, out var _));
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_3()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            TextDifference[] differences = new TextDifference[]
            {
                new TextDifference(1, 1, 1, 1),
                new TextDifference(3, 1, 3, 1)
            };
            string a = "aaaa";
            string b = "abac";


            string expected = $"a{textDifferenceResult.DifferentStartLable}a{textDifferenceResult.DifferentEndLable}a{textDifferenceResult.DifferentStartLable}a{textDifferenceResult.DifferentEndLable}";
            Assert.True(textDifferenceResult.TryMarkDifferences(differences, a, b, out var actual));
            Assert.Equal(expected, actual);
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, null, b, out var _));
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, a, null, out var _));
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_4()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            TextDifference[] differences = new TextDifference[]
            {
                new TextDifference(1, 1, 1, 1)
            };
            string a = "aaaa";
            string b = "abac";


            Assert.False(textDifferenceResult.TryMarkDifferences(differences, null, b, out var _));
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, a, null, out var _));
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_5()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            string a = "aaaa";
            string b = "abac";
            TextDifference[] differences = new TextDifference[]
            {
                new TextDifference(0, a.Length, 0, b.Length)
            };

            Assert.True(textDifferenceResult.TryMarkDifferences(differences, null, b, out var actual));
            string expected = textDifferenceResult.LackingStartLabel + b + textDifferenceResult.LackingEndLabel;
            Assert.Equal(expected, actual);

            expected = textDifferenceResult.ExtraStartLabel + a + textDifferenceResult.ExtraEndLabel;
            Assert.True(textDifferenceResult.TryMarkDifferences(differences, a, null, out actual));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_6()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            string a = "aaabbbeeeddd";
            string b = "aaacccbbbddd";
            var differences = new TextDifference[]
            {
                new TextDifference(3, 0, 3, 3),
                new TextDifference(6, 3, 9, 0)
            };
            string expected = $"aaa{textDifferenceResult.LackingStartLabel}ccc{textDifferenceResult.LackingEndLabel}bbb{textDifferenceResult.ExtraStartLabel}eee{textDifferenceResult.ExtraEndLabel}ddd";
            Assert.True(textDifferenceResult.TryMarkDifferences(differences, a, b, out var actual));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_7()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();

            string a = "aaabbbeeeddd";
            string b = "aaacccbbbd";
            var differences = new TextDifference[]
            {
                new TextDifference(3, 0, 3, 3),
                new TextDifference(6, 3, 9, 0),
                new TextDifference(10, 2, 10, 0)
            };
            string expected = $"aaa{textDifferenceResult.LackingStartLabel}ccc{textDifferenceResult.LackingEndLabel}bbb{textDifferenceResult.ExtraStartLabel}eee{textDifferenceResult.ExtraEndLabel}d{textDifferenceResult.ExtraStartLabel}dd{textDifferenceResult.ExtraEndLabel}";
            Assert.True(textDifferenceResult.TryMarkDifferences(differences, a, b, out var actual));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_bDiff_1()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            TextDifference[] differences = null;
            string a = "aaaa";
            string b = "abac";

            Assert.False(textDifferenceResult.TryMarkDifferences(differences, a, b, out var _));
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, null, b, out var _));
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, a, null, out var _));
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_bDiff_2()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            TextDifference[] differences = new TextDifference[0];
            string a = "aaaa";
            string b = "abac";

            Assert.False(textDifferenceResult.TryMarkDifferences(differences, a, b, out var _));
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, null, b, out var _));
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, a, null, out var _));
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_bDiff_3()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            string a = "aaaa";
            string b = "abac";

            var differences = new TextDifference[]
            {
                new TextDifference(1, 1, 1, 1),
                new TextDifference(3, 1, 3, 1)
            };

            string[] expected = {
                $"a{textDifferenceResult.DifferentStartLable}a{textDifferenceResult.DifferentEndLable}a{textDifferenceResult.DifferentStartLable}a{textDifferenceResult.DifferentEndLable}",
                $"a{textDifferenceResult.DifferentStartLable}b{textDifferenceResult.DifferentEndLable}a{textDifferenceResult.DifferentStartLable}c{textDifferenceResult.DifferentEndLable}"
            };

            string[] actual = new string[2];
            ref string actualA = ref actual[0];
            ref string actualB = ref actual[1];

            Assert.True(textDifferenceResult.TryMarkDifferences(differences, a, b, out actualA, out actualB));
            Assert.Equal(expected, actual);
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, null, b, out var _));
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, a, null, out var _));
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_bDiff_4()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            string a = "aaaa";
            string b = "abac";

            var differences = new TextDifference[]
            {
                new TextDifference(1, 1, 1, 1)
            };

            string[] expected = {
                $"a{textDifferenceResult.DifferentStartLable}a{textDifferenceResult.DifferentEndLable}a{textDifferenceResult.DifferentStartLable}a{textDifferenceResult.DifferentEndLable}",
                $"a{textDifferenceResult.DifferentStartLable}b{textDifferenceResult.DifferentEndLable}a{textDifferenceResult.DifferentStartLable}c{textDifferenceResult.DifferentEndLable}"
            };

            string[] actual = new string[2];
            ref string actualA = ref actual[0];
            ref string actualB = ref actual[1];

            Assert.False(textDifferenceResult.TryMarkDifferences(differences, null, b, out var _));
            Assert.False(textDifferenceResult.TryMarkDifferences(differences, a, null, out var _));
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_bDiff_5()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            string a = "aaaa";
            string b = "abac";

            var differences = new TextDifference[]
            {
                new TextDifference(0, a.Length, 0, b.Length)
            };

            string[] expected = {
                textDifferenceResult.LackingStartLabel + b + textDifferenceResult.LackingEndLabel,
                textDifferenceResult.ExtraStartLabel + b + textDifferenceResult.ExtraEndLabel
            };

            string[] actual = new string[2];
            ref string actualA = ref actual[0];
            ref string actualB = ref actual[1];


            Assert.True(textDifferenceResult.TryMarkDifferences(differences, null, b, out actualA, out actualB));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_bDiff_6()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            string a = "aaaa";
            string b = "abac";

            var differences = new TextDifference[]
            {
                new TextDifference(0, a.Length, 0, b.Length)
            };

            string[] expected = {
                textDifferenceResult.ExtraStartLabel + a + textDifferenceResult.ExtraEndLabel,
                textDifferenceResult.LackingStartLabel + a + textDifferenceResult.LackingEndLabel
            };

            string[] actual = new string[2];
            ref string actualA = ref actual[0];
            ref string actualB = ref actual[1];
            Assert.True(textDifferenceResult.TryMarkDifferences(differences, a, null, out actualA, out actualB));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_bDiff_7()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            string[] expected = {
                $"aaa{textDifferenceResult.LackingStartLabel}ccc{textDifferenceResult.LackingEndLabel}bbb{textDifferenceResult.ExtraStartLabel}eee{textDifferenceResult.ExtraEndLabel}ddd",
                $"aaa{textDifferenceResult.ExtraStartLabel}ccc{textDifferenceResult.ExtraEndLabel}bbb{textDifferenceResult.LackingStartLabel}eee{textDifferenceResult.LackingEndLabel}ddd"
            };

            string[] actual = new string[2];
            ref string actualA = ref actual[0];
            ref string actualB = ref actual[1];

            string a = "aaabbbeeeddd";
            string b = "aaacccbbbddd";
            var differences = new TextDifference[]
            {
                new TextDifference(3, 0, 3, 3),
                new TextDifference(6, 3, 9, 0)
            };

            Assert.True(textDifferenceResult.TryMarkDifferences(differences, a, b, out actualA, out actualB));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TryMarkDifferences_BY_differences_a_b_aDiff_bDiff_8()
        {
            TextDifferenceResultProcessing textDifferenceResult = new TextDifferenceResultProcessing();
            string[] expected = {
                $"aaa{textDifferenceResult.LackingStartLabel}ccc{textDifferenceResult.LackingEndLabel}bbb{textDifferenceResult.ExtraStartLabel}eee{textDifferenceResult.ExtraEndLabel}d{textDifferenceResult.ExtraStartLabel}dd{textDifferenceResult.ExtraEndLabel}",
                $"aaa{textDifferenceResult.ExtraStartLabel}ccc{textDifferenceResult.ExtraEndLabel}bbb{textDifferenceResult.LackingStartLabel}eee{textDifferenceResult.LackingEndLabel}d{textDifferenceResult.LackingStartLabel}dd{textDifferenceResult.LackingEndLabel}"
            };

            string[] actual = new string[2];
            ref string actualA = ref actual[0];
            ref string actualB = ref actual[1];


            string a = "aaabbbeeeddd";
            string b = "aaacccbbbd";
            var differences = new TextDifference[]
            {
                new TextDifference(3, 0, 3, 3),
                new TextDifference(6, 3, 9, 0),
                new TextDifference(10, 2, 10, 0)
            };

            Assert.True(textDifferenceResult.TryMarkDifferences(differences, a, b, out actualA, out actualB));
            Assert.Equal(expected, actual);
        }
    }
}
