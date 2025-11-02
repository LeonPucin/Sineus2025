using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace UI.LevelInfo
{
    public class RageView : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void Initialize(IRageViewPresenter presenter)
        {
            presenter.Rage.Subscribe(val => _image.fillAmount = val).AddTo(this);
        }
    }
}