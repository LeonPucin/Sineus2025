using DoubleDCore.Storage.Base;
using DoubleDCore.TimeTools;
using UnityEngine;
using Zenject;

namespace Game.Utility
{
    public class AutoSaver : MonoBehaviour
    {
        [SerializeField] private float _saveDelay = 10;

        private ISaveController _saveController;
        private readonly Timer _saveTimer = new(TimeBindingType.RealTime);

        [Inject]
        private void Init(ISaveController saveController)
        {
            _saveController = saveController;
        }

        private void Awake()
        {
            _saveTimer.Start(_saveDelay, OnTimerEnded);
        }

        private void OnTimerEnded()
        {
            _saveController.SaveAll();
            _saveTimer.Start(_saveDelay, OnTimerEnded);
        }

        private void OnDestroy()
        {
            _saveTimer.Stop();
        }
    }
}