using EasyAgenda.Models;
using Xamarin.Forms;

namespace EasyAgenda.Controls
{
    public class MyListItemCell : ViewCell
    {
        

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            RegisterGestures();
        }

        private void RegisterGestures()
        {
            var deleteOption = new MenuItem()
            {
                Text = "Apagar",
                Icon = "delete.png",
                CommandParameter = ((Usuario)BindingContext),
                Command = new Command(() =>
                {
                    MessagingCenter.Send((Usuario)BindingContext, "UsuarioApagar");
                })

            };
            //deleteOption.Clicked += deleteOption_Clicked;
            ContextActions.Add(deleteOption);   
                            
            

        }
        //private void deleteOption_Clicked(object sender, EventArgs e)
        //{
        //    //To retrieve the parameters (if is more than one, you should use an object, which could be the same ItemModel 
        //    Usuario usuario = (Usuario)((MenuItem)sender).CommandParameter;                   
           
        //}
        
    }
}
