using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using FluentAssertions;
using NUnit.Framework;
using VaraniumSharp.Shenfield.Converters;

namespace VaraniumSharp.Shenfield.Tests.Converters
{
    public class DictionaryValueExtractionConverterTests
    {
        #region Public Methods

        [Test]
        public void ConvertBackThrowANotImplementedException()
        {
            // arrange
            var sut = new DictionaryIntValueExtractionConverter();
            var act = new Action(() => sut.ConvertBack(new object[0], new Type[0], new object(), CultureInfo.InvariantCulture));

            // act
            // assert
            act.ShouldThrow<NotImplementedException>();
        }

        [Test]
        public void IfCollectionDoesNotImplementIDictionaryTheBindingDoesNothing()
        {
            // arrange
            var sut = new DictionaryIntValueExtractionConverter();

            // act
            var result = sut.Convert(new object[] { new List<string>(), 1 }, typeof(int), new object(), CultureInfo.InvariantCulture);

            // assert
            result.Should().Be(Binding.DoNothing);
        }

        [Test]
        public void IfKeyCouldNotBeFoundInTheDictionaryTheBindingDoesNothing()
        {
            // arrange
            const int expectedKey = 2;
            const string expectedValue = "This one";
            var sut = new DictionaryIntValueExtractionConverter();
            var dictionary = new Dictionary<int, string>
            {
                { 1, "Not this one" },
                { expectedKey, expectedValue }
            };

            // act
            var result = sut.Convert(new object[] { dictionary, 12 }, typeof(int), string.Empty, CultureInfo.InvariantCulture);

            // assert
            result.Should().Be(Binding.DoNothing);
        }

        [Test]
        public void IfKeyIsNullTheBindingShouldDoNothing()
        {
            // arrange
            const int expectedKey = 2;
            const string expectedValue = "This one";
            var sut = new DictionaryIntValueExtractionConverter();
            var dictionary = new Dictionary<int, string>
            {
                { 1, "Not this one" },
                { expectedKey, expectedValue }
            };

            // act
            var result = sut.Convert(new object[] { dictionary, null }, typeof(int), string.Empty, CultureInfo.InvariantCulture);

            // assert
            result.Should().Be(Binding.DoNothing);
        }

        [Test]
        public void IfThereAreNotEnoughParametersTheBindingDoesNothing()
        {
            // arrange
            var sut = new DictionaryIntValueExtractionConverter();

            // act
            var result = sut.Convert(new object[] { new Dictionary<int, string>() }, typeof(int), new object(), CultureInfo.InvariantCulture);

            // assert
            result.Should().Be(Binding.DoNothing);
        }

        [Test]
        public void ValueIsCorrectlyExtractedFromIntKeyedDictionary()
        {
            // arrange
            const int expectedKey = 2;
            const string expectedValue = "This one";
            var sut = new DictionaryIntValueExtractionConverter();
            var dictionary = new Dictionary<int, string>
            {
                { 1, "Not this one" },
                { expectedKey, expectedValue }
            };

            // act
            var result = sut.Convert(new object[] { dictionary, expectedKey }, typeof(int), string.Empty, CultureInfo.InvariantCulture);

            // assert
            result.Should().Be(expectedValue);
        }

        [Test]
        public void ValueIsCorrectlyExtractedFromStringKeyedDictionary()
        {
            // arrange
            const string expectedKey = "Two";
            const string expectedValue = "This one";
            var sut = new DictionaryStringValueExtractionConverter();
            var dictionary = new Dictionary<string, string>
            {
                { "One", "Not this one" },
                { expectedKey, expectedValue }
            };

            // act
            var result = sut.Convert(new object[] { dictionary, expectedKey }, typeof(string), string.Empty, CultureInfo.InvariantCulture);

            // assert
            result.Should().Be(expectedValue);
        }

        #endregion
    }
}