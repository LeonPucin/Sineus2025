using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LevelInfo
{
    public class LevelInfoView : MonoBehaviour
    {
        [SerializeField] private CurrentSequenceView _currentSequenceView;
        [SerializeField] private CurrentDifficultyView _currentDifficultyView;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _playButton;
        
        public void Initialize(ILevelInfoViewPresenter presenter)
        {
            _currentSequenceView.Initialize(presenter.CurrentSequenceViewPresenter);
            _currentDifficultyView.Initialize(presenter.CurrentDifficultyViewPresenter);
            
            presenter.Name.Subscribe((txt) => _name.text = txt).AddTo(this);
            presenter.Description.Subscribe((txt) => _description.text = txt).AddTo(this);
            presenter.PlayRequest.BindTo(_playButton).AddTo(this);
        }
    }
}