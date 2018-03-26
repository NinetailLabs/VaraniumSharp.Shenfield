using System.Windows;

namespace VaraniumSharp.Shenfield.Interfaces.HelperClasses
{
    /// <summary>
    /// Assist with loading styles from the <see cref="ResourceDictionary"/>
    /// </summary>
    public interface IStyleLoader
    {
        #region Public Methods

        /// <summary>
        /// Load a Style from the <see cref="ResourceDictionary"/> that are loaded in the application.
        /// </summary>
        /// <param name="styleName">Name of the style to load</param>
        /// <returns>Style unless it could not be found, in which case null</returns>
        Style LoadStyle(string styleName);

        #endregion
    }
}