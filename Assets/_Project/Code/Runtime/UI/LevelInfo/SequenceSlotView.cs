using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace UI.LevelInfo
{
    public class SequenceSlotView : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        [Header("Add")] [SerializeField] private GameObject _addRoot;
        [SerializeField] private Button _addButton;
        [SerializeField] private GameObject _addIconBack;

        [Header("Remove")] [SerializeField] private GameObject _removeRoot;
        [SerializeField] private Button _removeButton;

        public void Initialize(ISequenceSlotViewPresenter presenter)
        {
            presenter.CanAddMovement.Subscribe((can) =>
            {
                _addRoot.SetActive(can);
                _addIconBack.SetActive(!can);
            }).AddTo(this);
            
            presenter.CanRemoveMovement.Subscribe((can) =>
            {
                _removeRoot.SetActive(can);
            }).AddTo(this);

            presenter.Icon.Subscribe((icon) =>
            {
                _icon.enabled = icon != null;
                _icon.sprite = icon;
            }).AddTo(this);

            presenter.AddRequest.BindTo(_addButton).AddTo(this);
            presenter.RemoveRequest.BindTo(_removeButton).AddTo(this);
        }
    }
}