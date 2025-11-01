using System.Linq;
using Gameplay.Movements;
using UniRx;

namespace UI.Cards
{
    public class CardsViewPresenter : ICardsViewPresenter
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        public ICardViewPresenter[] Cards { get; }
        public ReactiveCommand<int> SelectCardCommand { get; } = new();
        public ReactiveCommand<(int, MovementConfig)> SelectedCardEvent { get; } = new();

        public CardsViewPresenter(MovementConfigsCatalog movementsCatalog)
        {
            Cards = movementsCatalog.GetAllItems().Select(m => new CardViewPresenter(m)).ToArray<ICardViewPresenter>();

            foreach (var card in Cards)
            {
                card.SelectedEvent.Subscribe((info) =>
                {
                    foreach (var cardPresenter in Cards)
                        cardPresenter.DisableSelectionCommand.Execute();
                    
                    SelectedCardEvent.Execute(info);
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