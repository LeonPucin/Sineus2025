using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace UI.LevelInfo
{
    public class RageView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        public void Initialize(IRageViewPresenter presenter)
        {
            presenter.Rage.Subscribe(val => _slider.value = val).AddTo(this);
        }
    }
}