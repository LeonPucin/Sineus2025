using Gameplay.Movements;
using Gameplay.UnitCrowd;
using UnityEngine;
using Zenject;

namespace Systems.Mocks
{
    public class TestSceneStarter : MonoBehaviour
    {
        [SerializeField] private MovementConfig[] _startSeq;
        [SerializeField] private int _unitCount = 20;
        
        private CrowdController _crowdController;

        [Inject]
        private void Init(CrowdController crowdController)
        {
            _crowdController = crowdController;
        }
        
        private void Start()
        {
            var movementSequence = new MovementSequence();
            
            foreach (var config in _startSeq)
                movementSequence.AddMovement(config);
            
            _crowdController.SetupCrowd(movementSequence, _unitCount);
        }
    }
}