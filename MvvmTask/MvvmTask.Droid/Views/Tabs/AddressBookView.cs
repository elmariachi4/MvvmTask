using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmTask.Core.ViewModels;
using MvvmCross.Droid.Shared.Attributes;
using System;
using Android.Content;
using MvvmTask.Core.Models;
using Android.Provider;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading;
using MvvmTask.Droid.Adapters;
using Android.Support.V7.Widget;

namespace MvvmTask.Droid.Views
{
    [MvxFragment(typeof(MainViewModel),Resource.Id.AddressBookContent)]
    public class AddressBookView : MvxFragment<AddressBookViewModel>
    {
        View view;
        RecyclerView addressListView;
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.AddressBookView, container, false);
            var llProgress = view.FindViewById<Android.Widget.LinearLayout>(Resource.Id.llHeaderProgress);
            addressListView = view.FindViewById<RecyclerView>(Resource.Id.ContactListView);
            LinearLayoutManager llm = new LinearLayoutManager(Context);
            llProgress.Visibility = ViewStates.Visible;
            var searchBox = view.FindViewById<SearchView>(Resource.Id.searchContactsView);
            searchBox.QueryTextChange += SearchBox_QueryTextChange;
            addressListView.SetLayoutManager(llm);
            FastScroller fastScroller = view.FindViewById<FastScroller>(Resource.Id.fastscroller);
            fastScroller.SetRecyclerView(addressListView);
            new Thread(new ThreadStart(() =>
            {
                ViewModel.ContactList = GetContactList();
                Activity.RunOnUiThread(() =>
                {
                    addressListView.SetAdapter(new AddressBookAdapter(ViewModel.ContactList));
                    llProgress.Visibility = ViewStates.Gone;
                });
            })).Start();
            return view;
        }
        
        private void SearchBox_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewText))
            {
                var newContactList = new ObservableCollection<Contact>
                   (from cont in ViewModel.ContactList where cont.Name.ToLower().Contains(e.NewText.ToLower()) select cont);
                addressListView.SetAdapter(new AddressBookAdapter(newContactList));
            }
            else
                addressListView.SetAdapter(new AddressBookAdapter(ViewModel.ContactList) {  });
        }

        public ObservableCollection<Contact> GetContactList()
        {
            var contactList = ViewModel.ContactList;
            contactList = new ObservableCollection<Contact>();
            ContentResolver cr = Context.ContentResolver;
            var cur = cr.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null, null);
            if (cur.Count > 0)
            {
                while (cur.MoveToNext())
                {
                    string id = cur.GetString(cur.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId));
                    string name = cur.GetString(cur.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
                    string phone = cur.GetString(cur.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                    string email = "";
                    cr.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null, null); //query to get the phones
                    var emailCur = cr.Query(ContactsContract.CommonDataKinds.Email.ContentUri,
                                            null,
                                            ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + " = " + id.ToString(),
                                            null,
                                            null); // query to get contact's emails
                    while (emailCur.MoveToNext())
                    {
                        email = emailCur.GetString(emailCur.GetColumnIndex(ContactsContract.CommonDataKinds.Email.Address));
                    }

                    if (contactList.ToList().Exists(v => v.Name == name))
                    {
                        var match = contactList.ToList().Find(o => o.Name == name);
                        match.Phone += ";" + phone;
                        match.Email += ";" + email;
                    }
                    else
                        contactList.Add(new Contact(Convert.ToInt32(id), name, phone, email));
                }
            }
            return new ObservableCollection<Contact>(contactList.OrderBy(i => i.Name)); //setting the list to alphabetical order
        }

        public struct ContactActions
        {
            public string Action;
            public string Value;
        }
    }
}