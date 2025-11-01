using DoubleDCore.Identification;
using Gameplay.Quests;
using UnityEngine;

namespace Gameplay.Session
{
    [CreateAssetMenu(fileName = "Level Config", menuName = "Configs/LevelConfig")]
    public class LevelConfig : IdentifyingScriptable
    {
        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private QuestConfig _quest;
        [SerializeField] private float _timeLimit;
        
        public string Name => _name;
        public string Description => _description;
        public QuestConfig Quest => _quest;
        public float TimeLimit => _timeLimit;
        
        protected override string GetIDPrefix()
        {
            return "level";
        }
    }
}