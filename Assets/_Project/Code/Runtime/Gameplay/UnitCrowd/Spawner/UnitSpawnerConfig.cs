using DoubleDCore.Configuration.Base;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.UnitCrowd
{
    [CreateAssetMenu(fileName = "Unit Spawner Config", menuName = "Configs/UnitSpawnerConfig", order = 0)]
    public class UnitSpawnerConfig : ScriptableConfig
    {
        [SerializeField] private Unit[] _unitPrefabs;
        [SerializeField] private UnitState[] _brokenStates;
        
        public UnitState GetRandomBrokenState()
        {
            int randomIndex = Random.Range(0, _brokenStates.Length);
            return _brokenStates[randomIndex];
        }
        
        public Unit GetRandomPrefab()
        {
            int randomIndex = Random.Range(0, _unitPrefabs.Length);
            return _unitPrefabs[randomIndex];
        }
    }
}