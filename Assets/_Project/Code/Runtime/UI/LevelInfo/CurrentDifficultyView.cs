using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace UI.LevelInfo
{
    public class CurrentDifficultyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _difficultyText;
        [SerializeField] private Image _fillDisplayer;
        
        private ICurrentDifficultyViewPresenter _presenter;

        public void Initialize(ICurrentDifficultyViewPresenter presenter)
        {
            _presenter = presenter;
            
            _presenter.CurrentDifficulty.Subscribe((_) => UpdateState()).AddTo(this);
        }
        
        private void UpdateState()
        {
            _difficultyText.text = $"{_presenter.CurrentDifficulty.Value} / {_presenter.MaxDifficulty}";
            
            float fillAmount = (float)_presenter.CurrentDifficulty.Value / _presenter.MaxDifficulty;
            _fillDisplayer.fillAmount = fillAmount;
        }
    }
}