namespace Gameplay.UnitCrowd
{
    public class DifficultyConverter
    {
        private readonly DifficultyConfig _config;

        public DifficultyConverter(DifficultyConfig config)
        {
            _config = config;
        }
        
        public float ConvertToBrokenPart(int difficultyPoints)
        {
            return _config.GetSettings(difficultyPoints).StartBrokenPart;
        }
        
        public int ConvertToCrowdSize(int difficultyPoints)
        {
            return _config.GetSettings(difficultyPoints).CrowdSize;
        }
        
        public bool IsDistributionAvailable(int difficultyPoints)
        {
            return difficultyPoints >= _config.DistributionDifficultyThreshold;
        }
        
        public float GetRageAddition(int difficultyPoints)
        {
            return _config.GetSettings(difficultyPoints).RageAddition;
        }
    }
}