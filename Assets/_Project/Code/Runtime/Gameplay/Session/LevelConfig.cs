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
        [SerializeField, Range(0, 1)] private float _winThreshold = 0.8f;
        
        public string Name => _name;
        public string Description => _description;
        public QuestConfig Quest => _quest;
        public float TimeLimit => _timeLimit;
        public float WinThreshold => _winThreshold;
        
        protected override string GetIDPrefix()
        {
            return "level";
        }
    }
}