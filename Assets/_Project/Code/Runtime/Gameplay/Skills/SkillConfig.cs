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
        [PropertySpace]
        [SerializeField] private UnitState _targetState;
        
        public string Name => _name;
        public Sprite Icon => _icon;
        public string Description => _description;
        public UnitState TargetState => _targetState;
        
        public abstract void Accept(ISkillVisitor visitor);
    }
}