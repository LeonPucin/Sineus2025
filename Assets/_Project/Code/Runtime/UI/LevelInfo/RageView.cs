using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace UI.LevelInfo
{
    public class RageView : MonoBehaviour
    {
        //[SerializeField] private Slider _slider;
        [SerializeField] private Image _image;

        public void Initialize(IRageViewPresenter presenter)
        {
            //presenter.Rage.Subscribe(val => _slider.value = val).AddTo(this);
            presenter.Rage.Subscribe(val => _image.fillAmount = 1 - val).AddTo(this);
        }
    }
}