using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using EasyAgenda.Services;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using EasyAgenda.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(SocialAuthentication))]
namespace EasyAgenda.iOS
{
    public class SocialAuthentication : IAuthenticate
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
        {
            try
            {
                var current = GetController();
                var user = await client.LoginAsync(current, provider);
                Helpers.Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Helpers.Settings.UserId = user?.UserId;
                return user;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private UIKit.UIViewController GetController()
        {
            var window = UIKit.UIApplication.SharedApplication.KeyWindow;
            var root = window.RootViewController;
            if (root == null) return null;

            var current = root;
            while (current.PresentedViewController != null)
            {
                current = current.PresentedViewController;
            }
            return current;
        }
    }
}