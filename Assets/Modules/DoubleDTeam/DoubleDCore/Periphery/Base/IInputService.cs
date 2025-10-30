namespace DoubleDCore.Periphery.Base
{
    public interface IInputService<out TProvider>
    {
        public TProvider GetInputProvider();
        
        public void SwitchMap<TMap>() where TMap : Map;
        
        public void Enable();
        public void Disable();
    }
}