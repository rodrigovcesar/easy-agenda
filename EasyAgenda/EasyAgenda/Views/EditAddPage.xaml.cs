using EasyAgenda.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EasyAgenda.Models;

namespace EasyAgenda.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAddPage : ContentPage
    {        

        private EditAddViewModel ViewModel => BindingContext as EditAddViewModel;
        public EditAddPage()
        {
            InitializeComponent();
            BindingContext = new EditAddViewModel();
        }

        public EditAddPage(Usuario usuario)
        {
            InitializeComponent();
            BindingContext = new EditAddViewModel(usuario);
        }
        
    }
}