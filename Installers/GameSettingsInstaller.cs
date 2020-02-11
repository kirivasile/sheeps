using UnityEngine;
using Zenject;
using Sheeps.Level;
using Sheeps.GUI;

namespace Sheeps.Installers {
    [CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public Food.Settings Food;
        public TileMap.Settings Map;
        public GameInstaller.Settings GameInstaller;
        public GuiController.Settings GUI;
        public CameraHandler.Settings Camera;

        public override void InstallBindings() {
            Container.BindInstance(Food);
            Container.BindInstance(Map);
            Container.BindInstance(GameInstaller);
            Container.BindInstance(GUI);
            Container.BindInstance(Camera);
        }
    }
}