using System;
using System.Diagnostics;
using System.Windows.Input;

namespace VaraniumSharp.Shenfield
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates.
    /// The default return value for the CanExecute method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Constructor

        /// <summary>
        /// Creates a new command that can always execute
        /// </summary>
        /// <param name="execute">The execution logic</param>
        public RelayCommand(Action<object> execute)
            // ReSharper disable once IntroduceOptionalParameters.Global
            : this(execute, null)
        { }

        /// <summary>
        /// Creates a new command
        /// </summary>
        /// <param name="execute">The execution logic</param>
        /// <param name="canExecute">The execution status logic</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region Events

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        [DebuggerStepThrough]
        public bool CanExecute(object parameters)
        {
            return _canExecute == null || _canExecute(parameters);
        }

        /// <inheritdoc />
        public void Execute(object parameters)
        {
            _execute(parameters);
        }

        #endregion

        #region Variables

        private readonly Predicate<object> _canExecute;

        private readonly Action<object> _execute;

        #endregion
    }
}