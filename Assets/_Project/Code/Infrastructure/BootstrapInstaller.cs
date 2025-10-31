using DoubleDCore.Ad;
using DoubleDCore.Ad.Base;
using DoubleDCore.Analytics;
using DoubleDCore.Automation;
using DoubleDCore.Automation.Base;
using DoubleDCore.Community;
using DoubleDCore.Fabrics;
using DoubleDCore.Fabrics.Base;
using DoubleDCore.Finder;
using DoubleDCore.Initialization;
using DoubleDCore.Initialization.Base;
using DoubleDCore.Localization;
using DoubleDCore.Localization.Base;
using DoubleDCore.Periphery.Base;
using DoubleDCore.PhysicsTools.Casting.Raycasting;
using DoubleDCore.Tween;
using DoubleDCore.Tween.Base;
using DoubleDCore.UI;
using DoubleDCore.UI.Base;
using DoubleDCore.Donation;
using DoubleDCore.Donation.Base;
using DoubleDCore.Storage;
using DoubleDCore.Storage.Base;
using Game.Input;
using Game.Input.Maps;
using Infrastructure.States;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Infrastructure
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private string _nextSceneIndex = "MainMenu";
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private CoroutineRunner _coroutineRunner;

        public override void InstallBindings()
        {
            RegisterServices();
        }

        public override async void Start()
        {
            base.Start();

            Initialize();
        }

        public void Initialize()
        {
            PostBind();

            var stateMachine = InitializeStateMachine();

            stateMachine.Enter<BootstrapState>();
        }

        private IFullStateMachine InitializeStateMachine()
        {
            var gameStateMachine = Container.Resolve<GameStateMachine>();

            gameStateMachine.BindState(Container.Instantiate<BootstrapState>);
            gameStateMachine.BindState(Container.Instantiate<GameLoopState>);

            return gameStateMachine;
        }

        private void RegisterServices()
        {
            RegisterUtilities();
            RegisterFactories();
            RegisterInputService();
            SetSettings();

            Container.Bind<GameStateMachine>()
                .FromInstance(new GameStateMachine(new StateMachine())).AsSingle().NonLazy();
        }

        private void RegisterUtilities()
        {
            Container.Bind<BootstrapInfo>().FromInstance(new BootstrapInfo(_nextSceneIndex));
            Container.Bind<ICoroutineRunner>().To<CoroutineRunner>().FromInstance(_coroutineRunner).AsSingle();

            Container.Bind<IUIManager>().To<UIManager>().AsSingle();
            Container.Bind<IRayCaster>().To<RayCaster>().AsSingle();
            Container.Bind<IResourcesContainer>().To<ResourcesContainer>().AsSingle();
            Container.Bind<IGameObjectFinder>().To<GameObjectFinder>().AsSingle();
            Container.Bind<ILocalizationService>().To<DefaultLocalizationService>().AsSingle();
            
            Container.Bind<IAdvertisingService>().To<MockAdvertisingService>().AsSingle();
            Container.Bind<IDonationService>().To<MockDonationService>().AsSingle();
            Container.Bind<ILeaderboardService>().To<MockLeaderboardService>().AsSingle();
            Container.Bind<IMetricService>().To<MockMetricService>().AsSingle();

            Container.Bind<IInitializeService>().To<InitializeService>().AsSingle();

            Container.Bind<EventSystemProvider>().FromInstance(new EventSystemProvider(_eventSystem)).AsSingle();
        }

        private void RegisterFactories()
        {
            Container.Bind<IPrefabFabric>().To<PrefabFabric>().AsSingle();
        }

        private void RegisterInputService()
        {
            var inputControls = new InputControls();
            var inputService = new InputService(inputControls);

            inputService.AddMap(new CharacterMap(inputControls));
            inputService.AddMap(new UIMap(inputControls));

            Container.Bind<IInputService<InputControls>>().FromInstance(inputService).AsSingle();
        }

        private void PostBind()
        {
            BindSaveController();
        }

        private void BindSaveController()
        {
            Container.Bind<ISaveController>().To<FileSaver>().AsSingle();
        }

        private void SetSettings()
        {
        }
    }
}