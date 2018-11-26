using System.Windows;
using Microsoft.Extensions.Logging;
using VaraniumSharp.Attributes;
using VaraniumSharp.Shenfield.Interfaces.HelperClasses;

namespace VaraniumSharp.Shenfield.HelperClasses
{
    /// <inheritdoc />
    [AutomaticContainerRegistration(typeof(IStyleLoader))]
    public class StyleLoader : IStyleLoader
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public StyleLoader()
        {
            _logger = Logging.StaticLogger.GetLogger<StyleLoader>();
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public Style LoadStyle(string styleName)
        {
            try
            {
                var style = (Style)Application.Current.FindResource(styleName);
                return style;
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                _logger.LogWarning("Resource {StyleName} could not be found", styleName);
                return null;
            }
        }

        #endregion

        #region Variables

        /// <summary>
        /// Logger instance
        /// </summary>
        private readonly ILogger _logger;

        #endregion
    }
}