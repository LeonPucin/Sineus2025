using DoubleDCore.Identification;
using UnityEngine;

namespace Gameplay.Quests
{
    public abstract class QuestConfig : IdentifyingScriptable
    {
        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _description;
        [SerializeField, TextArea] private string _tip;
        
        public string Name => _name;
        public string Description => _description;
        public string Tip => _tip;
        
        protected override string GetIDPrefix()
        {
            return "quest";
        }
        
        public abstract void Accept(IQuestVisitor visitor);
    }
}