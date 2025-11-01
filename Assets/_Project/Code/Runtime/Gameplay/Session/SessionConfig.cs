using DoubleDCore.Configuration.Base;
using UnityEngine;

namespace Gameplay.Session
{
    [CreateAssetMenu(fileName = "Session Config", menuName = "Configs/SessionConfig")]
    public class SessionConfig : ScriptableConfig
    {
        [SerializeField] private LevelConfig[] _levels;
        [SerializeField] private int _sequenceSize = 5;
        
        public LevelConfig[] Levels => _levels;
        public int SequenceSize => _sequenceSize;
    }
}