using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DoubleDCore.Identification
{
    public abstract class IdentifyingScriptable : ScriptableObject, IIdentifying
    {
        [ReadOnly, SerializeField] private string _id;

        public string ID => _id;

        protected abstract string GetIDPrefix();

#if UNITY_EDITOR
        [Button]
        public void ResetID()
        {
            if (EditorUtility.DisplayDialog("Generate new ID",
                    "Are you sure you want to generate ID?", "Yes", "No") == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(_id) == false && IDHelper.HasDuplicate(_id) == false)
            {
                if (EditorUtility.DisplayDialog("Change unique ID",
                        "Are you sure you want to regenerate ID?", "Yes", "No") == false)
                {
                    return;
                }
            }

            Undo.RecordObject(this, "Change ID");

            _id = IDHelper.GetUniqueID(GetIDPrefix());

            EditorUtility.SetDirty(this);
        }

        public void ResetIDForced()
        {
            Undo.RecordObject(this, "Change ID");
            _id = IDHelper.GetUniqueID(GetIDPrefix());
            EditorUtility.SetDirty(this);
        }
        
        public void RemoveIDForced()
        {
            Undo.RecordObject(this, "Remove ID");
            _id = string.Empty;
            EditorUtility.SetDirty(this);
        }
#endif
        
        public static bool operator ==(IdentifyingScriptable a, IdentifyingScriptable b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null)
                return false;
            
            return a.ID == b.ID;
        }
        
        public static bool operator !=(IdentifyingScriptable a, IdentifyingScriptable b)
        {
            return !(a == b);
        }
    }
}