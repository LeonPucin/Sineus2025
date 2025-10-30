using DoubleDCore.Configuration.Base;
using Game.Utility;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "Sensitivity Config", menuName = "Configs/Sensitivity")]
    public class SensitivityConfig : ScriptableConfig
    {
        [SerializeField] private RangedValue _value;

        public float MinValue => _value.MinValue;
        public float MaxValue => _value.MaxValue;
    }
}