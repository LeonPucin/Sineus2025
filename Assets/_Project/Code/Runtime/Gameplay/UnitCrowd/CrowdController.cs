using System.Collections.Generic;
using System.Linq;
using Gameplay.Movements;
using Gameplay.Units;

namespace Gameplay.UnitCrowd
{
    public class CrowdController
    {
        private readonly UnitSpawner _unitSpawner;
        private readonly DifficultyConverter _difficultyConverter;
        private readonly CrowdMovementsController _movementsController;
        private readonly CrowdPlaceController _placeController;
        private readonly Unit _mainUnit;
        
        public CrowdController(CrowdPlaceController placeController, Unit mainUnit, UnitSpawner unitSpawner,
            DifficultyConverter difficultyConverter, CrowdMovementsController movementsController)
        {
            _placeController = placeController;
            _mainUnit = mainUnit;
            _unitSpawner = unitSpawner;
            _difficultyConverter = difficultyConverter;
            _movementsController = movementsController;
        }

        public void SetupCrowd(MovementSequence movementSequence, int count)
        {
            float brokenPart =
                _difficultyConverter.ConvertToBrokenPart(movementSequence.CurrentMovements
                    .Select(m => m.DifficultyPoints).Sum());
            
            var spawnedUnits = _unitSpawner.Spawn(count, brokenPart);
            _placeController.DistributeUnits(spawnedUnits);
            
            spawnedUnits.Add(_mainUnit);
            _movementsController.Setup(spawnedUnits, movementSequence);
            
            _movementsController.Start(); //TODO: invoke in another place after user confirmation
        }
    }
}