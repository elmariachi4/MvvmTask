using MvvmCross.Platform.IoC;
using MvvmCross.Core.ViewModels;
using MvvmTask.Core.ViewModels;

namespace MvvmTask.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();
            CreatableTypes().EndingWith("Service").AsInterfaces().RegisterAsLazySingleton();
            RegisterAppStart<MainViewModel>();
        }
    }
}
