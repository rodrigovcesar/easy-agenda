﻿#define OFFLINE_SYNC_ENABLED
using EasyAgenda.Helpers;
using EasyAgenda.Models;
using EasyAgenda.Services;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AzureService))]
namespace EasyAgenda.Services
{
    public class AzureService
    {
        static readonly string AppUrl = AppSettings.AZURE_MOBILE_SERVICE_URL;
        public MobileServiceClient Client { get; set; } = null;
        public static bool UseAuth { get; set; } = false;
        public UsuarioManager Manager { get; set; }

        public AzureService()
        {
            Initialize();
        } 

        public void Initialize()
        {
            Client = new MobileServiceClient(AppUrl);
            if (!string.IsNullOrWhiteSpace(Settings.AuthToken) && !string.IsNullOrWhiteSpace(Settings.UserId))
            {
                Client.CurrentUser = new MobileServiceUser(Settings.UserId)
                {
                    MobileServiceAuthenticationToken = Settings.AuthToken
                };
            }

            Manager = new UsuarioManager(Client);                   
        }

        public async Task<bool> LoginAsync()
        {
            //Initialize();
            try
            {
                var auth = DependencyService.Get<IAuthenticate>();
                var user = await auth.LoginAsync(Client, MobileServiceAuthenticationProvider.Facebook);

                if (user == null)
                {
                    Settings.AuthToken = string.Empty;
                    Settings.UserId = string.Empty;
                    return Erro();
                    
                }
                else
                {
                    Settings.AuthToken = user.MobileServiceAuthenticationToken;
                    Settings.UserId = user.UserId;
                    return true;
                }
            } catch (InvalidOperationException)
            {
                return Erro();
            }
        }

        private bool Erro()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await App.Current.MainPage.DisplayAlert("Ops", "Não conseguimos efetuar o login, tente novamente", "OK");
            });

            return false;
        }
    }
}
