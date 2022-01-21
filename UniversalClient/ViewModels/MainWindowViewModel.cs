using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SocketClient.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private int _port;
        private string server;
        private string _message;
        private ObservableCollection<string> _receivedMessages;
        public ICommand SendMessage { set; get; }
        private Task _senderTask;

        public MainWindowViewModel()
        {
            SendMessage = new DelegateCommand(OnSendMessage, OnSendMessageCanExecute);
            ReceivedMessages = new ObservableCollection<string>();

            Port = 11000;
            Server = "localhost";
            Message = "Hallo " + DateTime.Now;
        }

        private void OnSendMessage(object obj)
        {
            _senderTask = Task.Run(() =>
                AsynchronousClient.StartClient(Server, Port, Message));

            ReceivedMessages.Add(Message);
        }

        private bool OnSendMessageCanExecute(object obj)
        {
            if (Port > 1024 && !string.IsNullOrEmpty(Server) && !string.IsNullOrEmpty(Message))
                return true;
            else
                return false;
        }



        public int Port
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
