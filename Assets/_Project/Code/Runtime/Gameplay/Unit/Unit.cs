using Gameplay.Movements;
using UnityEngine;

namespace Gameplay.Unit
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitConfig _config;
        
        [Header("Components")]
        [SerializeField] private MovementsComponent _movements;

        public MovementsComponent Movements => _movements;

        [Zenject.Inject]
        private void Init()
        {
            _movements.Init(_config);
        }
    }
}