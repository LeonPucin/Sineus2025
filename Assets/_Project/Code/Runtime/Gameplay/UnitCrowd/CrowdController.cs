using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DoubleDCore.TimeTools;
using Gameplay.Movements;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.UnitCrowd
{
    public class CrowdController
    {
        private readonly UnitSpawner _unitSpawner;
        private readonly DifficultyConverter _difficultyConverter;
        private readonly CrowdMovementsController _movementsController;
        private readonly CrowdConfig _config;
        private readonly CrowdPlaceController _placeController;
        private readonly Unit _mainUnit;
        private readonly Dictionary<Unit, Timer> _unitTimers = new();
        
        public CrowdController(CrowdPlaceController placeController, Unit mainUnit, UnitSpawner unitSpawner,
            DifficultyConverter difficultyConverter, CrowdMovementsController movementsController, CrowdConfig config)
        {
            _placeController = placeController;
            _mainUnit = mainUnit;
            _unitSpawner = unitSpawner;
            _difficultyConverter = difficultyConverter;
            _movementsController = movementsController;
            _config = config;
        }

        public void SetupCrowd(MovementSequence movementSequence, int count)
        {
            float brokenPart =
                _difficultyConverter.ConvertToBrokenPart(movementSequence.CurrentMovements
                    .Select(m => m.DifficultyPoints).Sum());
            
            var spawnedUnits = _unitSpawner.Spawn(count, brokenPart);
            _placeController.DistributeUnits(spawnedUnits);
            
            _unitTimers.Clear();
            foreach (var spawnedUnit in spawnedUnits)
                _unitTimers.Add(spawnedUnit, new Timer(TimeBindingType.ScaledTime));
            
            spawnedUnits.Add(_mainUnit);
            _movementsController.Setup(spawnedUnits, movementSequence);
            
            Start(); //TODO: invoke in another place after user confirmation
        }

        public void Start()
        {
            _movementsController.Start();

            foreach (var (unit, timer) in _unitTimers)
            {
                if (unit.CurrentState != UnitState.Normal)
                {
                    var stateConfig = _config.GetStateConfig(unit.CurrentState);
                    timer.Start(stateConfig.DistributionDelay, () => DistributeUnit(unit));
                }
                
                unit.StateChanged += OnUnitStateChanged;
            }
        }

        public void Stop()
        {
            _movementsController.Stop();
            
            foreach (var (unit, timer) in _unitTimers)
            {
                timer.Stop();
                unit.StateChanged -= OnUnitStateChanged;
            }
        }

        private void DistributeUnit(Unit unit)
        {
            if (unit.CurrentState == UnitState.Normal)
                return;
            
            var stateConfig = _config.GetStateConfig(unit.CurrentState);
            var nearUnits = _placeController.GetUnitsInRadius(unit, stateConfig.DistributionRadius)
                .Where(u => u.CurrentState == UnitState.Normal).ToList();
            var unitsToDistributeCount = Random.Range(stateConfig.DistributionAmount.MinValue,
                stateConfig.DistributionAmount.MaxValue + 1);

            for (int i = 0; i < unitsToDistributeCount && nearUnits.Count > 0; i++)
            {
                var randomIndex = Random.Range(0, nearUnits.Count);
                var selectedUnit = nearUnits[randomIndex];
                nearUnits.RemoveAt(randomIndex);
                
                selectedUnit.SetState(unit.CurrentState);
            }
        }
        
        private void OnUnitStateChanged(Unit unit, UnitState state)
        {
            if (!_unitTimers.ContainsKey(unit))
                return;
            
            var timer = _unitTimers[unit];
            timer.Stop();
            
            if (state == UnitState.Normal)
                return;
            
            var stateConfig = _config.GetStateConfig(state);
            timer.Start(stateConfig.DistributionDelay, () => DistributeUnit(unit));
        }
    }
}