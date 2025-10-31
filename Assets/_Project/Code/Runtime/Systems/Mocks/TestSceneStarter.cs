using Gameplay.Movements;
using Gameplay.Unit;
using Zenject;

namespace Systems.Mocks
{
    public class TestSceneStarter : IInitializable
    {
        private readonly Unit _mainUnit;
        private readonly MovementConfigsCatalog _movementsCatalog;

        public TestSceneStarter(Unit mainUnit, MovementConfigsCatalog movementsCatalog)
        {
            _mainUnit = mainUnit;
            _movementsCatalog = movementsCatalog;
        }

        public void Initialize()
        {
            var moveSequence = new MoveSequence();
            var allItems = _movementsCatalog.GetAllItems();

            for (int i = 0; i < allItems.Length; i++)
            {
                moveSequence.AddMovement(allItems[i]);
            }
            
            _mainUnit.Movements.SetSequence(moveSequence);
            _mainUnit.Movements.StartPlaying();
        }
    }
}