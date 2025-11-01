using System;

namespace Game.Utility
{
    [Serializable]
    public struct RangedValue
    {
        public float MinValue;
        public float MaxValue;
    }
    
    [Serializable]
    public struct RangedValueInt
    {
        public int MinValue;
        public int MaxValue;
    }
}