using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using UI.Cards;
using UI.LevelInfo;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.Pages
{
    public class PlayerChoosePage : MonoPage, IUIPage
    {
        [SerializeField] private CardsView _cardsView;
        [SerializeField] private LevelInfoView _levelInfoView;
        
        private DiContainer _container;

        [Inject]
        private void Init(DiContainer diContainer)
        {
            _container = diContainer;
        }
        
        public override void Initialize()
        {
            var cardsViewPresenter = _container.Instantiate<CardsViewPresenter>();
            var levelInfoViewPresenter = _container.Instantiate<LevelInfoViewPresenter>();
            
            _cardsView.Initialize(cardsViewPresenter);
            _levelInfoView.Initialize(levelInfoViewPresenter);

            levelInfoViewPresenter.AddMovementRequest.Subscribe((index) =>
            {
                cardsViewPresenter.SelectCardCommand.Execute(index);
            }).AddTo(this);
            
            cardsViewPresenter.SelectedCardEvent.Subscribe((info) =>
            {
                levelInfoViewPresenter.AddMovementCommand.Execute(info);
            }).AddTo(this);
            
            SetCanvasState(true);
        }

        public void Open()
        {
            SetCanvasState(true);
        }

        public override void Close()
        {
            SetCanvasState(false);
        }
    }
}