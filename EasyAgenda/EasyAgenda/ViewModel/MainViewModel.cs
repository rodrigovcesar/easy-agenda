using EasyAgenda.Models;
using EasyAgenda.Services;
using EasyAgenda.ViewModel;
using EasyAgenda.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(MainViewModel))]
namespace EasyAgenda.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private AzureService _azureService;
        private UsuarioManager _usuarioManager;
        
        public Command AdicionarCommand { get; }
        public ObservableCollection<Usuario> Usuarios { get; private set; }        
        public Command LoadListCommand { get; }  
                

        public MainViewModel(INavigation nav): base(nav)
        {
            _azureService = DependencyService.Get<AzureService>();
            _usuarioManager = _azureService.Manager;
            Usuarios = new ObservableCollection<Usuario>();

            AdicionarCommand = new Command(() => 
            App.Current.MainPage.Navigation.PushAsync(new EditAddPage()));

            LoadListCommand = new Command(async () =>
            await ExecuteLoadListCommandAsync()
            );
            LoadListCommand.Execute(null);
                             
        }

        public async Task ExecuteLoadListCommandAsync()
        {
            IsBusy = true;            
            var usuarios = await _usuarioManager.GetUsuarioListAsync();
            System.Diagnostics.Debug.WriteLine("FOUND {0} Usuários", usuarios.LongCount());
            Usuarios.Clear();
            foreach (var usuario in usuarios)
            {
                Usuarios.Add(usuario);
            }
            OnPropertyChanged(nameof(Usuarios));
            IsBusy = false;
        }

        public async Task ApagarUsuarioAsync(Usuario usuario)
        {
            IsBusy = true;
            if (await _usuarioManager.DeleteUsuario(usuario))
            {
                Usuarios.Remove(usuario);
                OnPropertyChanged(nameof(Usuarios));
            }
            IsBusy = false;

        }
        
    }
}
