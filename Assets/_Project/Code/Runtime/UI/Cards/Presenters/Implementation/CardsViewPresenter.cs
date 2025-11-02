using System.Linq;
using Gameplay.Movements;
using Gameplay.Session;
using UniRx;

namespace UI.Cards
{
    public class CardsViewPresenter : ICardsViewPresenter
    {
        private readonly SessionInfo _sessionInfo;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        public ICardViewPresenter[] Cards { get; }
        public ReactiveCommand<MovementConfig> SelectCardEvent { get; } = new();

        public CardsViewPresenter(MovementConfigsCatalog movementsCatalog, SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;
            Cards = movementsCatalog.GetAllItems().Select(m => new CardViewPresenter(m)).ToArray<ICardViewPresenter>();

            foreach (var card in Cards)
            {
                card.SelectedEvent.Subscribe((config) =>
                {
                    foreach (var cardPresenter in Cards)
                        cardPresenter.SetSelectionAvailabilityCommand.Execute(false);
                    
                    SelectCardEvent.Execute(config);
                }).AddTo(_disposables);
            }
            
            _sessionInfo.SequenceMovementChanged += OnSequenceChanged;
            _sessionInfo.LevelChanged += OnLevelChanged;
            
            OnLevelChanged();
        }

        private void OnLevelChanged()
        {
            OnSequenceChanged(0);
        }

        private void OnSequenceChanged(int ind)
        {
            var sequence = _sessionInfo.CurrentSequence;
            bool hasFreeSlots = sequence.ValidSequence.Count < sequence.Length;
            
            foreach (var card in Cards)
                card.SetSelectionAvailabilityCommand.Execute(hasFreeSlots);
        }

        ~CardsViewPresenter()
        {
            _disposables.Dispose();
            _sessionInfo.SequenceMovementChanged -= OnSequenceChanged;
        }
    }
}