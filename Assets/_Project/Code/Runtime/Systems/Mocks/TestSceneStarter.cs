using DoubleDCore.UI.Base;
using Gameplay.Movements;
using Gameplay.Session;
using Gameplay.UnitCrowd;
using UI.Pages;
using UnityEngine;
using Zenject;

namespace Systems.Mocks
{
    public class TestSceneStarter : MonoBehaviour
    {
        [SerializeField] private MovementConfig[] _startSeq;
        [SerializeField] private int _unitCount = 20;
        
        private CrowdController _crowdController;
        private SessionInfo _sessionInfo;
        private IUIManager _uiManager;

        [Inject]
        private void Init(CrowdController crowdController, SessionInfo sessionInfo, IUIManager uiManager)
        {
            _crowdController = crowdController;
            _sessionInfo = sessionInfo;
            _uiManager = uiManager;
        }
        
        private void Start()
        {
            _uiManager.OpenPage<PlayerChoosePage>();
            
            // var movementSequence = _sessionInfo.CurrentSequence;
            //
            // for (int i = 0; i < _startSeq.Length; i++)
            // {
            //     movementSequence.SetMovement(i, _startSeq[i]);
            // }
            //
            // _crowdController.SetupCrowd(movementSequence, _unitCount);
        }
    }
}