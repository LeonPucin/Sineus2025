using Gameplay.Movements;
using UniRx;

namespace UI.Cards
{
    public interface ICardsViewPresenter
    {
        ICardViewPresenter[] Cards { get; }
        ReactiveCommand<int> SelectCardCommand { get; }
    }
}