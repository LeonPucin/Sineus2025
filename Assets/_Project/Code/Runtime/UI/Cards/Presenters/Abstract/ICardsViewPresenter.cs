using Gameplay.Movements;
using UniRx;

namespace UI.Cards
{
    public interface ICardsViewPresenter
    {
        ICardViewPresenter[] Cards { get; }
        ReactiveCommand<MovementConfig> SelectCardEvent { get; }
    }
}