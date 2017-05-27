using EasyAgenda.ViewModel;
using Xamarin.Forms;

namespace Views
{
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel ViewModel => BindingContext as LoginViewModel;

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation);            
        }
    }
}
