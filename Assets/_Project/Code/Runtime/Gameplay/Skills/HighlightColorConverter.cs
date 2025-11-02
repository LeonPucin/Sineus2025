using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Skills
{
    public class HighlightColorConverter
    {
        private readonly ColorCalculator _colorCalculator;

        public HighlightColorConverter()
        {
            _colorCalculator = new ColorCalculator();
        }
        
        public Color GetHighlightColor(Unit target, SkillConfig skill)
        {
            _colorCalculator.SetTarget(target);
            skill.Accept(_colorCalculator);
            
            return _colorCalculator.ResultColor;
        }

        private class ColorCalculator : ISkillVisitor
        {
            private Unit _target;
            
            public Color ResultColor { get; private set; } = Color.red;

            public void SetTarget(Unit target)
            {
                _target = target;
            }
            
            public void Visit(TargetSkillConfig config)
            {
                if (_target.CurrentState == config.TargetState)
                    ResultColor = config.SkillColor;
                else
                    ResultColor = Color.red;
            }

            public void Visit(AreaSkillConfig config)
            {
                ResultColor = config.SkillColor;
            }
        }
    }
}