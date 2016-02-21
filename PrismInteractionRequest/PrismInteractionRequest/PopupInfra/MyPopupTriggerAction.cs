using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace PrismInteractionRequest.PopupInfra
{
    public class MyPopupTriggerAction: TriggerAction<FrameworkElement>
    {
        MyPopupWindow popupWindow;

        protected override void Invoke(object parameter)
        {
            //Raise the Popup Dialog from here

            InteractionRequestedEventArgs irea = parameter as InteractionRequestedEventArgs;

            if(irea ==null)
            {
                return;
            }


            popupWindow = new MyPopupWindow();
            popupWindow.DataContext = irea.Context as Confirmation;
            popupWindow.Closed += popupWindow_Closed;
            popupWindow.ShowDialog();
        }

        void popupWindow_Closed(object sender, EventArgs e)
        {
            popupWindow.Closed -= popupWindow_Closed;
        }
    }
}
