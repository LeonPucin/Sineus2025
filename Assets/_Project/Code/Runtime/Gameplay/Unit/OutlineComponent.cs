using System;
using System.Collections.Generic;
using EPOOutline;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

namespace Gameplay.Units
{
    [Serializable]
    public class OutlineComponent
    {
        [SerializeField] private SerializedDictionary<UnitState, OutlineSettings> _statesOutlines = new();
        [SerializeField] private OutlineSettings _highlightOutline;
        [SerializeField] private Outlinable _outlinable;

        private OutlineSettings? _lastOutlineSettings;
        private bool _isHighlighted;
        
        public void ChangeOutline(UnitState newState)
        {
            
        }
        
        public void SetHighlight(bool highlighted, Color color)
        {
            _outlinable.OutlineParameters.Color = color;
            _outlinable.enabled = highlighted;
        }

        private void UseSettings(OutlineSettings settings)
        {
            _outlinable.OutlineParameters.DilateShift = settings.Dilate;
            _outlinable.OutlineParameters.BlurShift = settings.Blur;
            _outlinable.OutlineParameters.Color = settings.Color;
        }
    }

    [Serializable]
    public struct OutlineSettings
    {
        [Range(0, 1)] public float Dilate;
        [Range(0, 1)] public float Blur;
        [ColorUsage(true, true)] public Color Color;
    }
}