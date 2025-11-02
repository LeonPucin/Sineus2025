using System;

namespace Gameplay.Rage
{
    public class RageState
    {
        public float Amount { get; private set; } = 0;
        
        public event Action<float, float> AmountChanged;
        
        public void AddAmount(float amount)
        {
            float previousAmount = Amount;
            Amount += amount;
            AmountChanged?.Invoke(Amount, previousAmount);
        }
    }
}