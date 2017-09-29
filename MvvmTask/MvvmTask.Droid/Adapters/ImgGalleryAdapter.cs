using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Com.Bumptech.Glide;
using MvvmTask.Droid.Views;
using Android.Content;
using static Android.Views.View;
using MvvmTask.Droid.Views.Dialogs;

namespace MvvmTask.Droid.Adapters
{
    public class ImgGalleryAdapter : RecyclerView.Adapter
    {
        string[] items; // array for images list
        public ImgGalleryAdapter(string[] data)
        {
            items = data;
        }
        
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.ListItem_ImageGallery, parent, false);
            var vh = new ImgGalleryViewHolder(itemView);
            return vh;
        }
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];
            var holder = viewHolder as ImgGalleryViewHolder;
            Context itemContext = holder.ItemView.Context;
            ImageView image = holder.imageView;
            ProgressBar pb = holder.progressBar;
            pb.Visibility = ViewStates.Visible;
            Glide.With(itemContext)
                .AsBitmap()
                .Load(ImageGalleryView.Images[position])
                .Into(image);
            pb.Visibility = ViewStates.Gone;
        }

        public override int ItemCount => items.Length;
        
        
    }

    public class ImgGalleryViewHolder : RecyclerView.ViewHolder, IOnClickListener
    {
        public ImageView imageView { get;  set; }
        public ProgressBar progressBar { get;  set; }

        public ImgGalleryViewHolder(View itemView) : base(itemView)
        {
            imageView = itemView.FindViewById<ImageView>(Resource.Id.galleryItemImage);
            progressBar = itemView.FindViewById<ProgressBar>(Resource.Id.galleryItemProgressBar);
            itemView.SetOnClickListener(this);

        }

        public void OnClick(View v)
        {
            ImageGalleryDetails detailFragment = new ImageGalleryDetails(imageView);
            Android.Support.V4.App.FragmentManager fm = ((Android.Support.V4.App.FragmentActivity)v.Context).SupportFragmentManager;
            detailFragment.Show(fm,"");
        }
    }
    
}