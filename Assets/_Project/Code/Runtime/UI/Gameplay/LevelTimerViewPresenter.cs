using System;
using Gameplay.Session;
using UniRx;

namespace UI.Gameplay
{
    public class LevelTimerViewPresenter
    {
        private readonly LevelTimer _levelTimer;
        private readonly StringReactiveProperty _timeLeftText = new("0:00");
        
        public IReadOnlyReactiveProperty<string> TimeLeftText => _timeLeftText;

        public LevelTimerViewPresenter(LevelTimer levelTimer)
        {
            _levelTimer = levelTimer;
            
            _levelTimer.Ticked += OnTicked;
            _levelTimer.Finished += OnFinished;
        }

        private void OnTicked()
        {
            int remainingTime = _levelTimer.GetRemainingTime();
            var timeSpan = TimeSpan.FromSeconds(remainingTime);
            _timeLeftText.Value = timeSpan.ToString(@"m\:ss");
        }
        
        private void OnFinished()
        {
            _timeLeftText.Value = "0:00";
        }
        
        ~LevelTimerViewPresenter()
        {
            _levelTimer.Ticked -= OnTicked;
            _levelTimer.Finished -= OnFinished;
        }
    }
}