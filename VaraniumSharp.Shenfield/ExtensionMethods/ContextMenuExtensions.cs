using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VaraniumSharp.Shenfield.ExtensionMethods
{
    /// <summary>
    /// Extension methods for <see cref="ContextMenu"/>
    /// </summary>
    public static class ContextMenuExtensions
    {
        #region Public Methods

        /// <summary>
        /// Create a shortcut key for an item in the context menu
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if no modifier key is used and the selected key is not a num pad key</exception>
        /// <param name="menu">The context menu to which the InputBinding should be added</param>
        /// <param name="menuItem">MenuItem to set InputGestureText for</param>
        /// <param name="action">The action that should be executed by the input</param>
        /// <param name="sender">The sender of the event</param>
        /// <param name="routedEventArgs">Routed event arguments for the event</param>
        /// <param name="key">The key that triggers the shortcut</param>
        /// <param name="modifierKeys">ModifierKeys for triggering the shortcut</param>
        public static void AddInputBinding(this ContextMenu menu, MenuItem menuItem, Action<object, RoutedEventArgs> action, object sender, RoutedEventArgs routedEventArgs, Key key, ModifierKeys modifierKeys)
        {
            menu.InputBindings.Add(GenerateInputBinding(menuItem, action, sender, routedEventArgs, key, modifierKeys));
        }

        /// <summary>
        /// Create a shortcut key for an item in the context menu
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if no modifier key is used and the selected key is not a num pad key</exception>
        /// <param name="menu">The menu which contains sub-menuitems to which the InputBinding should be added</param>
        /// <param name="menuItem">MenuItem to set InputGestureText for</param>
        /// <param name="action">The action that should be executed by the input</param>
        /// <param name="sender">The sender of the event</param>
        /// <param name="routedEventArgs">Routed event arguments for the event</param>
        /// <param name="key">The key that triggers the shortcut</param>
        /// <param name="modifierKeys">ModifierKeys for triggering the shortcut</param>
        public static void AddInputBinding(this MenuItem menu, MenuItem menuItem, Action<object, RoutedEventArgs> action, object sender, RoutedEventArgs routedEventArgs, Key key, ModifierKeys modifierKeys)
        {
            menu.InputBindings.Add(GenerateInputBinding(menuItem, action, sender, routedEventArgs, key, modifierKeys));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generate the Gesture stirng for the key combination
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifierKeys"></param>
        /// <returns></returns>
        private static string GenerateGestureString(Key key, ModifierKeys modifierKeys)
        {
            var sb = new StringBuilder();
            foreach (var mKey in modifierKeys.ToString().Split(','))
            {
                switch (mKey.Trim())
                {
                    case nameof(ModifierKeys.Alt):
                        sb.Append("Alt + ");
                        break;

                    case nameof(ModifierKeys.Control):
                        sb.Append("Ctrl + ");
                        break;

                    case nameof(ModifierKeys.Shift):
                        sb.Append("Shift + ");
                        break;

                    case nameof(ModifierKeys.Windows):
                        sb.Append("Win + ");
                        break;

                    default:
                        sb.Append("");
                        break;
                }
            }

            sb.Append(key.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// Create the InputBinding
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="action"></param>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        /// <param name="key"></param>
        /// <param name="modifierKeys"></param>
        /// <returns></returns>
        private static InputBinding GenerateInputBinding(MenuItem menuItem, Action<object, RoutedEventArgs> action, object sender, RoutedEventArgs routedEventArgs, Key key, ModifierKeys modifierKeys)
        {
            if (modifierKeys == ModifierKeys.None && !key.ToString().Contains("Num"))
            {
                throw new ArgumentException("Modifier key cannot be None unless keys from the numpad is used");
            }

            if (menuItem != null)
            {
                menuItem.InputGestureText = GenerateGestureString(key, modifierKeys);
            }
            var keyGesture = new KeyGesture(key, modifierKeys);
            return new InputBinding(new RelayCommand(param => action.Invoke(sender, routedEventArgs)), keyGesture);
        }

        #endregion
    }
}