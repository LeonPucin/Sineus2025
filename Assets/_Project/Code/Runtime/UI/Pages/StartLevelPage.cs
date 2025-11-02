using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using Gameplay.Session;
using Gameplay.UnitCrowd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pages
{
    public class StartLevelPage : MonoPage, IUIPage
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private TMP_Text _levelTitle;
        
        private LevelStarter _levelStarter;
        private SessionInfo _sessionInfo;

        [Zenject.Inject]
        private void Init(LevelStarter levelStarter, SessionInfo sessionInfo)
        {
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
            _levelTitle.text = _sessionInfo.CurrentLevel.Name;
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