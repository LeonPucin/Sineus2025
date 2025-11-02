using DoubleDCore.Periphery.Base;
using DoubleDCore.UI.Base;
using Game.Input;
using Game.Input.Maps;
using Gameplay.Rage;
using Gameplay.Skills;
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
        private readonly AlterKeysListener _alterKeysListener;
        private readonly MainThreadInputCommander _mainThreadInputCommander;
        private readonly SkillActivator _skillActivator;
        private readonly WinChecker _winChecker;
        private readonly RageState _rageState;

        public LevelStarter(SessionInfo sessionInfo, CrowdController crowdController, IUIManager uiManager,
            DifficultyConverter difficultyConverter, LevelTimer levelTimer, AlterKeysListener alterKeysListener,
            MainThreadInputCommander mainThreadInputCommander, SkillActivator skillActivator, WinChecker winChecker, 
            RageState rageState)
        {
            _sessionInfo = sessionInfo;
            _crowdController = crowdController;
            _uiManager = uiManager;
            _difficultyConverter = difficultyConverter;
            _levelTimer = levelTimer;
            _alterKeysListener = alterKeysListener;
            _mainThreadInputCommander = mainThreadInputCommander;
            _skillActivator = skillActivator;
            _winChecker = winChecker;
            _rageState = rageState;
        }

        public void SetupLevel()
        {
            _uiManager.ClosePage<PlayerChoosePage>();
            _uiManager.OpenPage<StartLevelPage>();
            
            var currentSequence = _sessionInfo.CurrentSequence;
            _crowdController.SetupCrowd(currentSequence,
                _difficultyConverter.ConvertToCrowdSize(currentSequence.TotalDifficultyPoints));
            _skillActivator.ResetCooldowns();
        }
        
        public void StartLevel()
        {
            _uiManager.ClosePage<StartLevelPage>();
            _uiManager.OpenPage<GameplayPage>();
            
            _mainThreadInputCommander.SwitchMap<CharacterMap>();

            _levelTimer.Finished += StopLevel;
            
            _crowdController.Start();
            _levelTimer.Start();
            _alterKeysListener.enabled = true;
        }

        public void StopLevel()
        {
            _levelTimer.Finished -= StopLevel;
            
            _mainThreadInputCommander.SwitchMap<UIMap>();
            
            _crowdController.Stop();
            _skillActivator.DeactivateCurrent();
            _alterKeysListener.enabled = false;
            
            _uiManager.ClosePage<GameplayPage>();

            bool isWin = _winChecker.IsLevelWinned();
            bool isLastLevel = _sessionInfo.IsLastLevel();
            float currentRage = _rageState.Amount;

            if (!isWin)
            {
                _rageState.RemoveAmount(
                    _difficultyConverter.GetRageAddition(_sessionInfo.CurrentSequence.TotalDifficultyPoints));
            }

            float newRage = _rageState.Amount;
            
            if (!_sessionInfo.IsLastLevel() && isWin)
                _sessionInfo.MoveToNextLevel();
            
            _uiManager.OpenPage<EndSessionPage, EndSessionInfo>(new EndSessionInfo(currentRage, newRage, isWin, isLastLevel));
        }
    }
}