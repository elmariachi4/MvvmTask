using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Java.IO;
using Android.Graphics;

namespace MvvmTask.Droid.Views.Dialogs
{
    class ImageGalleryEdit : DialogFragment
    {
        ImageView image, biggerImage;
        Random rand = new Random();
        public ImageGalleryEdit(ImageView _image)
        {
            image = _image;
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ImageGalleryEdit, container, false);
            var biggerImage = view.FindViewById<ImageView>(Resource.Id.editBiggerImage);
            var buttonSave = view.FindViewById<Button>(Resource.Id.buttonGalleryEditSave);
            var buttonRemove = view.FindViewById<Button>(Resource.Id.buttonGalleryEditRemove);
            biggerImage.SetImageDrawable(image.Drawable);
            biggerImage.BuildDrawingCache(true);
            buttonSave.Click += buttonSave_Click;
            buttonRemove.Click += ButtonRemove_Click;
            return view;
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            
        }

        public static void SaveImage(Bitmap bmp)
        {
            try
            {
                using (var os = new System.IO.FileStream(Android.OS.Environment.ExternalStorageDirectory + "/DCIM/Camera/MikeBitMap2.jpg", System.IO.FileMode.CreateNew))
                {
                    bmp.Compress(Bitmap.CompressFormat.Jpeg, 95, os);
                }
            }
            catch (Exception)
            {

            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            var directory = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures),"k");
            if (!directory.Exists())
                directory.Mkdir();
            var filename = "image" + rand.Next(0, 100).ToString()+".jpg";
            var jpgFilename = System.IO.Path.Combine(directory.AbsolutePath,filename);
            
            Bitmap bmp = image.GetDrawingCache(true);
                Save(bmp, jpgFilename);
                Toast.MakeText(Context, "Saved to gallery as " + filename.ToString(), ToastLength.Short).Show();
        }

        void Save(Bitmap bmp, string filename)
        {
            /*Intent mediaScan = new Intent(Intent.ActionMediaScannerScanFile);
            Java.IO.File f = new File(filename);
            mediaScan.SetData(Android.Net.Uri.FromFile(f));
            Activity.SendBroadcast(mediaScan)*/;
            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.OpenOrCreate))
            {
                bmp.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, fs);
            }
        }
    }
}