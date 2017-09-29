using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MvvmTask.Droid
{
    public class ImgModel : IParcelable
    {
        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        public IntPtr Handle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ImgModel(string url)
        {
            _url = url;
        }

        protected ImgModel(Parcel incomeParcel)
        {
            _url = incomeParcel.ReadString();
        }
        public static ImgModel[] getSpacePhotos()
        {

            return new ImgModel[]{
                new ImgModel("http://i.imgur.com/zuG2bGQ.jpg"),
                new ImgModel("http://i.imgur.com/ovr0NAF.jpg"),
                new ImgModel("http://i.imgur.com/n6RfJX2.jpg"),
                new ImgModel("http://i.imgur.com/qpr5LR2.jpg"),
                new ImgModel("http://i.imgur.com/pSHXfu5.jpg"),
                new ImgModel("http://i.imgur.com/3wQcZeY.jpg"),
                };
        }

        public int DescribeContents()
        {
            return 0;
        }
        public void WriteToParcel(Parcel parcel, [GeneratedEnum] ParcelableWriteFlags flags)
        {
            parcel.WriteString(_url);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
