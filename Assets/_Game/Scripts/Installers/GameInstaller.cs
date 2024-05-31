using Scripts.Context.Signals;
using Scripts.Managers;
using Scripts.Models;
using Scripts.Signals;
using Zenject;

namespace _Game.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            InstallModels();
            InstallSignals();
            InstallManager();
        }

        private void InstallManager()
        {
        }

        private void InstallModels()
        {
            Container.Bind<GameModel>().AsSingle();
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<StartAppSignal>();
            Container.DeclareSignal<TapSignal>().OptionalSubscriber();
            Container.DeclareSignal<OnStartGameSignal>();
            Container.DeclareSignal<OpenPanelSignal>();
            Container.DeclareSignal<OnScoreChangeSignal>();
            Container.DeclareSignal<StartAppSignal>();
            Container.DeclareSignal<OnGameInitializeSignal>();
            Container.DeclareSignal<OnLevelFinishedSignals>();
            Container.DeclareSignal<ResetGameSignal>();
            Container.DeclareSignal<TapToContinueSignal>();
            
            Container.DeclareSignal<OnStartGameSignal>();
        }
    }
}