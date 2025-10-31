using Cysharp.Threading.Tasks;
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
#if UNITY_WEBGL
using GamePush;
using GamePush.Save;
using GamePush.Ad;
using GamePush.Donation;
using GamePush.Community;
using GamePush.Localization;
using Yandex.Analytics;
#endif
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

#if UNITY_WEBGL && !UNITY_EDITOR
            await GP_Init.Ready.AsUniTask();
            await GamePushTask.Sync();
#endif
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

#if UNITY_WEBGL && !UNITY_EDITOR
            Container.Bind<IAdvertisingService>().To<GamePushAdService>().AsSingle();
            Container.Bind<IDonationService>().To<GamePushDonationService>().AsSingle();
            Container.Bind<ILeaderboardService>().To<GamePushLeaderboardService>().AsSingle();
            Container.Bind<ILocalizationService>().To<GamePushLocalization>().AsSingle();
            Container.Bind<IMetricService>().To<YandexMetricService>().AsSingle();
#else
            Container.Bind<IAdvertisingService>().To<MockAdvertisingService>().AsSingle();
            Container.Bind<IDonationService>().To<MockDonationService>().AsSingle();
            Container.Bind<ILeaderboardService>().To<MockLeaderboardService>().AsSingle();
            Container.Bind<IMetricService>().To<MockMetricService>().AsSingle();
#endif

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
#if UNITY_WEBGL && !UNITY_EDITOR
            Container.Bind<ISaveController>().To<GamePushSaver>().AsSingle();
#else
            Container.Bind<ISaveController>().To<FileSaver>().AsSingle();
#endif
        }

        private void SetSettings()
        {
#if UNITY_EDITOR
            Application.targetFrameRate = 1000;
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
            Application.targetFrameRate = 60;
#endif
        }
    }
}