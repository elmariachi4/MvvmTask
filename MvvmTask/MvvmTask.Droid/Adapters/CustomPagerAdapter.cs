using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Java.Lang;

namespace MvvmTask.Droid.Adapters
{
    public class CustomPagerAdapter : FragmentStatePagerAdapter
        {
            const int PAGE_AMOUNT = 4;
            private string[] tabTitles = { "Address Book", "Images Collection", "Database", "Image Change" };
            readonly Context context;
            
            public CustomPagerAdapter(Context _context, FragmentManager fm) : base(fm)
            {
                context = _context;
            }

            public override int Count
            {
                get
                {
                    return PAGE_AMOUNT; 
                }
            }

            public override Fragment GetItem(int position) => null; 

            public override ICharSequence GetPageTitleFormatted(int position)
                => CharSequence.ArrayFromStringArray(tabTitles)[position];

            public View GetTabView(int position)
            {
                var tv = (TextView)LayoutInflater.From(context).Inflate(Resource.Layout.TabView, null);
                tv.Text = tabTitles[position];
                return tv;
            }
        }
    }
