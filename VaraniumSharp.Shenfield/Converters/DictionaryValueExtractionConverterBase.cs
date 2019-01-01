using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace VaraniumSharp.Shenfield.Converters
{
    /// <summary>
    /// Abstract implementation of <see cref="IMultiValueConverter"/> for use in a <see cref="MultiBinding"/> that allows extracting
    /// values from a Dictionary based on a generic key.
    /// <remarks>
    /// The implementation is abstract as there is no easy way in WPF to use generic types, however it is easy and quick to create a strongly typed
    /// class that inherits the abstract class and use it instead
    /// </remarks>
    /// </summary>
    /// <typeparam name="T">The type value of the dictionary key</typeparam>
    public abstract class DictionaryValueExtractionConverterBase<T> : IMultiValueConverter
    {
        #region Public Methods

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null
                || values.Length != 2
                || values[1] == null)
            {
                return Binding.DoNothing;
            }

            var parsedKey = (T)TypeDescriptor
                .GetConverter(typeof(T))
                .ConvertFromString(values[1].ToString());

            if (!(values[0] is IDictionary dict)
                || parsedKey == null)
            {
                return Binding.DoNothing;
            }

            var dictionaryValue = dict[parsedKey];
            return dictionaryValue ?? Binding.DoNothing;
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}