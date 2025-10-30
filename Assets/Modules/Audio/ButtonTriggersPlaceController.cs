using Audio.Triggers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class ButtonTriggersPlaceController : MonoBehaviour
    {
        [SerializeField] private AudioClip _buttonSound;

        [Button]
        private void UpdateAllButtons()
        {
            var buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var button in buttons)
            {
                if (button.TryGetComponent(out ButtonSoundTrigger oldTrigger))
                {
                    oldTrigger.SetClip(_buttonSound);
                    continue;
                }

                var trigger = button.gameObject.AddComponent<ButtonSoundTrigger>();
                trigger.SetClip(_buttonSound);
            }
        }
    }
}