using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace VaraniumSharp.Shenfield.Collections
{
    /// <summary>
    /// Wrapper around a Dictionary to make it observable for WPF consumption
    /// </summary>
    /// <typeparam name="TKey">Type of the dictionary key</typeparam>
    /// <typeparam name="TValue">Type of values stored in the dictionary</typeparam>
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged
    {
        #region Constructor

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public ObservableDictionary()
        {
            _baseDictionary = new Dictionary<TKey, TValue>();
        }

        #endregion

        #region Events

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Properties

        /// <inheritdoc />
        public int Count => _baseDictionary.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public TValue this[TKey key]
        {
            get => _baseDictionary[key];
            set
            {
                _baseDictionary.TryGetValue(key, out var oldValue);
                _baseDictionary[key] = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                    new List<KeyValuePair<TKey, TValue>> { new KeyValuePair<TKey, TValue>(key, value) },
                    new List<KeyValuePair<TKey, TValue>> { new KeyValuePair<TKey, TValue>(key, oldValue) }));
            }
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys => _baseDictionary.Keys;

        /// <inheritdoc />
        public ICollection<TValue> Values => _baseDictionary.Values;

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _baseDictionary.Add(item.Key, item.Value);
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                    new List<KeyValuePair<TKey, TValue>> { item }));
        }

        /// <inheritdoc />
        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <inheritdoc />
        public void Clear()
        {
            _baseDictionary.Clear();
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _baseDictionary.Contains(item);
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key)
        {
            return _baseDictionary.ContainsKey(key);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var currentIndex = arrayIndex;
            foreach (var entry in _baseDictionary)
            {
                array[currentIndex] = entry;
                currentIndex++;
            }
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _baseDictionary.GetEnumerator();
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            _baseDictionary.TryGetValue(key, out var removedValue);
            var result = _baseDictionary.Remove(key);
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                    new List<KeyValuePair<TKey, TValue>> { new KeyValuePair<TKey, TValue>(key, removedValue) }));
            return result;
        }

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _baseDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Update the dictionary from a different dictionary.
        /// This method will remove entries not in the source dictionary, add entries not in the target dictionary and update changed entries
        /// </summary>
        /// <param name="dictionaryToUpdateFrom">Dictionary to mirror</param>
        public void UpdateDictionary(IDictionary<TKey, TValue> dictionaryToUpdateFrom)
        {
            var newEntries = dictionaryToUpdateFrom.Where(x => !_baseDictionary.ContainsKey(x.Key)).ToList();
            var removedEntries = _baseDictionary.Where(x => !dictionaryToUpdateFrom.ContainsKey(x.Key)).ToList();
            var updatedEntries = dictionaryToUpdateFrom.Where(x => _baseDictionary.ContainsKey(x.Key)).ToList();

            foreach (var oldEntry in removedEntries)
            {
                Remove(oldEntry.Key);
            }

            foreach (var updatedEntry in updatedEntries)
            {
                this[updatedEntry.Key] = updatedEntry.Value;
            }

            foreach (var newEntry in newEntries)
            {
                Add(newEntry);
            }
        }

        #endregion

        #region Private Methods

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _baseDictionary.GetEnumerator();
        }

        #endregion

        #region Variables

        /// <summary>
        ///     Base dictionary that is being wrapped
        /// </summary>
        private readonly Dictionary<TKey, TValue> _baseDictionary;

        #endregion
    }
}