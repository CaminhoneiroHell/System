using System;
using Zenject;

public class GameInstaller : MonoInstaller {
    public override void InstallBindings()
    {
        Container.Bind<ISoundManager>() //Contract type
            .To<SoundManager>() //Result type
            .AsSingle();

        Container.Bind<IWeapon>() //Contract type
            .WithId("PlayerWeapon")//Identifier 
            .To<Buster>()       //Result type
            .AsSingle();


        //Container.Bind<IUnityService>()
        //    .To<UnityService>()
        //    .AsSingle();

    }
}
