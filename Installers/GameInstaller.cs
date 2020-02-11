using System;
using UnityEngine;
using Zenject;
using Sheeps.Core;
using Sheeps.Level;
using Sheeps.Save;

namespace Sheeps.Installers {
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        Settings _settings = null;

        public override void InstallBindings() {
            InstallCore();
            InstallLevel();
            InstallAnimals();
            InstallSave();
        }

        void InstallCore() {
            Container.Bind<GameController>().AsSingle();
        }

        void InstallLevel() {
            Container.Bind<Transform>()
                .WithId(TileMap.MapId)
                .FromComponentInNewPrefab(_settings.MapPrefab)
                .WithGameObjectName("Map")
                .AsSingle();

            Container.Bind<ExplosionHandler>().AsSingle();

            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<ExplosionDeadSignal>();
            Container.BindSignal<ExplosionDeadSignal>()
                .ToMethod<ExplosionHandler>(x => x.Despawn).FromResolve();

            Container.BindMemoryPool<Explosion, Explosion.Pool>()
                .FromComponentInNewPrefab(_settings.ExplosionPrefab)
                .WithGameObjectName("Explosion")
                .UnderTransformGroup("Explosions");

            Container.BindInterfacesAndSelfTo<CameraHandler>().AsSingle();            
        }

        void InstallAnimals() {
            /*Container.BindFactory<Food, Food.Factory>()
                .FromComponentInNewPrefab(_settings.FoodPrefab)
                .WithGameObjectName("Food")
                .UnderTransformGroup("FoodList");*/

            // TODO: Factory not working
            Container.Bind<Food>()
                .FromComponentInNewPrefab(_settings.FoodPrefab)
                .WithGameObjectName("Food")
                .UnderTransformGroup("FoodList")
                .AsTransient();

            Container.BindInterfacesAndSelfTo<AnimalManager>().AsSingle();
            Container.Bind<TileMap>().AsSingle();

            Container.BindFactory<float, Animal, Animal.Factory>()
                .FromComponentInNewPrefab(_settings.AnimalPrefab)
                .WithGameObjectName("Animal")
                .UnderTransformGroup("Animals");
        }

        void InstallSave() {
            Container.Bind<SavingSystem>().AsSingle();
        }


        [Serializable]
        public class Settings {
            public GameObject ExplosionPrefab;
            public GameObject AnimalPrefab;
            public GameObject FoodPrefab;
            public GameObject MapPrefab;
        }
    }
}
