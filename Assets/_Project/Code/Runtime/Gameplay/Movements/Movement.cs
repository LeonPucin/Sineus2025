namespace Gameplay.Movements
{
    public class Movement
    {
        public MovementConfig Config { get; }

        public Movement(MovementConfig config)
        {
            Config = config;
        }
    }
}