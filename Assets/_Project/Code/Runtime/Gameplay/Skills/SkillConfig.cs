using DoubleDCore.Configuration.Base;
using Gameplay.Units;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Skills
{
    public abstract class SkillConfig : ScriptableConfig
    {
        [SerializeField] private string _name;
        [SerializeField, PreviewField] private Sprite _icon;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private float _cooldown;
        
        public string Name => _name;
        public Sprite Icon => _icon;
        public string Description => _description;
        public float Cooldown => _cooldown;
        
        public abstract void Accept(ISkillVisitor visitor);
    }
}