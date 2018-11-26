using System.Collections.Generic;

namespace VaraniumSharp.Shenfield.Converters
{
    /// <summary>
    /// Allows extracting value from a <see cref="IDictionary{TKey,TValue}"/> that uses an <see cref="string"/> key
    /// </summary>
    public class DictionaryStringValueExtractionConverter : DictionaryValueExtractionConverterBase<string>
    { }
}