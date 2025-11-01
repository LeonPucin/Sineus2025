using Game.Utility;
using UnityEngine;

namespace Gameplay.Quests
{
    [CreateAssetMenu(fileName = "Difficulty Quest Config", menuName = "Configs/Quests/DifficultyQuestConfig")]
    public class DifficultyQuestConfig : QuestConfig
    {
        [SerializeField] private RangedValueInt _targetDifficultyPoints;

        public RangedValueInt TargetDifficultyPoints => _targetDifficultyPoints;
        
        public override void Accept(IQuestVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}