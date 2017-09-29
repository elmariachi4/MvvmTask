using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using MvvmCross.Droid.Support.V4;
using MvvmTask.Core.ViewModels;
using System.Collections.Generic;

namespace MvvmTask.Droid.Views
{
    //[Register("mvvmtask.droid.views.mainview")]
    [Android.App.Activity(Label = "Test")]
    public class MainView : MvxCachingFragmentActivity<MainViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);
            var viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.tabLayout);

            if (viewPager != null)
            {
                var fragments = new List<MvxCachingFragmentStatePagerAdapter.FragmentInfo>
                 {
                     new MvxCachingFragmentStatePagerAdapter.FragmentInfo
                         (ViewModel.TabNames[0],
                         typeof(AddressBookView),
                         typeof(AddressBookViewModel)),
                     new MvxCachingFragmentStatePagerAdapter.FragmentInfo
                         (ViewModel.TabNames[1],
                         typeof(ImageGalleryView),
                         typeof(ImageGalleryViewModel)),
                     new MvxCachingFragmentStatePagerAdapter.FragmentInfo(
                         ViewModel.TabNames[2],
                         typeof(DatabaseView),
                         typeof(DatabaseViewModel)),
                     new MvxCachingFragmentStatePagerAdapter.FragmentInfo
                         (ViewModel.TabNames[3],
                         typeof(ImageChangeView),
                         typeof(ImageChangeViewModel))
                 };
                viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(this, SupportFragmentManager, fragments);
                viewPager.OffscreenPageLimit = 4;
            }
            tabLayout.SetupWithViewPager(viewPager);
        }
    }
}
