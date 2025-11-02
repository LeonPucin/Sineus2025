using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using UI.Cards;
using UI.LevelInfo;
using UI.QuestInfo;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.Pages
{
    public class PlayerChoosePage : MonoPage, IUIPage
    {
        [SerializeField] private CardsView _cardsView;
        [SerializeField] private LevelInfoView _levelInfoView;
        [SerializeField] private QuestInfoView _questInfoView;
        [SerializeField] private RageView _rageView;
        
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
            var questInfoViewPresenter = _container.Instantiate<QuestInfoViewPresenter>();
            var rageViewPresenter = _container.Instantiate<RageViewPresenter>();
            
            _cardsView.Initialize(cardsViewPresenter);
            _levelInfoView.Initialize(levelInfoViewPresenter);
            _questInfoView.Initialize(questInfoViewPresenter);
            _rageView.Initialize(rageViewPresenter);

            cardsViewPresenter.SelectCardEvent.Subscribe((config) =>
            {
                levelInfoViewPresenter.AddMovementRequest.Execute(config);
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