using System.Collections.Generic;
using System.Linq;
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
        private readonly UnitSpawnerConfig _spawnerConfig;
        private readonly CrowdPlaceController _placeController;
        private readonly Unit _mainUnit;
        private readonly Dictionary<Unit, MovementTimer> _unitTimers = new();
        private readonly List<Unit> _currentUnits = new();
        private readonly MovementTimer _distributionTimer;

        private MovementSequence _movementSequence;
        private bool _canDistributeUnits = false;
        
        public IReadOnlyList<Unit> AllUnits => _currentUnits;
        
        public CrowdController(CrowdPlaceController placeController, Unit mainUnit, UnitSpawner unitSpawner,
            DifficultyConverter difficultyConverter, CrowdMovementsController movementsController, CrowdConfig config,
            UnitSpawnerConfig spawnerConfig)
        {
            _placeController = placeController;
            _mainUnit = mainUnit;
            _unitSpawner = unitSpawner;
            _difficultyConverter = difficultyConverter;
            _movementsController = movementsController;
            _config = config;
            _spawnerConfig = spawnerConfig;
            
            _distributionTimer = new MovementTimer(_movementsController);
        }

        public void SetupCrowd(MovementSequence movementSequence, int count)
        {
            if (_currentUnits.Count > 0)
            {
                for (int i = 0; i < _currentUnits.Count; i++)
                    GameObject.Destroy(_currentUnits[i].gameObject);
            }
            
            _movementSequence = movementSequence;
            _canDistributeUnits = _difficultyConverter.IsDistributionAvailable(_movementSequence.TotalDifficultyPoints);
            float brokenPart = _difficultyConverter.ConvertToBrokenPart(_movementSequence.TotalDifficultyPoints);
            
            _currentUnits.Clear();
            
            var spawnedUnits = _unitSpawner.Spawn(count, brokenPart);
            _placeController.DistributeUnits(spawnedUnits);
            _currentUnits.AddRange(spawnedUnits);
            
            _unitTimers.Clear();
            foreach (var spawnedUnit in spawnedUnits)
                _unitTimers.Add(spawnedUnit, new MovementTimer(_movementsController));
            
            _movementsController.Setup(spawnedUnits, _mainUnit, movementSequence);
        }

        public void Start()
        {
            foreach (var (unit, timer) in _unitTimers)
            {
                if (_canDistributeUnits && unit.CurrentState != UnitState.Normal)
                {
                    var stateConfig = _config.GetStateConfig(unit.CurrentState);
                    timer.Start(stateConfig.DistributionDelay, () => DistributeUnit(unit));
                }
                
                unit.StateChanged += OnUnitStateChanged;
            }
            
            _distributionTimer.Start(_config.DistributionDelay, DistributeRandom);
            _movementsController.Start();
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

        public float GetBrokenPart()
        {
            return _currentUnits.Count(u => u.CurrentState == UnitState.Broken || u.CurrentState == UnitState.Idle) /
                   (float)_currentUnits.Count;
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
            
            _unitTimers[unit].Start(stateConfig.DistributionDelay, () => DistributeUnit(unit));
        }
        
        private void OnUnitStateChanged(Unit unit, UnitState previousState)
        {
            if (unit.CurrentState == UnitState.Invincible || previousState == UnitState.Invincible)
                return;
            
            var timer = _unitTimers[unit];
            timer.Stop();

            if (unit.CurrentState == UnitState.Normal)
            {
                unit.SetState(UnitState.Invincible);
                timer.Start(_config.InvincibleDelay, () => DisableInvincibility(unit));
            }
            else if (_canDistributeUnits)
            {
                var stateConfig = _config.GetStateConfig(unit.CurrentState);
                timer.Start(stateConfig.DistributionDelay, () => DistributeUnit(unit));
            }
        }
        
        private void DisableInvincibility(Unit unit)
        {
            unit.SetState(UnitState.Normal);
        }

        private void DistributeRandom()
        {
            var amount = Random.Range(_config.DistributionAmount.MinValue, _config.DistributionAmount.MaxValue + 1);
            var possibleUnits = _currentUnits.Where(u => u.CurrentState == UnitState.Normal).ToList();

            for (int i = 0; i < amount && possibleUnits.Count > 0; i++)
            {
                var randomIndex = Random.Range(0, possibleUnits.Count);
                var selectedUnit = possibleUnits[randomIndex];
                possibleUnits.RemoveAt(randomIndex);

                var randomState = _spawnerConfig.GetRandomBrokenState();
                selectedUnit.SetState(randomState);
            }
            
            _distributionTimer.Start(_config.DistributionDelay, DistributeRandom);
        }
    }
}