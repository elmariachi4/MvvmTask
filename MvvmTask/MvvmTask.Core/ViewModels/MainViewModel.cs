using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
namespace MvvmTask.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            Tab1VM = Mvx.IocConstruct<AddressBookViewModel>();
            Tab2VM = Mvx.IocConstruct<ImageGalleryViewModel>();
            Tab3VM = Mvx.IocConstruct<DatabaseViewModel>();
            Tab4VM = Mvx.IocConstruct<ImageChangeViewModel>();
        }

        public MvxViewModel Tab1VM { get; set; }
        public MvxViewModel Tab2VM { get; set; }
        public MvxViewModel Tab3VM { get; set; }
        public MvxViewModel Tab4VM { get; set; }

        public string[] TabNames = new string[] 
        {
            "Address Book",
            "Images Collection",
            "Database",
            "Image Change"
        };
        
        
    }
}
