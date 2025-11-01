using UnityEngine;

namespace UI.Cards
{
    public class CardsView : MonoBehaviour
    {
        [SerializeField] private CardView[] _cardViews;
        
        private ICardsViewPresenter _presenter;

        public void Initialize(ICardsViewPresenter presenter)
        {
            _presenter = presenter;
            
            for (int i = 0; i < _cardViews.Length; i++)
                _cardViews[i].Initialize(_presenter.Cards[i]);
        }
    }
}