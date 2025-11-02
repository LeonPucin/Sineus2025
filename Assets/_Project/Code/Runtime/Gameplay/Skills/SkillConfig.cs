using DoubleDCore.Configuration.Base;
using DoubleDCore.Identification;
using Gameplay.Units;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Skills
{
    public abstract class SkillConfig : IdentifyingScriptable
    {
        [SerializeField] private string _name;
        [SerializeField, PreviewField] private Sprite _icon;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private float _cooldown;
        [SerializeField] private Key _alterKey;
        [SerializeField] private Color _skillColor;
        
        public string Name => _name;
        public Sprite Icon => _icon;
        public string Description => _description;
        public float Cooldown => _cooldown;
        public Key AlterKey => _alterKey;
        public Color SkillColor => _skillColor;
        
        public abstract void Accept(ISkillVisitor visitor);
        
        protected override string GetIDPrefix()
        {
            return "skill";
        }
    }
}