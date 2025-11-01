using DoubleDCore.Identification;
using UnityEngine;

namespace Gameplay.Session
{
    [CreateAssetMenu(fileName = "Level Config", menuName = "Configs/LevelConfig")]
    public class LevelConfig : IdentifyingScriptable
    {
        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _description;
        
        public string Name => _name;
        public string Description => _description;
        
        protected override string GetIDPrefix()
        {
            return "level";
        }
    }
}