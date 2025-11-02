using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using Gameplay.Session;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pages
{
    public class EducationPage : MonoPage, IUIPage
    {
        [SerializeField] private Button _startButton;
        
        private IUIManager _uiManager;
        private LevelStarter _levelStarter;
        private SessionInfo _sessionInfo;

        [Zenject.Inject]
        private void Init(IUIManager uiManager, LevelStarter levelStarter, SessionInfo sessionInfo)
        {
            _uiManager = uiManager;
            _levelStarter = levelStarter;
            _sessionInfo = sessionInfo;
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
            _sessionInfo.IsEducated = true;
            _uiManager.ClosePage<EducationPage>();
            _levelStarter.StartLevel();
        }

        public override void Close()
        {
            SetCanvasState(false);
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
        }
    }
}