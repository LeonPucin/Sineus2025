using System;

namespace Gameplay.Units
{
    [Serializable]
    public enum UnitState
    {
        Normal = 0,
        Idle = 1,
        Broken = 2,
        Invincible = 3
    }
}