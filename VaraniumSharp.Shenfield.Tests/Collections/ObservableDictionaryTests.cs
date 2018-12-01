using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using VaraniumSharp.Shenfield.Collections;

namespace VaraniumSharp.Shenfield.Tests.Collections
{
    public class ObservableDictionaryTests
    {
        #region Public Methods

        [Test]
        public void AddingAnItemToTheDictionaryRaisesTheCollectionChangedEvent()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";
            var wasRaised = false;
            var raisedAction = NotifyCollectionChangedAction.Reset;
            var raisedEntry = new KeyValuePair<int, string>();

            var sut = new ObservableDictionary<int, string>();
            sut.CollectionChanged += (sender, args) =>
            {
                wasRaised = true;
                raisedAction = args.Action;
                raisedEntry = (KeyValuePair<int, string>)args.NewItems[0];
            };

            // act
            sut.Add(keyToAdd, valueToAdd);

            // assert
            wasRaised.Should().BeTrue();
            raisedAction.Should().Be(NotifyCollectionChangedAction.Add);
            raisedEntry.Key.Should().Be(keyToAdd);
            raisedEntry.Value.Should().Be(valueToAdd);
        }

        [TestCase(1, false)]
        [TestCase(0, true)]
        public void CheckingIfDictionaryContainsAKeyReturnsTheCorrectResult(int keyToCheck, bool expectedResult)
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";

            var sut = new ObservableDictionary<int, string> { { keyToAdd, valueToAdd } };

            // act
            var result = sut.ContainsKey(keyToAdd);

            // assert
            result.Should().Be(result);
        }

        [TestCase(1, "", false)]
        [TestCase(0, "Test", true)]
        [TestCase(0, "Meh", false)]
        public void CheckingIfDictionaryContainsAnItemReturnsTheCorrectResult(int keyToCheck, string valueToCheck, bool expectedResult)
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";

            var sut = new ObservableDictionary<int, string> { { keyToAdd, valueToAdd } };

            // act
            var result = sut.Contains(new KeyValuePair<int, string>(keyToCheck, valueToCheck));

            // assert
            result.Should().Be(result);
        }

        [Test]
        public void ClearingTheDictionaryRaisesTheCollectionChangedEvent()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";
            var wasRaised = false;
            var raisedAction = NotifyCollectionChangedAction.Add;

            var sut = new ObservableDictionary<int, string>();
            sut.CollectionChanged += (sender, args) =>
            {
                if (args.Action != NotifyCollectionChangedAction.Reset)
                {
                    return;
                }
                wasRaised = true;
                raisedAction = args.Action;
            };

            sut.Add(keyToAdd, valueToAdd);

            // act
            sut.Clear();

            // assert
            wasRaised.Should().BeTrue();
            raisedAction.Should().Be(NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public void DictionaryDoesNotSupportReadOnlyMode()
        {
            // arrange
            // ReSharper disable once CollectionNeverUpdated.Local - We do not care about updating the collection
            var sut = new ObservableDictionary<int, string>();

            // act
            // assert
            sut.IsReadOnly.Should().BeFalse();
        }

        [Test]
        public void DictionaryIsCorrectlyCopiedToAnArray()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";
            var targetArray = new KeyValuePair<int, string>[2];

            var sut = new ObservableDictionary<int, string> { { keyToAdd, valueToAdd } };

            // act
            sut.CopyTo(targetArray, 1);

            // assert
            targetArray[1].Key.Should().Be(keyToAdd);
            targetArray[1].Value.Should().Be(valueToAdd);
        }

        [Test]
        public void DictionaryReturnsCorrectKeysAndValues()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";

            var sut = new ObservableDictionary<int, string> { { keyToAdd, valueToAdd } };

            // act
            var keys = sut.Keys;
            var values = sut.Values;

            // assert
            keys.Count.Should().Be(1);
            values.Count.Should().Be(1);
            keys.First().Should().Be(keyToAdd);
            values.First().Should().Be(valueToAdd);
        }

        [Test]
        public void GetEnumeratorCorrectlyReturnsAnEnumeratorForTheDictionary()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";

            var sut = new ObservableDictionary<int, string> { { keyToAdd, valueToAdd } };

            // act
            using (var enumerator = sut.GetEnumerator())
            {
                // assert
                enumerator.Current.Key.Should().Be(keyToAdd);
            }
        }

        [Test]
        public void GettingDictionarySizeReturnsTheCorrectResult()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";

            var sut = new ObservableDictionary<int, string> { { keyToAdd, valueToAdd } };

            // act
            var entries = sut.Count;

            // assert
            entries.Should().Be(1);
        }

        [Test]
        public void GettingEnumeratorByCastingToIEnumerableReturnsTheEnumeratorForTheDictionary()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";

            var sut = new ObservableDictionary<int, string> { { keyToAdd, valueToAdd } };

            // act
            foreach (var entry in (IEnumerable)sut)
            {
                // assert
                var typedEntry = (KeyValuePair<int, string>)entry;
                typedEntry.Key.Should().Be(keyToAdd);
                typedEntry.Value.Should().Be(valueToAdd);
            }
        }

        [Test]
        public void GettingValueByKeyReturnsTheExpectedValue()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";

            var sut = new ObservableDictionary<int, string> { { keyToAdd, valueToAdd } };

            // act
            var result = sut[keyToAdd];

            // assert
            result.Should().Be(valueToAdd);
        }

        [Test]
        public void RemovingAnEntryFromTheDictionaryRaisesTheCollectionChangeEvent()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";
            var wasRaised = false;
            var raisedAction = NotifyCollectionChangedAction.Reset;
            var raisedEntry = new KeyValuePair<int, string>();

            var sut = new ObservableDictionary<int, string>();
            sut.CollectionChanged += (sender, args) =>
            {
                if (args.Action != NotifyCollectionChangedAction.Remove)
                {
                    return;
                }

                wasRaised = true;
                raisedAction = args.Action;
                raisedEntry = (KeyValuePair<int, string>)args.OldItems[0];
            };
            sut.Add(keyToAdd, valueToAdd);

            // act
            sut.Remove(new KeyValuePair<int, string>(keyToAdd, valueToAdd));

            // assert
            wasRaised.Should().BeTrue();
            raisedAction.Should().Be(NotifyCollectionChangedAction.Remove);
            raisedEntry.Key.Should().Be(keyToAdd);
            raisedEntry.Value.Should().Be(valueToAdd);
        }

        [Test]
        public void TryingToGetAValueThatDoesNotExistReturnsNothing()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";

            var sut = new ObservableDictionary<int, string> { { keyToAdd, valueToAdd } };

            // act
            sut.TryGetValue(1, out var result);

            // assert
            result.Should().BeNullOrEmpty();
        }

        [Test]
        public void UpdatingDictionaryValueRaisesTheCollectionChangedEvent()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";
            const string updatedValue = "New Value";
            var wasRaised = false;
            var raisedAction = NotifyCollectionChangedAction.Reset;
            var raisedEntry = new KeyValuePair<int, string>();

            var sut = new ObservableDictionary<int, string>();
            sut.CollectionChanged += (sender, args) =>
            {
                if (args.Action != NotifyCollectionChangedAction.Replace)
                {
                    return;
                }

                wasRaised = true;
                raisedAction = args.Action;
                raisedEntry = (KeyValuePair<int, string>)args.NewItems[0];
            };
            sut.Add(keyToAdd, valueToAdd);

            // act
            sut[keyToAdd] = updatedValue;

            // assert
            wasRaised.Should().BeTrue();
            raisedAction.Should().Be(NotifyCollectionChangedAction.Replace);
            raisedEntry.Key.Should().Be(keyToAdd);
            raisedEntry.Value.Should().Be(updatedValue);
        }

        [Test]
        public void UpdatingTheDictionaryCorrectlyAddsNewEntries()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";
            const int newKey = 1;
            const string newValue = "A new value";

            var updateDictionary = new Dictionary<int, string> { { newKey, newValue } };

            var sut = new ObservableDictionary<int, string> { { keyToAdd, valueToAdd } };

            // act
            sut.UpdateDictionary(updateDictionary);

            // assert
            sut.ContainsKey(newKey).Should().BeTrue();
            sut[newKey].Should().Be(newValue);
        }

        [Test]
        public void UpdatingTheDictionaryRemovedNonExistentEntries()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";
            const int newKey = 1;
            const string newValue = "A new value";

            var updateDictionary = new Dictionary<int, string> { { newKey, newValue } };

            var sut = new ObservableDictionary<int, string> { { keyToAdd, valueToAdd } };

            // act
            sut.UpdateDictionary(updateDictionary);

            // assert
            sut.ContainsKey(keyToAdd).Should().BeFalse();
        }

        [Test]
        public void UpdatingTheDictionaryUpdatesExistingEntries()
        {
            // arrange
            const int keyToAdd = 0;
            const string valueToAdd = "Test";
            const int newKey = 1;
            const string newValue = "A new value";
            const string changedValue = "A changed value";

            var updateDictionary = new Dictionary<int, string> { { newKey, newValue }, { keyToAdd, changedValue } };

            var sut = new ObservableDictionary<int, string> { { keyToAdd, valueToAdd } };

            // act
            sut.UpdateDictionary(updateDictionary);

            // assert
            sut.ContainsKey(keyToAdd).Should().BeTrue();
            sut[keyToAdd].Should().Be(changedValue);
        }

        #endregion
    }
}