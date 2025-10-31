using Gameplay.Unit;
using Systems.Mocks;
using UnityEngine;
using Zenject;

namespace Core.DI
{
    public class GameplaySceneContext : MonoInstaller
    {
        [SerializeField] private Unit _mainUnit;
        
        public override void InstallBindings()
        {
            Container.Bind<Unit>().FromInstance(_mainUnit).AsSingle();
            
            Container.BindInterfacesAndSelfTo<TestSceneStarter>().FromNew().AsSingle();
        }
    }
}