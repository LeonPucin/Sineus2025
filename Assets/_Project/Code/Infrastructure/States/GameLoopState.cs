using DoubleDCore.Automation.Base;
using DoubleDCore.Jurisdiction.Base;
using DoubleDCore.Localization.Base;
using DoubleDCore.Periphery.Base;
using DoubleDCore.Storage.Base;
using Game.Input.Maps;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly BootstrapInfo _bootstrapInfo;
        private readonly ISaveController _saveController;
        private readonly DiContainer _diContainer;
        private readonly IInputService<InputControls> _inputServices;

        [Inject]
        public GameLoopState(BootstrapInfo bootstrapInfo, ISaveController saveController, DiContainer diContainer,
            IInputService<InputControls> inputServices)
        {
            _bootstrapInfo = bootstrapInfo;
            _saveController = saveController;
            _diContainer = diContainer;
            _inputServices = inputServices;
        }

        private LocaleSave _localeSave;
        private IBuild _gameplayBuild;

        public void Enter()
        {
            _localeSave = _diContainer.Instantiate<LocaleSave>();
            _saveController.Subscribe(_localeSave);

            SceneManager.LoadScene(_bootstrapInfo.NextSceneName, LoadSceneMode.Single);
            
            _inputServices.SwitchMap<CharacterMap>();
        }

        public void Exit()
        {
            _saveController.SaveAll();

            _saveController.Unsubscribe(_localeSave.Key);

            _gameplayBuild.Dispose();
            _diContainer.Unbind<IBuild>();
        }
    }
}