﻿using Gameplay.Skills;
using Gameplay.UnitCrowd;
using Gameplay.Units;
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
            Container.BindInstance(_mainUnit).AsSingle();
            
            Container.Bind<SpawnPlaceholder>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CrowdPlaceController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SkillUseConfirmator>().FromComponentInHierarchy().AsSingle();

            Container.Bind<UnitSpawner>().FromNew().AsSingle();
            Container.Bind<DifficultyConverter>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<SkillActivator>().AsSingle();
            
            Container.Bind<CrowdMovementsController>().FromNew().AsSingle();
            Container.Bind<CrowdController>().FromNew().AsSingle();
        }
    }
}