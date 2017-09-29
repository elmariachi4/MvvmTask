using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Shared.Attributes;
using MvvmTask.Core.ViewModels;
using Android.Widget;
using MvvmTask.Droid.Adapters;
using Android.Support.V7.Widget;

namespace MvvmTask.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.ImagesCollectionContent)]
    public class ImageGalleryView : MvxFragment<ImageGalleryViewModel>
    {
        RecyclerView recList;
        Button buttonRefresh;
        public static string[] Images;
        System.Random rand = new System.Random();
        public string[] Generate(int amount)
        {
            string[] imagesList = new string[amount];
            for (int i = 0; i < amount; i++)
            {
                imagesList[i] = string.Format("http://placekitten.com/g/{0}/{1}",rand.Next(300,640), rand.Next(350,480));
            }
            return imagesList;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ImageGalleryView, container, false);
            StaggeredGridLayoutManager layoutManager = new StaggeredGridLayoutManager(2, (int)Orientation.Vertical);
            layoutManager.AutoMeasureEnabled = true;
            recList = view.FindViewById<RecyclerView>(Resource.Id.recView);
            recList.HasFixedSize = true;
            recList.SetLayoutManager(layoutManager);
            Images = Generate(50);
            recList.SetAdapter(new ImgGalleryAdapter(Images));
            buttonRefresh = view.FindViewById<Button>(Resource.Id.buttonRefresh);
            buttonRefresh.Click += delegate
            {
                Activity.RunOnUiThread(() =>
                {
                    recList.SetAdapter(new ImgGalleryAdapter(Generate(50)));
                });
            };
            return view;
        }



    }
}