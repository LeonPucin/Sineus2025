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
        private readonly DifficultyConverter _difficultyConverter;
        private readonly LevelTimer _levelTimer;
        private readonly InputControls _inputControls;
        
        public LevelStarter(SessionInfo sessionInfo, IInputService<InputControls> inputService,
            CrowdController crowdController, IUIManager uiManager, DifficultyConverter difficultyConverter,
            LevelTimer levelTimer)
        {
            _sessionInfo = sessionInfo;
            _crowdController = crowdController;
            _uiManager = uiManager;
            _difficultyConverter = difficultyConverter;
            _levelTimer = levelTimer;
            _inputControls = inputService.GetInputProvider();
        }

        public void SetupLevel()
        {
            _inputControls.UI.Disable();
            _inputControls.Character.Enable();
            
            _uiManager.ClosePage<PlayerChoosePage>();
            _uiManager.OpenPage<StartGamePage>();
            
            var currentSequence = _sessionInfo.CurrentSequence;
            _crowdController.SetupCrowd(currentSequence,
                _difficultyConverter.ConvertToCrowdSize(currentSequence.TotalDifficultyPoints));
        }
        
        public void StartLevel()
        {
            _uiManager.ClosePage<StartGamePage>();
            _uiManager.OpenPage<GameplayPage>();

            _levelTimer.Finished += StopLevel;
            
            _crowdController.Start();
            _levelTimer.Start();
        }

        public void StopLevel()
        {
            _levelTimer.Finished -= StopLevel;
            
            _inputControls.Character.Disable();
            _inputControls.UI.Enable();
            
            _crowdController.Stop();
            
            _uiManager.ClosePage<GameplayPage>();
            
            if (!_sessionInfo.IsLastLevel())
                _sessionInfo.MoveToNextLevel();
            
            _uiManager.OpenPage<PlayerChoosePage>(); //TODO: add statistics page
        }
    }
}