
using MvvmCross.Core.ViewModels;
using MvvmTask.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using Android.Widget;

namespace MvvmTask.Core.ViewModels
{
    
    public class AddressBookViewModel : BaseViewModel
    {
        private Contact _selectedItem;
        private ObservableCollection<Contact> _contactList;
        public static ObservableCollection<Contact> ContList;
        public ObservableCollection<Contact> ContactList
        {
            get { return _contactList; }
            set
            {
                _contactList = value;
                ContList = value;
                RaisePropertyChanged(() => ContactList);
            }
        }
    }
}
