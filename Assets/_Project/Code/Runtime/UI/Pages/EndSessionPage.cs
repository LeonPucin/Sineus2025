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

        public void Open(EndSessionInfo context)
        {
            SetCanvasState(true);
            
            _text.text = context.NewValue > context.OldValue ? _winText : _loseText;
            
            _slider.value = context.OldValue;
            _slider.DOValue(context.NewValue, _duration).SetEase(Ease.OutCubic);
        }

        public override void Initialize()
        {
            SetCanvasState(false);
        }

        public override void Close()
        {
            SetCanvasState(false);
        }
    }

    public class EndSessionInfo
    {
        public float OldValue { get; private set; }
        public float NewValue { get; private set; }

        public EndSessionInfo(float oldValue, float newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}