using TMPro;
using UniRx;
using UnityEngine;

namespace UI.Gameplay
{
    public class LevelTimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        
        private LevelTimerViewPresenter _presenter;
        private CompositeDisposable _disposables = new();
        
        public void Initialize(LevelTimerViewPresenter presenter)
        {
            if (_presenter != null)
            {
                _disposables.Dispose();
                _presenter = null;
            }

            _presenter = presenter;
            _presenter.TimeLeftText.Subscribe(txt => _timerText.text = txt).AddTo(_disposables);
        }

        private void OnDestroy()
        {
            if (_presenter != null)
            {
                _disposables.Dispose();
                _presenter = null;
            }
        }
    }
}