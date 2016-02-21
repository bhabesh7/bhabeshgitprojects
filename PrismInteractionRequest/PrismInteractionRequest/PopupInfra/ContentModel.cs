using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismInteractionRequest.PopupInfra
{
    public class ContentModel: NotificationObject
    {
        private string _contentMessage;

        public string ContentMessage
        {
            get { return _contentMessage; }
            set { _contentMessage = value; RaisePropertyChanged(()=> ContentMessage); }
        }

        private string _confirmationMessage;

        public string ConfirmationMessage        
        {
            get { return _confirmationMessage; }
            set { _confirmationMessage = value; RaisePropertyChanged(() => ConfirmationMessage); }
        }

        public ContentModel()
        {
            ConfirmationMessage = string.Empty;
            ContentMessage = string.Empty;
        }

        public ContentModel(string contentMessage, string confirmationMessage)
        {
            ConfirmationMessage = confirmationMessage;
            ContentMessage = contentMessage;
        }

    }
}
