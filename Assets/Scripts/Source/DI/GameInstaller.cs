using Controller;
using Data;
using Source.DataProvider;
using Source.DataProvider.Interfaces;
using Source.Pool;
using Source.Resource;
using UnityEngine;
using View;
using Zenject;

namespace Source.DI
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameWindowView gameWindowView;
        [SerializeField] private LoseWindowView loseWindowView;
        [SerializeField] private ScorePanelView scorePanelView;
        [SerializeField] private UnityPoolObject[] poolViewPrefabs;
        [SerializeField] private Transform poolRoot;
        [SerializeField] private RandomFrequencyDto randomFrequencyDto;

        public override void InstallBindings()
        {
            BindPools();
            Container.Bind<GameController>().AsSingle().NonLazy();
            Container.BindInstance(randomFrequencyDto).AsSingle();
            Container.BindInstance(gameWindowView).AsSingle();
            Container.BindInstance(loseWindowView).AsSingle();
            Container.BindInstance(scorePanelView).AsSingle();
            Container.Bind<ILocalDataProvider>().To<LocalDataProvider>().AsSingle();
            Container.Bind<ILocalDataWriter>().To<LocalDataWriter>().AsSingle();
        }

        private void BindPools()
        {
            Container.Bind<IResourceProvider>().To<Resource.ResourceProvider>().AsSingle();
            Container.BindInstance(poolViewPrefabs);
            Container.Bind<UnityPool.Factory>().AsSingle();
            Container.Bind<UnityPool>().AsSingle().WithArguments(poolRoot);
        }
    }
}