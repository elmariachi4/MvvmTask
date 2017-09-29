using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using MvvmTask.Core.Models;
using Android.Graphics;

namespace MvvmTask.Droid.Adapters
{
    class DbListAdapter : BaseAdapter
    {

        Activity activity;
        List<SomeEntity> infoList;
        public DbListAdapter(Activity _activity, List<SomeEntity> _list)
        {
            activity = _activity;
            infoList = _list;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return infoList[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var currentEntity = infoList[position]; //get entity for each listview item 

            var view = activity.LayoutInflater.Inflate(Resource.Layout.ListItem_Database, parent, false);

            view.FindViewById<TextView>(Resource.Id.dbItemID).Text = currentEntity.Id.ToString();
            view.FindViewById<TextView>(Resource.Id.dbItemName).Text = currentEntity.Name;
            view.FindViewById<TextView>(Resource.Id.dbItemDescription).Text = currentEntity.Description;
            view.SetBackgroundColor(currentEntity.IsActive ? Color.Argb(150, 90, 238, 90) : Color.Argb(150, 211, 211, 211)); //LightGreen and LightGray colors with mid alpha
            view.FindViewById<TextView>(Resource.Id.dbItemDT).Text = currentEntity.Updated.ToString("ddd, dd.MM.yyyy HH:mm:ss");

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return 0;
            }
        }

    }

    class DbListAdapterViewHolder : Java.Lang.Object
    {
        public TextView Id { get; set; }
        public TextView Name { get; set; }
        public TextView Description { get; set; }
        public TextView Updated { get; set; }
    }
}