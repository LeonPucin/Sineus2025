using DoubleDCore.Periphery.Base;
using DoubleDCore.UI.Base;
using Gameplay.UnitCrowd;
using UI.Pages;
using UI.Pages.SkillsPage;

namespace Gameplay.Session
{
    public class LevelStarter
    {
        private readonly SessionInfo _sessionInfo;
        private readonly CrowdController _crowdController;
        private readonly IUIManager _uiManager;
        private readonly InputControls _inputControls;
        
        public LevelStarter(SessionInfo sessionInfo, IInputService<InputControls> inputService,
            CrowdController crowdController, IUIManager uiManager)
        {
            _sessionInfo = sessionInfo;
            _crowdController = crowdController;
            _uiManager = uiManager;
            _inputControls = inputService.GetInputProvider();
        }

        public void StartLevel()
        {
            _inputControls.UI.Disable();
            _inputControls.Character.Enable();
            
            _uiManager.ClosePage<PlayerChoosePage>();
            _uiManager.OpenPage<SkillsPage>();
            
            _crowdController.SetupCrowd(_sessionInfo.CurrentSequence, 20);
            _crowdController.Start();
        }

        public void StopLevel()
        {
            _inputControls.Character.Disable();
            _inputControls.UI.Enable();
            
            _crowdController.Stop();
        }
    }
}