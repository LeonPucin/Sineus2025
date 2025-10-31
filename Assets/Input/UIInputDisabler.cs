using System;
using DoubleDCore.Periphery.Base;
using DoubleDCore.UI.Base;
using Game.Level.Views;
using Game.UI.Pages;
using UnityEngine;
using Zenject;

namespace Game.Input
{
    public class UIInputDisabler : IInitializable, IDisposable
    {
        private readonly IUIManager _uiManager;
        private readonly InputControls _inputControls;

        public UIInputDisabler(IInputServices<InputControls> inputServices, IUIManager uiManager)
        {
            _uiManager = uiManager;
            _inputControls = inputServices.GetInputProvider();
        }

        public void Initialize()
        {
            _uiManager.PageOpened += OnPageOpened;
            _uiManager.PageClosed += OnPageClosed;
        }

        private void OnPageClosed(IPage page)
        {
            if (page is SplashPage)
                return;
            
            _inputControls.Character.Enable();
        }

        private void OnPageOpened(IPage page)
        {
            if (page is MobileControlPage || page is GameMenuPage || page is SplashPage)
                return;
            
            _inputControls.Character.Disable();
        }

        public void Dispose()
        {
            _uiManager.PageOpened -= OnPageOpened;
            _uiManager.PageClosed -= OnPageClosed;
        }
    }
}