using TMPro;
using UniRx;
using UnityEngine;

namespace UI.QuestInfo
{
    public class QuestInfoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _tip;
        
        public void Initialize(QuestInfoViewPresenter presenter)
        {
            presenter.Name.Subscribe(x => _name.text = x).AddTo(this);
            presenter.Description.Subscribe(x => _description.text = x).AddTo(this);
            presenter.Tip.Subscribe(x => _tip.text = x).AddTo(this);
        }
    }
}