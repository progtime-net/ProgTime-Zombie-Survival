using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Unity-serializable dictionary wrapper.
    /// - Shows in Inspector (with the drawer below).
    /// - Implements IDictionary<TKey,TValue>.
    /// - Rebuilds internal Dictionary on (de)serialization.
    /// </summary>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> :
        IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        // Unity can serialize these lists for concrete TKey/TValue types.
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        // Not serialized by Unity; rebuilt from lists.
        [NonSerialized] private Dictionary<TKey, TValue> dict;

        public SerializableDictionary()
        {
            dict = new Dictionary<TKey, TValue>();
        }

        #region IDictionary Implementation
        public TValue this[TKey key]
        {
            get => dict[key];
            set => dict[key] = value;
        }

        public ICollection<TKey> Keys => dict.Keys;
        public ICollection<TValue> Values => dict.Values;
        public int Count => dict.Count;
        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value) => dict.Add(key, value);
        public bool ContainsKey(TKey key) => dict.ContainsKey(key);
        public bool Remove(TKey key) => dict.Remove(key);
        public bool TryGetValue(TKey key, out TValue value) => dict.TryGetValue(key, out value);

        public void Add(KeyValuePair<TKey, TValue> item) => dict.Add(item.Key, item.Value);
        public void Clear() => dict.Clear();
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return dict.TryGetValue(item.Key, out var v) &&
                   EqualityComparer<TValue>.Default.Equals(v, item.Value);
        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            => ((ICollection<KeyValuePair<TKey, TValue>>)dict).CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (Contains(item)) return dict.Remove(item.Key);
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dict.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region Helpers (optional)
        public Dictionary<TKey, TValue> ToDictionary() => new Dictionary<TKey, TValue>(dict);
        public void SetDictionary(Dictionary<TKey, TValue> source)
        {
            dict = new Dictionary<TKey, TValue>(source);
            // Keep backing lists in sync for the Inspector
            SyncFromDictToLists();
        }
        #endregion

        #region Serialization
        public void OnBeforeSerialize()
        {
            // 1) Apply any Inspector edits (lists) into runtime dict
            RebuildDictFromLists();
            // 2) Normalize back into lists (keeps pairs aligned and resolves duplicates)
            SyncFromDictToLists();
        }

        public void OnAfterDeserialize()
        {
            // When Unity deserializes, rebuild dict from the lists
            RebuildDictFromLists();
        }

        private void RebuildDictFromLists()
        {
            if (dict == null) dict = new Dictionary<TKey, TValue>();
            else dict.Clear();

            int n = Math.Min(keys.Count, values.Count);
            var seen = new HashSet<TKey>();
            for (int i = 0; i < n; i++)
            {
                var k = keys[i];
                var v = values[i];

                // Last entry wins if there are duplicate keys in the serialized lists
                if (seen.Add(k))
                    dict[k] = v;
                else
                    dict[k] = v;
            }
        }


        private void SyncFromDictToLists()
        {
            keys.Clear();
            values.Clear();
            foreach (var kv in dict)
            {
                keys.Add(kv.Key);
                values.Add(kv.Value);
            }
        }
        #endregion
    }
}
