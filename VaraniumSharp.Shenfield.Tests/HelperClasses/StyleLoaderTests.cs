using FluentAssertions;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using VaraniumSharp.Shenfield.HelperClasses;

namespace VaraniumSharp.Shenfield.Tests.HelperClasses
{
    public class StyleLoaderTests
    {
        #region Public Methods

        [Test]
        public void LoadingResourceThatDoesNotExistReturnsNull()
        {
            // arrange
            const string styleName = "DoesNotExist";
            var fixture = new StyleLoaderFixture();
            var app = StyleLoaderFixture.App;
            app.Resources.MergedDictionaries.Clear();
            var sut = fixture.GetInstance();

            // act
            var result = sut.LoadStyle(styleName);

            // assert
            result.Should().BeNull();
        }

        [Test]
        public void LoadingResourceThatExistsCorrectlyLoadsTheResource()
        {
            // arrange
            const string styleName = "Exists";
            var fixture = new StyleLoaderFixture();
            var app = StyleLoaderFixture.App;
            var dict = new ResourceDictionary
            {
                {styleName, new Style()}
            };

            app.Resources.MergedDictionaries.Add(dict);
            var sut = fixture.GetInstance();

            // act
            var result = sut.LoadStyle(styleName);

            // assert
            result.Should().NotBeNull();
        }

        #endregion

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Local", Justification = "Test Fixture - Unit tests require access to Mocks")]
        private class StyleLoaderFixture
        {
            #region Constructor

            public StyleLoaderFixture()
            {
                if (App == null)
                {
                    App = new Application();
                }
            }

            #endregion

            #region Properties

            public static Application App { get; private set; }

            #endregion

            #region Public Methods

            public StyleLoader GetInstance()
            {
                return new StyleLoader();
            }

            #endregion
        }
    }
}