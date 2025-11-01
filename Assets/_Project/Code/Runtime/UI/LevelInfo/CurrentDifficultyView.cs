using TMPro;
using UI.Cards;
using UnityEngine;
using UniRx;

namespace UI.LevelInfo
{
    public class CurrentDifficultyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _difficultyText;
        [SerializeField] private DifficultyPointsDisplayer _pointsDisplayer;
        
        private ICurrentDifficultyViewPresenter _presenter;

        public void Initialize(ICurrentDifficultyViewPresenter presenter)
        {
            _presenter = presenter;
            
            _presenter.CurrentDifficulty.Subscribe((_) => UpdateState()).AddTo(this);
        }
        
        private void UpdateState()
        {
            _difficultyText.text = $"{_presenter.CurrentDifficulty.Value} / {_presenter.MaxDifficulty}";
            _pointsDisplayer.Display(_presenter.CurrentDifficulty.Value);
        }
    }
}