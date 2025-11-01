using Gameplay.UnitCrowd;
using Gameplay.Units;
using UniRx;
using Unit = Gameplay.Units.Unit;

namespace UI.Gameplay
{
    public class LevelStateViewPresenter
    {
        private readonly CrowdController _crowdController;
        private readonly FloatReactiveProperty _normalUnitsPart;
        
        public IReadOnlyReactiveProperty<float> NormalUnitsPart => _normalUnitsPart;

        public LevelStateViewPresenter(CrowdController crowdController)
        {
            _crowdController = crowdController;
            _normalUnitsPart = new FloatReactiveProperty(1 - _crowdController.GetBrokenPart());

            foreach (var unit in _crowdController.AllUnits)
                unit.StateChanged += OnUnitStateChanged;
        }

        private void OnUnitStateChanged(Unit arg1, UnitState arg2)
        {
            _normalUnitsPart.Value = 1 - _crowdController.GetBrokenPart();
        }
        
        ~LevelStateViewPresenter()
        {
            foreach (var unit in _crowdController.AllUnits)
                unit.StateChanged -= OnUnitStateChanged;
        }
    }
}