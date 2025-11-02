using DG.Tweening;
using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pages
{
    public class EndSessionPage : MonoPage, IPayloadPage<EndSessionInfo>
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Slider _slider;
        [SerializeField] private Button _closeButton;

        [SerializeField] private string _winText;
        [SerializeField] private string _loseText;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _startDelay = 0.5f;
        
        private IUIManager _uiManager;

        [Zenject.Inject]
        private void Init(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        public void Open(EndSessionInfo context)
        {
            SetCanvasState(true);
            
            _text.text = context.IsSuccess ? _winText : _loseText;
            _slider.gameObject.SetActive(!context.IsSuccess);
            
            _slider.value = context.OldValue;
            _slider.DOValue(context.NewValue, _duration).SetEase(Ease.OutCubic).SetDelay(_startDelay);
            
            _closeButton.onClick.AddListener(OnCloseRequested);
        }

        private void OnCloseRequested()
        {
            _uiManager.ClosePage<EndSessionPage>();
            _uiManager.OpenPage<PlayerChoosePage>();
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

        public EndSessionInfo(float oldValue, float newValue, bool isSuccess)
        {
            OldValue = oldValue;
            NewValue = newValue;
            IsSuccess = isSuccess;
        }
    }
}