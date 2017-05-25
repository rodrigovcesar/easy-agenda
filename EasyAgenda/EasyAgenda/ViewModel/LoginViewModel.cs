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
        private INavigation _navigation;

        public ICommand LoginCommand { get; }        

   
        public LoginViewModel(INavigation navigation)
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
                RemovePageFromStack();
                IsBusy = false;
            }
        }

        private void RemovePageFromStack()
        {
            var existingPages = _navigation.NavigationStack.ToList();
            foreach (var page in existingPages)
            {
                if (page.GetType() == typeof(LoginPage))
                    _navigation.RemovePage(page);
            }
        }

        public Task<bool> LoginAsync()
        {
            IsBusy = true;
            if (Settings.IsLoggedIn)
                return Task.FromResult(true);

            return _azureService.LoginAsync();
        }
    }
}
