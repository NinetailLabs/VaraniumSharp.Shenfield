using FluentAssertions;
using NUnit.Framework;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VaraniumSharp.Shenfield.ExtensionMethods;

namespace VaraniumSharp.Shenfield.Tests.ExtensionMethods
{
    [Apartment(System.Threading.ApartmentState.STA)]
    public class ContextMenuExtensionsTests
    {
        [TestCase(Key.A, ModifierKeys.Alt, "Alt + A")]
        [TestCase(Key.C, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl + Shift + C")]
        [TestCase(Key.R, ModifierKeys.Windows, "Win + R")]
        [TestCase(Key.NumPad0, ModifierKeys.None, "NumPad0")]
        public void InputGestureTextIsGeneratedCorrectly(Key key, ModifierKeys modifierKeys, string expectedGestureText)
        {
            // arrange
            var menuDummy = new ContextMenu();
            var menuItemDummy = new MenuItem();

            // act
            menuDummy.AddInputBinding(menuItemDummy, ActionDummy, this, null, key, modifierKeys);

            // assert
            menuItemDummy.InputGestureText.Should().Be(expectedGestureText);
        }

        [Test]
        public void IfMenuItemIsNullAnExceptionIsNotThrown()
        {
            // arrange
            var menuDummy = new ContextMenu();
            var act = new Action(() => menuDummy.AddInputBinding(null, ActionDummy, this, null, Key.A, ModifierKeys.Control));

            // act
            // assert
            act.ShouldNotThrow<Exception>();
        }

        [Test]
        public void IfNonNumPadKeyIsUsedAnExceptionIsThrown()
        {
            // arrange
            var menuDummy = new ContextMenu();
            var menuItemDummy = new MenuItem();
            var act = new Action(() => menuDummy.AddInputBinding(menuItemDummy, ActionDummy, this, null, Key.A, ModifierKeys.None));

            // act
            // assert
            act.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void InputBindingIsCorrectlyAttachedToTheMenu()
        {
            // arrange
            var menuDummy = new ContextMenu();
            var menuItemDummy = new MenuItem();

            // act
            menuDummy.AddInputBinding(menuItemDummy, ActionDummy, this, null, Key.A, ModifierKeys.Control);

            // assert
            menuDummy.InputBindings.Count.Should().Be(1);
        }

        [Test]
        public void CorrectCommandIsAttached()
        {
            // arrange
            var menuDummy = new ContextMenu();
            var menuItemDummy = new MenuItem();
            var wasRaised = false;
            var act = new Action<object, RoutedEventArgs>((s, a) =>
            {
                wasRaised = true;
            });

            menuDummy.AddInputBinding(menuItemDummy, act, this, null, Key.A, ModifierKeys.Control);
            var cmd = menuDummy.InputBindings[0].Command;

            // act
            cmd.Execute(null);

            // assert
            wasRaised.Should().BeTrue();
        }

        [Test]
        public void CorrectGestureIsAssignedToTheKey()
        {
            // arrange
            var menuDummy = new ContextMenu();
            var menuItemDummy = new MenuItem();

            // act
            menuDummy.AddInputBinding(menuItemDummy, ActionDummy, this, null, Key.A, ModifierKeys.Control);

            // assert
            (menuDummy.InputBindings[0].Gesture as KeyGesture).ShouldBeEquivalentTo(new KeyGesture(Key.A, ModifierKeys.Control));
        }

        private void ActionDummy(object s, RoutedEventArgs args)
        {
            // This does nothing
        }
    }
}