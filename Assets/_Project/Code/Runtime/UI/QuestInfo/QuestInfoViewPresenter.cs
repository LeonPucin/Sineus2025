using Gameplay.Session;
using UniRx;

namespace UI.QuestInfo
{
    public class QuestInfoViewPresenter
    {
        private readonly SessionInfo _sessionInfo;
        private readonly StringReactiveProperty _name = new();
        private readonly StringReactiveProperty _description = new();
        private readonly StringReactiveProperty _tip = new();
        
        public IReadOnlyReactiveProperty<string> Name => _name;
        public IReadOnlyReactiveProperty<string> Description => _description;
        public IReadOnlyReactiveProperty<string> Tip => _tip;

        public QuestInfoViewPresenter(SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;
            
            _sessionInfo.LevelChanged += OnLevelChanged;
            OnLevelChanged();
        }

        private void OnLevelChanged()
        {
            var currentQuest = _sessionInfo.CurrentLevel.Quest;
            _name.Value = currentQuest.Name;
            _description.Value = currentQuest.Description;
            _tip.Value = currentQuest.Tip;
        }
        
        ~QuestInfoViewPresenter()
        {
            _sessionInfo.LevelChanged -= OnLevelChanged;
        }
    }
}