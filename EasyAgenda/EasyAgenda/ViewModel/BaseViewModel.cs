using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EasyAgenda.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected INavigation _navigation;

        protected BaseViewModel(INavigation nav)
        {
            _navigation = nav;
        }

        bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            { SetProperty(ref isBusy, value); }
        } 
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void RemovePageFromStack<T>()
        {
            var existingPages = _navigation.NavigationStack.ToList();
            foreach (var page in existingPages)
            {
                if (page.GetType() == typeof(T))
                    _navigation.RemovePage(page);
            }            
        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

    }
}
