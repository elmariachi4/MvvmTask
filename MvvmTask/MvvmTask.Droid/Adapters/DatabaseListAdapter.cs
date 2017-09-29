using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using MvvmTask.Core.Models;
using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using static Android.Views.View;
using MvvmTask.Droid.Views.Dialogs;
using SQLite.Net;
using Android.OS;
using MvvmTask.Droid.Views;

namespace MvvmTask.Droid.Adapters
{
    public class DatabaseListAdapter : RecyclerView.Adapter
    {
       
        private List<SomeEntity> entityList;
        private Color activeColor = Color.Argb(150, 90, 238, 90);
        private Color nonActiveColor = Color.Argb(150, 211, 211, 211);
        public DatabaseListAdapter(List<SomeEntity> _entityList)
        {
            entityList = _entityList;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = null;
            var id = Resource.Layout.ListItem_Database;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);
            var vh = new DatabaseListAdapterViewHolder(itemView, entityList);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = entityList[position];
            var holder = viewHolder as DatabaseListAdapterViewHolder;
            
            holder.tvId.Text = item.Id.ToString();
            holder.tvName.Text = item.Name;
            holder.tvDescription.Text = item.Description;
            holder.tvUpdated.Text = item.Updated.ToString("ddd, dd.MM.yyyy HH: mm:ss");
            holder.ItemView.SetBackgroundColor(item.IsActive ? activeColor : nonActiveColor);

        }

        public override int ItemCount => entityList.Count;
        
    }

    public class DatabaseListAdapterViewHolder : RecyclerView.ViewHolder, IOnClickListener, IOnLongClickListener
    {
        public TextView tvId { get; set; }
        public TextView tvName { get; set; }
        public TextView tvDescription { get; set; }
        public TextView tvUpdated { get; set; }
        List<SomeEntity> entityList;
        public static SQLiteConnection dbConnection;
        public DatabaseListAdapterViewHolder(View itemView, List<SomeEntity> _list) : base(itemView)
        {
            entityList = _list;
            string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            dbConnection = new SQLiteConnection(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(), System.IO.Path.Combine(dbPath, "Database"));
            itemView.SetOnClickListener(this);
            itemView.SetOnLongClickListener(this);
            tvId = itemView.FindViewById<TextView>(Resource.Id.dbItemID);
            tvName = itemView.FindViewById<TextView>(Resource.Id.dbItemName);
            tvDescription = itemView.FindViewById<TextView>(Resource.Id.dbItemDescription);
            tvUpdated = itemView.FindViewById<TextView>(Resource.Id.dbItemDT);
        }

        public void OnClick(View v)
        {
            //Toast.MakeText(v.Context, "Short" + Position.ToString(), ToastLength.Short).Show();
            var item = entityList[AdapterPosition];
            DatabaseAddPopup dialog = new DatabaseAddPopup(entityList);
            Bundle args = new Bundle();
            args.PutString("itemName", item.Name);
            args.PutString("itemDescription", item.Description);
            args.PutBoolean("itemIsActive",item.IsActive);
            args.PutInt("itemID", item.Id);
            dialog.Arguments = args;
            Android.Support.V4.App.FragmentManager fm = ((Android.Support.V4.App.FragmentActivity)v.Context).SupportFragmentManager;
            dialog.Show(fm,"Edit");
        }
       
        public bool OnLongClick(View v)
        {
            var item = entityList[AdapterPosition];
            AlertDialog.Builder dlg = new AlertDialog.Builder(v.Context);
            AlertDialog alert = dlg.Create();
            dlg.SetMessage("Delete this item?");
            
            dlg.SetPositiveButton("OK", (_sender, e) =>
            {
                dbConnection.Delete(item);
                dbConnection.Commit();
                
                alert.Dismiss();
                Toast.MakeText(v.Context, "Deleted", ToastLength.Short).Show();
               ;
            });
            dlg.SetNegativeButton("Cancel", (_sender, e) => { alert.Dismiss(); });
            dlg.Show();
            return true;
        }
    }
}