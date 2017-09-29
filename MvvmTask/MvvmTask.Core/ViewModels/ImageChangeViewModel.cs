
using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace MvvmTask.Core.ViewModels
{
    public class ImageChangeViewModel : BaseViewModel
    {
        public ImageChangeViewModel()
        {

        }
        private MvxCommand<ImageChangeViewModel> _Rotate90Click;
        public virtual ICommand Rotate90Click
        {
            get
            {
                return _Rotate90Click = _Rotate90Click ?? new MvxCommand<ImageChangeViewModel>(
                    vm =>
                    {

                    });
            }
        }

        private MvxCommand<AddressBookViewModel> _FlipVClick;
        public virtual ICommand FlipVClick
        {
            get
            {
                return _FlipVClick = _FlipVClick ?? new MvxCommand<AddressBookViewModel>(
                    vm =>
                    {

                    });
            }
        }

        private MvxCommand<AddressBookViewModel> _FlipHClick;
        public virtual ICommand FlipHClick
        {
            get
            {
                return _FlipHClick = _FlipHClick ?? new MvxCommand<AddressBookViewModel>(
                    vm =>
                    {

                    });
            }
        }
    }
}
