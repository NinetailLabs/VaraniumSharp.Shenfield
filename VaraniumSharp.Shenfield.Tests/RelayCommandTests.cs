using System;
using System.Windows.Input;
using System.Windows.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace VaraniumSharp.Shenfield.Tests
{
    public class RelayCommandTests
    {
        #region Public Methods

        [Test]
        public void CanExecuteChangeRegistrationWorks()
        {
            // arrange
            var wasTriggered = false;
            var sut = new RelayCommand(o =>
            {
                //We just do nothing
            });

            EventHandler handler = (s, e) =>
            {
                wasTriggered = true;
            };
            sut.CanExecuteChanged += handler;

            // act
            CommandManager.InvalidateRequerySuggested();
            // Ensure the invalidate is processed
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));

            // assert
            wasTriggered.Should().BeTrue();
        }

        [Test]
        public void SettingUpRelayCommandWithoutActionThrowsException()
        {
            // arrange
            ICommand sut = null;
            var act = new Action(() =>
            {
                sut = new RelayCommand(null);
            });

            // act
            // assert
            act.ShouldThrow<ArgumentNullException>();
            sut.Should().BeNull();
        }

        [Test]
        public void SetupCommandThatCanAlwaysExecute()
        {
            // arrange
            var commandExecuted = false;
            var sut = new RelayCommand(o =>
            {
                commandExecuted = true;
            });

            // act
            sut.Execute(null);

            // assert
            sut.CanExecute(null).Should().BeTrue();
            commandExecuted.Should().BeTrue();
        }

        [Test]
        public void UnregisteringFromCanExecuteChangedWorks()
        {
            // arrange
            var wasTriggered = false;
            var sut = new RelayCommand(o =>
            {
                //We just do nothing
            });

            EventHandler handler = (s, e) =>
            {
                wasTriggered = true;
            };
            sut.CanExecuteChanged += handler;
            sut.CanExecuteChanged -= handler;

            // act
            CommandManager.InvalidateRequerySuggested();
            // Ensure the invalidate is processed
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));

            // assert
            wasTriggered.Should().BeFalse();
        }

        #endregion
    }
}