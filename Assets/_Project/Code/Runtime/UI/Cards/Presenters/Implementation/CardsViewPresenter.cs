using System.Linq;
using Gameplay.Movements;
using Gameplay.Session;
using UniRx;
using UnityEngine;

namespace UI.Cards
{
    public class CardsViewPresenter : ICardsViewPresenter
    {
        private readonly SessionInfo _sessionInfo;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        public ICardViewPresenter[] Cards { get; }
        public ReactiveCommand<int> SelectCardCommand { get; } = new();

        public CardsViewPresenter(MovementConfigsCatalog movementsCatalog, SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;
            Cards = movementsCatalog.GetAllItems().Select(m => new CardViewPresenter(m)).ToArray<ICardViewPresenter>();

            foreach (var card in Cards)
            {
                card.SelectedEvent.Subscribe((info) =>
                {
                    foreach (var cardPresenter in Cards)
                        cardPresenter.DisableSelectionCommand.Execute();
                    
                    _sessionInfo.CurrentSequence.SetMovement(info.Item1, info.Item2);
                }).AddTo(_disposables);
            }
            
            SelectCardCommand.Subscribe((index) =>
            {
                foreach (var card in Cards)
                    card.SelectCommand.Execute(index);
            }).AddTo(_disposables);
        }
        
        ~CardsViewPresenter()
        {
            _disposables.Dispose();
        }
    }
}