using System.Linq;
using Gameplay.Movements;
using UniRx;

namespace UI.LevelInfo
{
    public class CurrentDifficultyViewPresenter : ICurrentDifficultyViewPresenter
    {
        private readonly IntReactiveProperty _currentDifficulty = new(0);
        private readonly CompositeDisposable _disposables = new();
        
        public IReadOnlyReactiveProperty<int> CurrentDifficulty => _currentDifficulty;
        public int MaxDifficulty { get; }
        public ReactiveCommand<int> ChangeDifficultyCommand { get; } = new();

        public CurrentDifficultyViewPresenter(MovementConfigsCatalog movementsCatalog)
        {
            MaxDifficulty = movementsCatalog.GetAllItems().Sum(x => x.DifficultyPoints);

            ChangeDifficultyCommand.Subscribe((val) =>
            {
                _currentDifficulty.Value = val;
            }).AddTo(_disposables);
        }
        
        ~CurrentDifficultyViewPresenter()
        {
            _disposables.Dispose();
        }
    }
}