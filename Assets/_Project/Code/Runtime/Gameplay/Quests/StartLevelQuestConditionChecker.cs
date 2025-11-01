using System.Linq;
using Gameplay.Session;

namespace Gameplay.Quests
{
    public class StartLevelQuestConditionChecker
    {
        private readonly SessionInfo _sessionInfo;

        public StartLevelQuestConditionChecker(SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;
        }
        
        public bool CanStartLevel()
        {
            var quest = _sessionInfo.CurrentLevel.Quest;
            var checker = new ConditionChecker(_sessionInfo);
            quest.Accept(checker);
            return checker.CanStart;
        }

        private class ConditionChecker : IQuestVisitor
        {
            private readonly SessionInfo _sessionInfo;
            
            public bool CanStart { get; private set; } = false;

            public ConditionChecker(SessionInfo sessionInfo)
            {
                _sessionInfo = sessionInfo;
            }
            
            public void Visit(DifficultyQuestConfig difficultyQuestConfig)
            {
                int totalDifficulty = _sessionInfo.CurrentSequence.ValidSequence
                    .Sum(x => x.DifficultyPoints);
                
                CanStart = totalDifficulty >= difficultyQuestConfig.TargetDifficultyPoints.MinValue && 
                       totalDifficulty <= difficultyQuestConfig.TargetDifficultyPoints.MaxValue;
            }
        }
    }
}