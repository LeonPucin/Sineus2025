using TMPro;
using UI.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cards
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private DifficultyPointsDisplayer _difficultyPointsDisplayer;
        [SerializeField] private PointerCapturer _pointerCapturer;
        
        [Header("Selection")]
        [SerializeField] private GameObject _outline;
        
        private ICardViewPresenter _presenter;

        public void Initialize(ICardViewPresenter presenter)
        {
            _presenter = presenter;
            _icon.sprite = _presenter.Icon;
            _name.text = _presenter.Name;
            _difficultyPointsDisplayer.Display(_presenter.DifficultyPoints);
        }

        private void OnEnable()
        {
            _pointerCapturer.Entered += OnPointerEntered;
            _pointerCapturer.Exited += OnPoinerExited;
            _pointerCapturer.Clicked += OnClicked;
        }

        private void OnPointerEntered()
        {
            if (_presenter.CanBeSelected.Value)
                _outline.SetActive(true);
        }
        
        private void OnPoinerExited()
        {
            _outline.SetActive(false);
        }

        private void OnClicked()
        {
            if (_presenter.CanBeSelected.Value)
            {
                _presenter.SelectRequest.Execute();
                _outline.SetActive(false);
            }
        }

        private void OnDisable()
        {
            _pointerCapturer.Entered -= OnPointerEntered;
            _pointerCapturer.Exited -= OnPoinerExited;
            _pointerCapturer.Clicked -= OnClicked;
        }
    }
}