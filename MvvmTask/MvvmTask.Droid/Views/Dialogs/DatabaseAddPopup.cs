using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using MvvmTask.Core.Models;
using Android.Support.V4.App;
using SQLite.Net;
using MvvmTask.Droid.Adapters;

namespace MvvmTask.Droid.Views.Dialogs
{
    class DatabaseAddPopup : DialogFragment
    {
        bool isAdd;
        List<SomeEntity> entityList;
        SQLiteConnection dbConnection;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            dbConnection = DatabaseListAdapterViewHolder.dbConnection;
        }
        public DatabaseAddPopup(List<SomeEntity> _list)
        {
            isAdd = Tag == "Add" ? true : false;
            entityList = _list;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Popup_DatabaseAddItem, container, false);

            var actionButton = view.FindViewById<Button>(Resource.Id.dbActionButton);
            var editName = view.FindViewById<TextInputEditText>(Resource.Id.dbAddEditName);
            var editDescription = view.FindViewById<TextInputEditText>(Resource.Id.dbAddEditDescr);
            var isActive = view.FindViewById<Switch>(Resource.Id.dbAddIsActiveSwitch);
            if (!isAdd)
            {
                editName.Text = Arguments.GetString("itemName");
                editDescription.Text = Arguments.GetString("itemDescription");
                isActive.Checked = Arguments.GetBoolean("itemIsActive");
            }
            actionButton.Click += delegate
            {
                actionButton.Enabled = false;
                if (editName.Text.Trim(' ') != "" || editDescription.Text.Trim(' ') != "")
                {
                    SomeEntity se = new SomeEntity()
                    {
                        Name = editName.Text,
                        Description = editDescription.Text,
                        IsActive = isActive.Checked,
                        Updated = DateTime.Now
                    };
                    if (isAdd)
                        dbConnection.Insert(se);
                    else
                    {
                        se.Id = Arguments.GetInt("itemID");
                        dbConnection.Update(se);
                    }
                    dbConnection.Commit();

                    Dismiss();
                    FillListFromDb();
                }
                else
                    Toast.MakeText(Context, "Fill the fields!", ToastLength.Short).Show();
                actionButton.Enabled = true;
            };
            actionButton.Text = isAdd ? "Add" : "Save";
            return view;
        }
        public void FillListFromDb() //refresh data inside the listview
        {
       //     DatabaseView.infoList.Clear(); //clearing list with data
         //   var dbQuery = dbConnection.Table<SomeEntity>();
        //    foreach (var smEnt in dbQuery)
            //    infoList.Add(smEnt);
            //dbList.SetAdapter(new DatabaseListAdapter(infoList));
        }
    }
}