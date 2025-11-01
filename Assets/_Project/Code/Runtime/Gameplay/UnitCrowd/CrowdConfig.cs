using System;
using System.Collections.Generic;
using DoubleDCore.Configuration.Base;
using Game.Utility;
using Gameplay.Units;
using Sirenix.Serialization;
using UnityEngine;

namespace Gameplay.UnitCrowd
{
    [CreateAssetMenu(fileName = "Crowd Config", menuName = "Configs/CrowdConfig", order = 0)]
    public class CrowdConfig : ScriptableConfig
    {
        [SerializeField] private float _distributionDelay;
        [SerializeField] private RangedValueInt _distributionAmount;
        [OdinSerialize] private Dictionary<UnitState, StateConfig> _stateConfigs = new();
        
        public float DistributionDelay => _distributionDelay;
        public RangedValueInt DistributionAmount => _distributionAmount;
        
        public StateConfig GetStateConfig(UnitState state)
        {
            return _stateConfigs[state];
        }
    }

    [Serializable]
    public struct StateConfig
    {
        public float DistributionDelay;
        public RangedValueInt DistributionAmount;
        public float DistributionRadius;
    }
}