using DoubleDCore.Identification;
using UnityEngine;

namespace Gameplay.Unit
{
    [CreateAssetMenu(fileName = "Unit Config", menuName = "Configs/UnitConfig", order = 0)]
    public class UnitConfig : IdentifyingScriptable
    {
        protected override string GetIDPrefix()
        {
            return "unit";
        }
    }
}