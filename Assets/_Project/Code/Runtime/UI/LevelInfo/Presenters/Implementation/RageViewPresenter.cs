using Gameplay.Rage;
using UniRx;

namespace UI.LevelInfo
{
    public class RageViewPresenter : IRageViewPresenter
    {
        private readonly RageState _rageState;
        private readonly FloatReactiveProperty _rage = new();

        public IReadOnlyReactiveProperty<float> Rage => _rage;

        public RageViewPresenter(RageState rageState)
        {
            _rageState = rageState;
            _rage.Value = _rageState.Amount;
            _rageState.AmountChanged += OnAmountChanged;
        }

        private void OnAmountChanged(float newVal, float oldVal)
        {
            _rage.Value = newVal;
        }
        
        ~RageViewPresenter()
        {
            _rageState.AmountChanged -= OnAmountChanged;
        }
    }
}