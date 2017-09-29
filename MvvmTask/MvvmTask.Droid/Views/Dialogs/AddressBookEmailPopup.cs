using System;

using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmTask.Core.Models;
using Android.Support.V4.App;

namespace MvvmTask.Droid.Views.Dialogs
{
    public class AddressBookEmailPopup : DialogFragment
    {
        Contact actionsContact;
        EditText editTitle, editMessage;
        public AddressBookEmailPopup(Contact _actionsContact)
        {
            actionsContact = _actionsContact;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Popup_MailTemplate, container, false);
            editTitle = view.FindViewById<EditText>(Resource.Id.editMailTitle);
            editMessage = view.FindViewById<EditText>(Resource.Id.editMailMessage);
            editTitle.Text = string.Format("Hello, {0}", actionsContact.Name); //template for title
            editMessage.Text = string.Format("Dear {0}, ", actionsContact.Name);// and message of email
            var buttonSend = view.FindViewById<Button>(Resource.Id.buttonSend);
            buttonSend.Click += ButtonSend_Click;   
            view.FindViewById<TextView>(Resource.Id.textViewMailHeader).Text = string.Format("Email to {0}", actionsContact.Name);
            return view;
        }

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            var email = new Intent(Intent.ActionSend);
            email.PutExtra(Intent.ExtraEmail, new string[] { actionsContact.Email })
                    .PutExtra(Intent.ExtraSubject, editTitle.Text)
                    .PutExtra(Intent.ExtraText, editMessage.Text)
                    .SetType("message/rfc822");
            StartActivity(email); //starting the mail app with given data
        }
    }
}