using System;

using Android.Views;
using Android.Support.V7.Widget;
using System.Collections.ObjectModel;
using MvvmTask.Core.Models;
using static Android.Views.View;
using MvvmCross.Core.Views;
using Android.Content;
using static MvvmTask.Droid.Views.AddressBookView;
using System.Collections.Generic;
using MvvmTask.Droid.Views;
using MvvmTask.Droid.Views.Dialogs;
using MvvmTask.Core.ViewModels;

namespace MvvmTask.Droid.Adapters
{
    class AddressBookAdapter : BaseRecyclerAdapter
    {
        ObservableCollection<Contact> contactList;
        View itemView;

        public AddressBookAdapter(ObservableCollection<Contact> _contactList)
        {
            contactList = _contactList;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            itemView = null;
            var id = Resource.Layout.ListItem_AddressBook;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);
            var vh = new AddressBookAdapterViewHolder(itemView);
            return vh;
        }
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = contactList[position];
            var holder = viewHolder as AddressBookAdapterViewHolder;
            holder.ContactName.Text = item.Name;
            
        }
        
        public void RefreshList(ObservableCollection<Contact> newList)
        {
            contactList.Clear();
            contactList = newList;
            NotifyDataSetChanged();
        }
        
        public override int ItemCount => contactList.Count;

        public override string GetTextToShowInBubble(int pos)
        {
            return contactList[pos].Name[0].ToString();
        }
        
    }
    

    public class AddressBookAdapterViewHolder : RecyclerView.ViewHolder, IOnClickListener
    {
        public Android.Widget.TextView ContactName { get; set; }
        
        public AddressBookAdapterViewHolder(View itemView) : base(itemView)
        {
            ContactName = itemView.FindViewById<Android.Widget.TextView>(Resource.Id.ContactName);
            itemView.SetOnClickListener(this);
        }
        public void OnClick(View v)
        {
            PopupMenu popup = new PopupMenu(v.Context, v);
            var actionsContact = AddressBookViewModel.ContList[AdapterPosition];
            popup.Inflate(Resource.Menu.ContactOptionsMenu);
            List<ContactActions> actions = RequestContactData(actionsContact);
            foreach (ContactActions act in actions)
                popup.Menu.Add(string.Format("{0} {1}", act.Action, act.Value));
            popup.MenuItemClick += (sender, args) =>
            {
                var selectedItem = new ContactActions();
                for (int i = 0; i < actions.Count; i++)
                    if (args.Item.TitleFormatted.ToString().Contains(actions[i].Value))
                        selectedItem = actions[i];
                if (selectedItem.Action == "Call")
                {
                    var callIntent = new Intent(Intent.ActionCall, Android.Net.Uri.Parse(string.Format("tel:{0}", selectedItem.Value))); //definitely not the best optimization concept, but still works
                    ItemView.Context.StartActivity(callIntent);
                }
                else
                {
                    AddressBookEmailPopup emailPopup = new AddressBookEmailPopup(actionsContact);
                    Android.Support.V4.App.FragmentManager fm = ((Android.Support.V4.App.FragmentActivity)v.Context).SupportFragmentManager;
                    Android.OS.Bundle arguments = new Android.OS.Bundle();
                    arguments.PutString("emailTitle", string.Format("Hello, {0}", actionsContact.Name));
                    arguments.PutString("emailMessage", string.Format("Dear {0}, ", actionsContact.Name));
                    emailPopup.Arguments = arguments;
                    emailPopup.Show(fm,"");
                }
            };
            popup.Show();
        }

        private List<ContactActions> RequestContactData(Contact contact)
        {
            List<ContactActions> actionsList = new List<ContactActions>();
            string[] numbers = contact.Phone.Split(';');
            string[] emails = contact.Email.Split(';');
            foreach (string num in numbers)
                if (num != "")
                    actionsList.Add(new ContactActions { Action = "Call", Value = num });
            foreach (string mail in emails)
                if (mail != "")
                    actionsList.Add(new ContactActions { Action = "Send e-mail to", Value = mail });
            return actionsList;
        }
    }
    
    public abstract class BaseRecyclerAdapter : RecyclerView.Adapter
    {
        public abstract string GetTextToShowInBubble(int pos);
    }
}