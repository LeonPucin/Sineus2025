using DoubleDCore.Periphery.Base;

namespace Game.Input.Maps
{
    public class UIMap : Map
    {
        private readonly InputControls _inputControls;

        public UIMap(InputControls inputControls)
        {
            _inputControls = inputControls;
        }

        protected override void Activate()
        {
            _inputControls.UI.Enable();
        }

        protected override void Deactivate()
        {
            _inputControls.UI.Disable();
        }
    }
}