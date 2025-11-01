using DoubleDCore.Configuration.Base;
using Gameplay.Movements;
using UnityEngine;

namespace Gameplay.UnitCrowd
{
    [CreateAssetMenu(fileName = "Crowd Movements Config", menuName = "Configs/CrowdMovementsConfig", order = 0)]
    public class CrowdMovementsConfig : ScriptableConfig
    {
        //[SerializeField] private float _crowdLength;
        [SerializeField] private MovementConfig _idleMovementConfig;
        [SerializeField] private MovementConfig _heresyMovementConfig;
        [SerializeField] private MovementConfig[] _possibleMovements;

        //public float CrowdLength => _crowdLength;
        public MovementConfig IdleMovementConfig => _idleMovementConfig;
        public MovementConfig HeresyMovementConfig => _heresyMovementConfig;
        public MovementConfig[] PossibleMovements => _possibleMovements;
    }
}