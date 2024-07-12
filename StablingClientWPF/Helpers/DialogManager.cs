using StablingClientWPF.ViewModels;

namespace StablingClientWPF.Helpers
{
    public class DialogManager : BaseViewModel
    {
        private bool _IsDialogOpen = false;
        public bool IsDialogOpen
        {
            get { return _IsDialogOpen; }
            set { _IsDialogOpen = value; OnPropertyChanged(); }
        }

        public DelegateCommand OpenDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    OpenDialog();
                });
            }
        }
        public virtual void OpenDialog()
        {
            IsDialogOpen = true;
        }

        public DelegateCommand CloseDialogCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    CloseDialog();
                });
            }
        }
        public void CloseDialog()
        {
            IsDialogOpen = false;
        }
    }
}
