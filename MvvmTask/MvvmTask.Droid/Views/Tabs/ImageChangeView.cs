using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Net;
using System.Threading;
using MvvmCross.Droid.Support.V4;
using MvvmTask.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using Android.Runtime;
using System;
using Com.Bumptech.Glide;

namespace MvvmTask.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.ImageChangeContent)]
    public class ImageChangeView : MvxFragment<ImageChangeViewModel>
    {
        View view;
        Button buttonRotate, buttonFlipV, buttonFlipH;
        ImageView image;
        Bitmap bmpContainer;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           view = inflater.Inflate(Resource.Layout.ImageChangeView, container, false);

            image = view.FindViewById<ImageView>(Resource.Id.imageForRotation);
            image.BuildDrawingCache(true);    
            new Thread(new ThreadStart(delegate
            {
                Activity.RunOnUiThread(() =>
                {
                    Android.App.ProgressDialog progress = Android.App.ProgressDialog.Show(image.Context, "", "Loading image..", true);
                    bmpContainer = GetPictureFromUrl(string.Format("http://placekitten.com/g/{0}/{1}", image.Width, image.Height));
                    image.SetImageBitmap(bmpContainer);
                    progress.Dismiss();
                });
            })).Start();

            buttonRotate = view.FindViewById<Button>(Resource.Id.buttonRotate);
            buttonFlipH = view.FindViewById<Button>(Resource.Id.buttonFlipH);
            buttonFlipV = view.FindViewById<Button>(Resource.Id.buttonFlipV);

            //actions on rotate button
            buttonRotate.Click += (sender, eArgs) =>
            {
                Matrix matrix = new Matrix();
                image.BuildDrawingCache(true);
                var tempBmp = Bitmap.CreateBitmap(bmpContainer, 0, 0, image.Width, image.Height, matrix, true);
                image.SetImageBitmap(tempBmp);
                image.DestroyDrawingCache();
            };
            buttonFlipH.Click += OnButtonClickEvent;
            buttonFlipV.Click += OnButtonClickEvent;
            return view;
        }
        bool rotationIndicator = false;

        private void OnButtonClickEvent(object sender, EventArgs e)
        {
            Matrix matrix = new Matrix();
            image.BuildDrawingCache(true);
            var button = sender as Button;
            switch(button.Id)
            {
                case Resource.Id.buttonFlipH:
                    matrix.PostScale(-1, 1);
                    break;
                case Resource.Id.buttonFlipV:
                    matrix.PostScale(1, -1);
                    break;
            }
            var tempBmp = Bitmap.CreateBitmap(bmpContainer,0, 0, image.Width, image.Height, matrix, true);
            image.SetImageBitmap(tempBmp);
            image.DestroyDrawingCache();
        }
        
        public Bitmap GetPictureFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            try
            {
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
            }
            catch (WebException)
            {
                Toast.MakeText(Context, "Unable to load image. Reload page to try again.", ToastLength.Long).Show();
            }
            return imageBitmap;
        }
        
    }
}