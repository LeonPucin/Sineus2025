using System;

namespace Gameplay.Rage
{
    public class RageState
    {
        public float Amount { get; private set; } = 1;
        
        public event Action<float, float> AmountChanged;
        
        public void RemoveAmount(float amount)
        {
            float previousAmount = Amount;
            Amount -= amount;
            Amount = Math.Max(Amount, 0);
            AmountChanged?.Invoke(Amount, previousAmount);
        }
    }
}