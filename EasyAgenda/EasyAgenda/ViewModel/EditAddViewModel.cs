using EasyAgenda.Models;
using EasyAgenda.Services;
using EasyAgenda.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EasyAgenda.ViewModel
{
    public class EditAddViewModel: BaseViewModel
    {
        public Command SalvarCommand { get; private set; }
        private Usuario Usuario { get; set; }
        private AzureService _azureService;
        private UsuarioManager _manager;

        private string _nome;

        public string Nome
        {
            get { return _nome; }
            set {
                if (SetProperty(ref _nome, value))
                    SalvarCommand.ChangeCanExecute();
            }
        }
       
        private string _telefone;

        public string Telefone
        {
            get { return _telefone; }
            set {
                if(SetProperty(ref _telefone, value))
                    SalvarCommand.ChangeCanExecute();
            }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set {
                if (SetProperty(ref _email, value))
                    SalvarCommand.ChangeCanExecute();
            }
        }

        public EditAddViewModel(INavigation nav) : base(nav)
        {
            Initialize();
            Usuario = new Usuario();         
        }

        public EditAddViewModel(Usuario usuario, INavigation nav): base(nav)
        {            
            Initialize();
            Usuario = usuario;
        }

        private void Initialize()
        {
            SalvarCommand = new Command(ExecuteSaveCommand, CanExecuteSaveCommand);
            _azureService = DependencyService.Get<AzureService>();
            _manager = _azureService.Manager;
        }

        private async void ExecuteSaveCommand()
        {
            Usuario.Nome = Nome;
            Usuario.Email = Email;
            Usuario.Telefone = Telefone;
            await _manager.SaveUsuario(Usuario);
            RemovePageFromStack<EditAddPage>();
        }

        private bool CanExecuteSaveCommand()
        {
            return !string.IsNullOrWhiteSpace(Nome) &&
                !string.IsNullOrWhiteSpace(Telefone) &&
                !string.IsNullOrWhiteSpace(Email);
        }
    }
}
