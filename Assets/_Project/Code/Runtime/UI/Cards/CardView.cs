using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cards
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private DifficultyPointsDisplayer _difficultyPointsDisplayer;
        
        [Header("Selection")]
        [SerializeField] private GameObject _outline;
        [SerializeField] private Button _selectButton;
        
        private ICardViewPresenter _presenter;

        public void Initialize(ICardViewPresenter presenter)
        {
            _presenter = presenter;
            _icon.sprite = _presenter.Icon;
            _name.text = _presenter.Name;
            _difficultyPointsDisplayer.Display(_presenter.DifficultyPoints);
            
            presenter.CanBeSelected.Subscribe((canBe) =>
            {
                _outline.SetActive(canBe);
                _selectButton.interactable = canBe;
            }).AddTo(this);
            
            _selectButton.OnClickAsObservable()
                .Subscribe(_ => _presenter.SelectRequest.Execute())
                .AddTo(this);
        }
    }
}