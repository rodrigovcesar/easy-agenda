using EasyAgenda.Helpers;
using EasyAgenda.Services;
using EasyAgenda.UWP;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(SocialAuthentication))]
namespace EasyAgenda.UWP
{
    public class SocialAuthentication : IAuthenticate
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
        {
            try
            {
                var user = await client.LoginAsync(provider);
                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId;
                return user;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
