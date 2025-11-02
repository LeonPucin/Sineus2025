using DG.Tweening;
using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using Gameplay.Rage;
using Gameplay.Session;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pages
{
    public class EndSessionPage : MonoPage, IPayloadPage<EndSessionInfo>
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _slider;
        [SerializeField] private Button _closeButton;

        [SerializeField] private string _winText;
        [SerializeField] private string _loseText;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _startDelay = 0.5f;
        
        private IUIManager _uiManager;
        private RageState _rageState;
        private SessionInfo _sessionInfo;
        private EndSessionInfo _context;

        [Zenject.Inject]
        private void Init(IUIManager uiManager, RageState rageState, SessionInfo sessionInfo)
        {
            _uiManager = uiManager;
            _rageState = rageState;
            _sessionInfo = sessionInfo;
        }

        public void Open(EndSessionInfo context)
        {
            _context = context;
            
            SetCanvasState(true);
            
            _text.text = context.IsSuccess ? _winText : _loseText;
            
            _slider.fillAmount = context.OldValue;
            _slider.DOFillAmount(context.NewValue, _duration).SetEase(Ease.OutCubic).SetDelay(_startDelay);
            
            _closeButton.onClick.AddListener(OnCloseRequested);
        }

        private void OnCloseRequested()
        {
            _uiManager.ClosePage<EndSessionPage>();

            if (_rageState.Amount == 0)
            {
                _uiManager.OpenPage<EndGamePage, EndGameInfo>(new EndGameInfo { IsWin = false });
            }
            else if (_context.IsLastLevel && _context.IsSuccess)
            {
                _uiManager.OpenPage<EndGamePage, EndGameInfo>(new EndGameInfo { IsWin = true });
            }
            else
            {
                _uiManager.OpenPage<PlayerChoosePage>();
            }
        }

        public override void Initialize()
        {
            SetCanvasState(false);
        }

        public override void Close()
        {
            _closeButton.onClick.RemoveListener(OnCloseRequested);
            SetCanvasState(false);
        }
    }

    public class EndSessionInfo
    {
        public float OldValue { get; }
        public float NewValue { get; }
        public bool IsSuccess { get; }
        public bool IsLastLevel { get; }

        public EndSessionInfo(float oldValue, float newValue, bool isSuccess, bool isLastLevel)
        {
            OldValue = oldValue;
            NewValue = newValue;
            IsSuccess = isSuccess;
            IsLastLevel = isLastLevel;
        }
    }
}