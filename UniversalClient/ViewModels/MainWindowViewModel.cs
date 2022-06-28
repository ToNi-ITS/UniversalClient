using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace SocketClient.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private string _port;
        private string server;
        private string _message;
        private ObservableCollection<string> _receivedMessages;
        public ICommand SendMessage { set; get; }
        private Task _senderTask;


        object _lockObject = new object();



        public MainWindowViewModel()
        {
            SendMessage = new DelegateCommand(OnSendMessage, OnSendMessageCanExecute);
            ReceivedMessages = new ObservableCollection<string>();
            AsynchronousClient.StatusMessage += AsynchronousClient_StatusMessage;
            Port = "11000";
            Server = "127.0.0.1";
            Message = "21.87;78.20;1013.00;192.1.23.43";

            BindingOperations.EnableCollectionSynchronization(_receivedMessages, _lockObject);
        }

        private void AsynchronousClient_StatusMessage(object sender, string e)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                lock (_lockObject)
                {
                    ReceivedMessages.Add(e);
                }
            });
        }

        private void OnSendMessage(object obj)
        {
            _senderTask = Task.Run(() =>
                AsynchronousClient.StartClient(Server, int.Parse(Port), Message));


        }

        public bool OnSendMessageCanExecute(object obj)
        {
            if (Port != "" && Server != "")
                return true;
            else
                return false;

        }



        public string Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }
        public string Server
        {
            get => server;
            set
            {
                server = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ReceivedMessages
        {
            get => _receivedMessages;
            set
            {
                _receivedMessages = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }
    }
}
