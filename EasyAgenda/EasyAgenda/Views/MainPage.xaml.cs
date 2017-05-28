using EasyAgenda.Models;
using EasyAgenda.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyAgenda.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private MainViewModel ViewModel => BindingContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();            
        }

        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    if (ViewModel != null)
        //        await ViewModel.LoadList();
        //}

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Navigation.PushAsync(new EditAddPage(e.SelectedItem as Usuario));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<Usuario>(this, "UsuarioApagar",
                async (usuario) => {
                    await this.ViewModel.ApagarUsuarioAsync(usuario);
                });            
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<Usuario>(this, "UsuarioApagar");
        }
    }
}