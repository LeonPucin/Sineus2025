using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Input
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class NonUIProcessor : InputProcessor<float>
    {
#if UNITY_EDITOR
        static NonUIProcessor()
        {
            Initialize();
        }
#endif
        
        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            InputSystem.RegisterProcessor<NonUIProcessor>();
        }
        
        public override float Process(float value, InputControl control)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return 0;

            return value;
        }
    }
}