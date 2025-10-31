using System.Collections.Generic;
using System.Linq;
using DoubleDCore.Configuration.Base;
using DoubleDCore.Identification;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Infrastructure
{
    public abstract class Catalog<TItem> : ScriptableConfig where TItem : IdentifyingScriptable
    {
        [SerializeField] private TItem[] _items;

        private Dictionary<string, TItem> _itemsById;
        
#if UNITY_EDITOR
        [Button]
        private void UpdateIds()
        {
            if (EditorUtility.DisplayDialog("Change all IDs",
                    "Are you sure you want to regenerate all items' IDs?", "Yes", "No") == false)
            {
                return;
            }
            
            for (int i = 0; i < _items.Length; i++)
                _items[i].RemoveIDForced();
            
            for (int i = 0; i < _items.Length; i++)
                _items[i].ResetIDForced();
        }
#endif
        
        private void FillItemsDictionary()
        {
            _itemsById = new Dictionary<string, TItem>();
            
            foreach (var item in _items)
                _itemsById[item.ID] = item;
        }

        public TItem GetItem(string id)
        {
            if (_itemsById == null)
                FillItemsDictionary();

            return _itemsById[id];
        }

        public TItem[] GetAllItems()
        {
            return _items.ToArray();
        }
    }
}