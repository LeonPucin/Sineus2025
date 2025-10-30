using DoubleDCore.Identification;
using UnityEngine;

namespace DoubleDCore.Economy.Base
{
    public abstract class Item : IdentifyingScriptable
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _sprite;

        public string Name => _name;
        public string Description => _description;
        public Sprite Sprite => _sprite;

        protected abstract override string GetIDPrefix();
    }
}