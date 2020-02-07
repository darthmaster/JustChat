using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace JustChatClient
{
    public class ChatViewModel : INotifyPropertyChanged
    {
       // MainWindow window;
        HubConnection hubConnection;

        public string UserName { get; set; }
        public string Message { get; set; }
        public ObservableCollection<MessageData> Messages { get; }
        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }
        bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    OnPropertyChanged("IsConnected");
                }
            }
        }
        //public Command SendMessageCommand { get; }
        public ChatViewModel(string IP)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://{IP}:5000/chat")
                .WithAutomaticReconnect()
                .Build();
            Messages = new ObservableCollection<MessageData>();
            IsConnected = false;
            IsBusy = false;

            //SendMessageCommand = new Command(async () => await SendMessage(), () => IsConnected);
            hubConnection.Closed += async (error) =>
            {
                  SendLocalMessage(String.Empty, "Подключение закрыто...",DateTime.Now);
                  IsConnected = false;
                  await Task.Delay(5000);
                  await Connect();
            };  
            hubConnection.On<string, string, DateTime>("Receive", (user, message, time) =>
            {
                SendLocalMessage(user, message, time.ToLocalTime());
            });
        }


        public async Task Connect()
        {
            if (IsConnected) { SendLocalMessage("client", "connected=true", DateTime.Now); return; };
            try
            {
                await hubConnection.StartAsync().ConfigureAwait(true);
                SendLocalMessage(string.Empty, "Вы вошли в чат", DateTime.Now);
                IsConnected = true;
            }
            catch (Exception ex)
            {
                SendLocalMessage(string.Empty, $"Не удалось войти в чат по причине: {ex.Message}",DateTime.Now);
            }
        }
        public async Task Disconnect()
        {
            if (!IsConnected) return;
            await hubConnection.StopAsync();
            IsConnected = false;
            SendLocalMessage(string.Empty, "Вы вышли из чата", DateTime.Now);
        }
        public async Task SendMessage()
        {
            try
            {
                IsBusy = true;
                await hubConnection.InvokeAsync("Send", UserName, Message);
            }
            catch(Exception ex)
            {
                SendLocalMessage(string.Empty, $"Сообщение не отправлено потому, что: {ex.Message}",DateTime.Now);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private void SendLocalMessage(string user,string message, DateTime time)
        {
            Messages.Insert(0, new MessageData { Message = message, User = user , Time = $"[{time.ToLongTimeString()}]"});
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
