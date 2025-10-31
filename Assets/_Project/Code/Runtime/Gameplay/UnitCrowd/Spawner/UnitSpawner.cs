using System;
using System.Collections.Generic;
using Gameplay.Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.UnitCrowd
{
    public class UnitSpawner
    {
        private readonly UnitSpawnerConfig _config;
        private readonly SpawnPlaceholder _placeholder;
        private readonly UnitConfig[] _allUnitConfigs;

        public event Action<Unit> Spawned; 

        public UnitSpawner(UnitSpawnerConfig config, SpawnPlaceholder placeholder)
        {
            _config = config;
            _placeholder = placeholder;
        }
        
        public List<Unit> Spawn(int count, float brokenPart)
        {
            int brokenCount = Mathf.RoundToInt(brokenPart * count);
            List<Unit> spawnedUnits = new(count);

            for (int i = 0; i < count; i++)
            {
                var state = i < brokenCount ? _config.GetRandomBrokenState() : UnitState.Normal;
                var prefab = _config.GetRandomPrefab();
                Unit unit = GameObject.Instantiate(prefab, _placeholder.transform);
                unit.SetState(state);
                
                spawnedUnits.Add(unit);
            }

            return spawnedUnits;
        }
    }
}