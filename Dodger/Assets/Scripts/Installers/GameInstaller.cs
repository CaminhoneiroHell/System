using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Player>().AsSingle();
        Container.Bind<IUNityServices>().To<UnityServices>().AsSingle();
    }
}