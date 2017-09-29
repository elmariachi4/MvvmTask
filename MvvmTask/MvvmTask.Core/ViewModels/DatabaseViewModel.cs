using Android.Widget;
using MvvmCross.Core.ViewModels;
using MvvmTask.Core.Models;
using SQLite.Net;
using System.Collections.Generic;
using System.Windows.Input;

namespace MvvmTask.Core.ViewModels
{
    public class DatabaseViewModel : BaseViewModel
    {
        private List<SomeEntity> databaseList;
        public List<SomeEntity> DatabaseList
        {
            get
            {
                return databaseList;
            }
            set
            {
                databaseList = value;
                RaisePropertyChanged(() => DatabaseList);
            }
        }
      
       

       
        //SQLite
        private SQLiteConnection conn;
        public void CreateDB(SQLiteConnection _conn)
        {
            conn = _conn;
            conn.CreateTable<SomeEntity>(SQLite.Net.Interop.CreateFlags.AutoIncPK);
        }
        public void AddItemToDb(SomeEntity se)
        {
            conn.Insert(se);
            conn.Commit();
        }
        public void DeleteItemFromDb(SomeEntity se)
        {
            conn.Delete(se.Id);
            conn.Commit();
        }
        public SomeEntity GetItem(int entityID)
        {
            var query = from se in conn.Table<SomeEntity>() where se.Id == entityID select se;
            return query.FirstOrDefault();
        }
        public void UpdateDbItem(SomeEntity se)
        {
            conn.Update(se);
            conn.Commit();
        }
    }
}
