using Gameplay.UnitCrowd;

namespace Gameplay.Session
{
    public class WinChecker
    {
        private readonly SessionInfo _sessionInfo;
        private readonly CrowdController _crowdController;

        public WinChecker(SessionInfo sessionInfo, CrowdController crowdController)
        {
            _sessionInfo = sessionInfo;
            _crowdController = crowdController;
        }
        
        public bool IsLevelWinned()
        {
            float normalPart = 1 - _crowdController.GetBrokenPart();
            return normalPart >= _sessionInfo.CurrentLevel.WinThreshold;
        }
    }
}