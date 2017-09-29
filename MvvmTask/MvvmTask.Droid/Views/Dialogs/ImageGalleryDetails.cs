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
namespace MvvmTask.Droid.Views.Dialogs
{
    class ImageGalleryDetails : DialogFragment
    {
        ImageView image;
        public ImageGalleryDetails(ImageView _image)
        {
            image = _image;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ImageGalleryDetails, container, false);
            Customized.Layout.TouchImageView biggerImage = view.FindViewById<Customized.Layout.TouchImageView>(Resource.Id.BiggerImage);
            var editButton = view.FindViewById<Button>(Resource.Id.imageDetailsEditButton);
            editButton.Click += EditButton_Click;
            if(biggerImage != null)
            {
                biggerImage.SetImageDrawable(image.Drawable);
                biggerImage.VerticalScrollBarEnabled = true;
                biggerImage.HorizontalScrollBarEnabled = true;
            }
            return view;
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            ImageGalleryEdit editFragment = new ImageGalleryEdit(image);
            FragmentManager fm = ((FragmentActivity)Context).SupportFragmentManager;
            editFragment.Show(fm, "");
        }
    }
}