using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DoubleDCore.TimeTools
{
    public class Timer
    {
        private readonly TimeBindingType _timeBinding;

        public float RemainingTime { get; private set; }

        private bool _isWorking;
        public bool IsWorking => _isWorking;

        public event Action<float> Started;
        public event Action<float> Stopped;
        public event Action Performed;

        private CancellationTokenSource _cancellationTokenSource;

        public Timer(TimeBindingType timeBindingType)
        {
            _timeBinding = timeBindingType;
        }

        public async UniTask Start(float time, Action onEnd = null, Action<float> onTick = null)
        {
            if (_isWorking)
                Stop();

            _cancellationTokenSource = new CancellationTokenSource();

            _isWorking = true;
            RemainingTime = time;

            float initialTime = time;

            Started?.Invoke(time);

            try
            {
                while (RemainingTime > 0)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, _cancellationTokenSource.Token);
                    
                    if (_cancellationTokenSource.IsCancellationRequested)
                        break;

                    float delta = _timeBinding switch
                    {
                        TimeBindingType.RealTime => Time.unscaledDeltaTime,
                        TimeBindingType.ScaledTime => Time.deltaTime,
                        TimeBindingType.FixedTime => Time.fixedDeltaTime,
                        _ => throw new ArgumentOutOfRangeException($"{_timeBinding} is not supported")
                    };

                    RemainingTime -= delta;
                    onTick?.Invoke(1 - RemainingTime / initialTime);
                }

                RemainingTime = 0;

                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    onEnd?.Invoke();
                    Performed?.Invoke();
                }
            }
            finally
            {
                _isWorking = false;
                Stopped?.Invoke(RemainingTime);
            }
        }

        public void Stop()
        {
            if (_isWorking == false)
                return;

            _cancellationTokenSource?.Cancel();
            RemainingTime = 0;
            _isWorking = false;
        }
    }
}