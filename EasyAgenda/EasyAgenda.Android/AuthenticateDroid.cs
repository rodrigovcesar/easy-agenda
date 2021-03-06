﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using EasyAgenda.Services;
using EasyAgenda.Helpers;
using EasyAgenda.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AuthenticateDroid))]
namespace EasyAgenda.Droid
{
    public class AuthenticateDroid : IAuthenticate
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
        {
            try
            {
                var user = await client.LoginAsync(Forms.Context, provider);
                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId;
                return user;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}