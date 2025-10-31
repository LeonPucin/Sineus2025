using DoubleDCore.Periphery.Base;

namespace Game.Input.Maps
{
    public class CharacterMap : Map
    {
        private readonly InputControls _inputControls;

        public CharacterMap(InputControls inputControls)
        {
            _inputControls = inputControls;
        }

        protected override void Activate()
        {
            _inputControls.Character.Enable();
        }

        protected override void Deactivate()
        {
            _inputControls.Character.Disable();
        }
    }
}