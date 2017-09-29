using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmTask.Core.Models;
using Android.Support.Design.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmTask.Core.ViewModels;
using MvvmCross.Droid.Shared.Attributes;
using SQLite.Net;
using Android.Graphics.Drawables;
using Android.Graphics;
using MvvmTask.Droid.Adapters;
using System;
using Android.Support.V7.Widget;
using MvvmTask.Droid.Views.Dialogs;
using Android.App;

namespace MvvmTask.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.DatabaseContent)]
    public class DatabaseView : MvxFragment<DatabaseViewModel>
    {
        View view;
        public static List<SomeEntity> infoList = new List<SomeEntity>();
        
        private Button addButton;
        private RecyclerView dbList;
        private SQLiteConnection dbConnection;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            dbConnection = new SQLiteConnection(new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid(),System.IO.Path.Combine(dbPath, "Database"));
            ViewModel.CreateDB(dbConnection);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.DatabaseView, container, false);
            dbList = view.FindViewById<RecyclerView>(Resource.Id.dbListView);
            addButton = view.FindViewById<Button>(Resource.Id.buttonAdd);
            LinearLayoutManager llm = new LinearLayoutManager(Context);
            dbList.SetLayoutManager(llm);
            dbList.SetAdapter(new DatabaseListAdapter(infoList));
            addButton.Click += AddButton_Click;
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            FillListFromDb();
            dbList.SetAdapter(new DatabaseListAdapter(infoList));
        }
        private void AddButton_Click(object sender, EventArgs e)
        {
            DatabaseAddPopup dialog = new DatabaseAddPopup(infoList);
            bool isClicked = false;
            if (!isClicked)
                dialog.Show(FragmentManager, "Add");
            isClicked = true;
        }
        public void FillListFromDb() //refresh data inside the listview
        {
            infoList.Clear(); //clearing list with data
            var dbQuery = dbConnection.Table<SomeEntity>();
            foreach (var smEnt in dbQuery)
                infoList.Add(smEnt);
            dbList.SetAdapter(new DatabaseListAdapter(infoList));
        }
    }
}