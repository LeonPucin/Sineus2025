using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Skills
{
    public class AlterKeysListener : MonoBehaviour
    {
        private SkillConfig[] _allSkills;
        private SkillActivator _skillActivator;

        [Zenject.Inject]
        private void Init(SkillsCatalog skillsCatalog, SkillActivator skillActivator)
        {
            _allSkills = skillsCatalog.GetAllItems();
            _skillActivator = skillActivator;
        }
        
        private void Update()
        {
            foreach (var skill in _allSkills)
            {
                if (Keyboard.current[skill.AlterKey].wasPressedThisFrame)
                    _skillActivator.Activate(skill);
            }
        }
    }
}