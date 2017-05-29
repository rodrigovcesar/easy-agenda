using EasyAgenda.Helpers;
using EasyAgenda.Services;
using EasyAgenda.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Views;
using Xamarin.Forms;

namespace EasyAgenda.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private AzureService _azureService;
        

        public ICommand LoginCommand { get; }        

   
        public LoginViewModel(INavigation navigation): base(navigation)
        {
            _navigation = navigation;
            _azureService = DependencyService.Get<AzureService>();
            LoginCommand = new Command(async () => await ExecuteLoginCommandAsync());
        }

        private async Task ExecuteLoginCommandAsync()
        {
            if (IsBusy || !(await LoginAsync()))
                return;
            else
            {               
                var mainPage = new MainPage();
                await _navigation.PushAsync(mainPage);
                RemovePageFromStack<LoginPage>();
                IsBusy = false;
            }
        }
        

        public Task<bool> LoginAsync()
        {            
            if (Settings.IsLoggedIn)
                return Task.FromResult(true);

            return _azureService.LoginAsync();
        }
    }
}
