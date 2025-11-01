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
            // if (_statesOutlines.ContainsKey(newState))
            // {
            //     _lastOutlineSettings = _statesOutlines[newState];
            //
            //     if (!_isHighlighted)
            //     {
            //         UseSettings(_statesOutlines[newState]);
            //         _outlinable.enabled = true;
            //     }
            // }
            // else
            // {
            //     _lastOutlineSettings = null;
            //     
            //     if (!_isHighlighted)
            //         _outlinable.enabled = false;
            // }
        }
        
        public void SetHighlight(bool highlighted)
        {
            _isHighlighted = highlighted;

            if (_lastOutlineSettings.HasValue)
            {
                if (_isHighlighted)
                    UseSettings(_highlightOutline);
                else
                    UseSettings(_lastOutlineSettings.Value);
            }
            else
            {
                _outlinable.enabled = _isHighlighted;
                
                if (_isHighlighted)
                    UseSettings(_highlightOutline);
            }
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