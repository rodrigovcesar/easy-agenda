#define OFFLINE_SYNC_ENABLED
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices;
using EasyAgenda.Helpers;
using EasyAgenda.Models;
using Plugin.Connectivity;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EasyAgenda.Services
{
    public class UsuarioManager
    {
        private MobileServiceClient Client { get; }
        private MobileServiceSQLiteStore Store { get; set; } = null;
        private IMobileServiceSyncTable<Usuario> _usuarioSyncTable;

        public UsuarioManager(MobileServiceClient Client)
        {
            this.Client = Client;
            Initialize();
        }

        public async Task Initialize()
        {
            try
            {
                Store = new MobileServiceSQLiteStore(AppSettings.LOCAL_STORAGE);
                Store.DefineTable<Usuario>();
                await Client.SyncContext.InitializeAsync(Store);
                _usuarioSyncTable = Client.GetSyncTable<Usuario>();

            }
            catch (MobileServicePushFailedException ex)
            {
                Errors(ex);
            }
        }


        public async Task<List<Usuario>> GetUsuarioListAsync()
        {
            try
            {
                await this.SyncUsuario();
                var clientList = await _usuarioSyncTable.ToListAsync();
                return clientList;

            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine($"Invalid sync operation: {msioe.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
            }

            return new List<Usuario>();

        }

        private async Task SyncUsuario()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                if (_usuarioSyncTable != null)
                {
                    await Client.SyncContext.PushAsync();
                    await _usuarioSyncTable.PullAsync("allUsuarios", _usuarioSyncTable.CreateQuery());
                }
            }
            catch (MobileServicePushFailedException ex)
            {
                Errors(ex);
            }
        }

        public async Task SaveUsuario(Usuario usuario)
        {
            try
            {
                if (string.IsNullOrEmpty(usuario.Id))
                {

                    await _usuarioSyncTable.InsertAsync(usuario);
                }
                else
                {
                    var usuarioToSave = _usuarioSyncTable.LookupAsync(usuario.Id);
                    if (usuarioToSave != null)
                    {
                        await _usuarioSyncTable.UpdateAsync(usuario);
                    }
                }               
            }
            catch (MobileServicePushFailedException ex)
            {
                Errors(ex);                
            }
        }

        public async Task<bool> DeleteUsuario(Usuario usuario)
        {
            try
            {
                var usuarioToDelete = await _usuarioSyncTable.LookupAsync(usuario.Id);
                if (usuarioToDelete != null)
                {
                    await _usuarioSyncTable.DeleteAsync(usuario);
                    await SyncUsuario();
                    return true;
                }
            }
            catch (MobileServicePushFailedException ex)
            {
                Errors(ex);
            }
            return false;
        }



        private async void Errors(MobileServicePushFailedException exc)
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            if (exc.PushResult != null)
            {
                syncErrors = exc.PushResult.Errors;
            }

            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
    }
}
