using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using Gameplay.Session;
using Gameplay.UnitCrowd;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pages
{
    public class StartGamePage : MonoPage, IUIPage
    {
        [SerializeField] private Button _startButton;
        
        private LevelStarter _levelStarter;

        [Zenject.Inject]
        private void Init(LevelStarter levelStarter)
        {
            _levelStarter = levelStarter;
        }
        
        public override void Initialize()
        {
            SetCanvasState(false);
        }

        public void Open()
        {
            SetCanvasState(true);
            
            _startButton.onClick.AddListener(OnStartButtonClicked);
        }

        private void OnStartButtonClicked()
        {
            _levelStarter.StartLevel();
        }

        public override void Close()
        {
            SetCanvasState(false);
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
        }
    }
}