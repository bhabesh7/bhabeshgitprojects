using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.ViewModel;
using PrismInteractionRequest.PopupInfra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

//Learnt from :
//http://blog.magnusmontin.net/2013/04/20/implement-a-confirmation-dialog-in-wpf-with-mvvm-and-prism/


namespace PrismInteractionRequest
{
    public class MainViewModel : NotificationObject
    {
        private InteractionRequest<Confirmation> _launchPopupRequest;
        public InteractionRequest<Confirmation> LaunchPopupRequest
        {
            get
            {
                return _launchPopupRequest;
            }
            set
            {
                _launchPopupRequest = value;
                RaisePropertyChanged(() => LaunchPopupRequest);
            }
            
        }
          


        private string _myMessage;

	public string MyMessage
	{
		get { return _myMessage;}
		set { _myMessage = value;
            RaisePropertyChanged(()=> MyMessage);
        }
	}
	


        private ICommand _launchPopupCommand;

        public ICommand LaunchPopupCommand
        {
            get { return _launchPopupCommand; }
            set
            {
                _launchPopupCommand = value;
                RaisePropertyChanged(() => LaunchPopupCommand);
            }
        }

        public MainViewModel()
        {
            LaunchPopupCommand = new DelegateCommand(ExecuteLaunchPopupCommand, CanExecuteLaunchPopupCommand);
            LaunchPopupRequest = new InteractionRequest<Confirmation>();

            
        }

        private bool CanExecuteLaunchPopupCommand()
        {
            return true;
        }

        private void ExecuteLaunchPopupCommand()
        {

            var confirmObject  = CreateConfirmationObject();

            LaunchPopupRequest.Raised += LaunchPopupRequest_Raised;
            LaunchPopupRequest.Raise(confirmObject,OnPopupClosed);
               
        }

        private void OnPopupClosed(Confirmation conf)
        {
            if (conf.Confirmed)
            {
                MyMessage = ((conf.Content) as ContentModel).ContentMessage;
            }
        }

        void LaunchPopupRequest_Raised(object sender, InteractionRequestedEventArgs e)
        {
            OnPopupClosed(e.Context as Confirmation);
        }

        private Confirmation CreateConfirmationObject()
        {
 	        return new Confirmation{ Confirmed = false, 
                Content = new ContentModel{ ConfirmationMessage = "Do you want to Save Changes ?", ContentMessage=MyMessage}, 
                Title="Confirm Save"};
        }



    }
}
