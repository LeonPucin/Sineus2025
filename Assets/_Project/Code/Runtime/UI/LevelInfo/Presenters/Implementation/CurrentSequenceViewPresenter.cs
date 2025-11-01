using System;
using Gameplay.Movements;
using Gameplay.Session;
using UniRx;
using UnityEngine;

namespace UI.LevelInfo
{
    public class CurrentSequenceViewPresenter : ICurrentSequenceViewPresenter
    {
        private readonly SessionInfo _sessionInfo;
        private readonly CompositeDisposable _disposables = new();
        
        public ISequenceSlotViewPresenter[] SequenceSlots { get; }
        public ReactiveCommand<int> AddMovementRequest { get; } = new();
        public ReactiveCommand<(int, MovementConfig)> AddMovementCommand { get; } = new();

        public CurrentSequenceViewPresenter(SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;
            int slotCount = sessionInfo.CurrentSequence.Length;
            SequenceSlots = new ISequenceSlotViewPresenter[slotCount];

            for (int i = 0; i < slotCount; i++)
            {
                int currentIndex = i;
                var slotPresenter = new SequenceSlotViewPresenter();
                
                slotPresenter.AddRequest.Subscribe((_) =>
                {
                    AddMovementRequest.Execute(currentIndex);
                }).AddTo(_disposables);
                
                slotPresenter.RemoveRequest.Subscribe((_) =>
                {
                    try
                    {
                        sessionInfo.CurrentSequence.RemoveMovement(currentIndex);
                        slotPresenter.RemoveCommand.Execute();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Error removing movement: " + e);
                    }
                }).AddTo(_disposables);
                
                SequenceSlots[i] = slotPresenter;
            }
            
            AddMovementCommand.Subscribe(OnAddMovement).AddTo(_disposables);
        }

        private void OnAddMovement((int, MovementConfig) info)
        {
            int index = info.Item1;
            MovementConfig config = info.Item2;
            _sessionInfo.CurrentSequence.SetMovement(index, config);
        }

        ~CurrentSequenceViewPresenter()
        {
            _disposables.Dispose();
        }
    }
}