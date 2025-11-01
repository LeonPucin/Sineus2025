namespace Gameplay.Quests
{
    public interface IQuestVisitor
    {
        void Visit(DifficultyQuestConfig difficultyQuestConfig);
    }
}