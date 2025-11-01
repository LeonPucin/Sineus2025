using UnityEngine;

namespace UI.LevelInfo
{
    public class CurrentSequenceView : MonoBehaviour
    {
        [SerializeField] private SequenceSlotView[] _slotViews;

        public void Initialize(ICurrentSequenceViewPresenter presenter)
        {
            for (int i = 0; i < _slotViews.Length; i++)
                _slotViews[i].Initialize(presenter.SequenceSlots[i]);
        }
    }
}