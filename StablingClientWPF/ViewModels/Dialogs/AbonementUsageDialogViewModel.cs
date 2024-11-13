using StablingApiClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StablingClientWPF.ViewModels.Dialogs
{
    public class AbonementUsageDialogViewModel : BaseViewModel
    {

        private ObservableCollection<AbonementForShow> _Abonements;
        public ObservableCollection<AbonementForShow> Abonements
        {
            get => _Abonements;
            set { _Abonements = value; OnPropertyChanged(); }
        }

        public AbonementUsageDialogViewModel(IEnumerable<AbonementForShow> abonements)
        {
            Abonements = new ObservableCollection<AbonementForShow>(abonements);
        }

        private AbonementForShow _SelectedAbonement;
        public AbonementForShow SelectedAbonement
        {
            get => _SelectedAbonement;
            set { _SelectedAbonement = value; OnPropertyChanged(); }
        }


    }
}
