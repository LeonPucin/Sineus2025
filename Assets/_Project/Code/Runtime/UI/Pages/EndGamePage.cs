using DoubleDCore.Periphery.Base;
using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Pages
{
    public class EndGamePage : MonoPage, IPayloadPage<EndGameInfo>
    {
        [SerializeField] private GameObject _winSplash;
        [SerializeField] private GameObject _loseSplash;
        
        private InputControls _provider;

        [Zenject.Inject]
        private void Init(IInputService<InputControls> inputService)
        {
            _provider = inputService.GetInputProvider();
        }

        public override void Initialize()
        {
            SetCanvasState(false);
        }

        public void Open(EndGameInfo context)
        {
            SetCanvasState(true);
            
            _winSplash.SetActive(context.IsWin);
            _loseSplash.SetActive(!context.IsWin);

            _provider.UI.Esc.performed += OnEscPerformed;
        }

        private void OnEscPerformed(InputAction.CallbackContext obj)
        {
            _provider.UI.Esc.performed -= OnEscPerformed;
            Application.Quit();
        }

        public override void Close()
        {
            SetCanvasState(false);
            _provider.UI.Esc.performed += OnEscPerformed;
        }
    }

    public struct EndGameInfo
    {
        public bool IsWin;
    }
}