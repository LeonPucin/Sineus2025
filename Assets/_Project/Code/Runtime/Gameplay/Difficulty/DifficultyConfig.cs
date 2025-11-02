using System;
using DoubleDCore.Configuration.Base;
using UnityEngine;
using UnityEngine.Rendering;

namespace Gameplay.UnitCrowd
{
    [CreateAssetMenu(fileName = "Difficulty Config", menuName = "Configs/DifficultyConfig")]
    public class DifficultyConfig : ScriptableConfig
    {
        [SerializeField] private SerializedDictionary<int, DifficultySettings> _difficultySettings;
        [SerializeField] private int _distributionDifficultyThreshold = 10;
        
        public int DistributionDifficultyThreshold => _distributionDifficultyThreshold;
        
        public DifficultySettings GetSettings(int difficultyPoints)
        {
            return _difficultySettings[difficultyPoints];
        }
    }

    [Serializable]
    public struct DifficultySettings
    {
        public int CrowdSize;
        [Range(0, 1)] public float StartBrokenPart;
        [Range(0, 1)] public float RageAddition;
    }
}