using System;
using DoubleDCore.TimeTools;
using UnityEngine;

namespace Gameplay.Session
{
    public class LevelTimer
    {
        private readonly SessionInfo _sessionInfo;
        private readonly Timer _timer = new(TimeBindingType.ScaledTime);
        
        public event Action Finished;
        public event Action Ticked; 
        
        public LevelTimer(SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;
        }

        public void Start()
        {
            _ = _timer.Start(_sessionInfo.CurrentLevel.TimeLimit, () => Finished?.Invoke(), (_) => Ticked?.Invoke());
        }

        public int GetRemainingTime()
        {
            return Mathf.RoundToInt(_timer.RemainingTime);
        }
    }
}