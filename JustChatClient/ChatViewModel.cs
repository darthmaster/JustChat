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
        MainWindow window;
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
        public ChatViewModel()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/chat")
                .Build();
            Messages = new ObservableCollection<MessageData>();
            IsConnected = false;
            IsBusy = false;

            //SendMessageCommand = new Command(async () => await SendMessage(), () => IsConnected);
            hubConnection.Closed += async (error) =>
            {
                  SendLocalMessage(String.Empty, "Подключение закрыто...");
                  IsConnected = false;
                  await Task.Delay(5000);
                  await Connect();
            };  
            hubConnection.On<string, string>("Receive", (user, message) =>
            {
                SendLocalMessage(user, message);
            });
        }


        public async Task Connect()
        {
            if (IsConnected) { SendLocalMessage("client", "connected=true"); return; };
            try
            {
                await hubConnection.StartAsync();
                SendLocalMessage(string.Empty, "Вы вошли в чат");
                IsConnected = true;
            }
            catch (Exception ex)
            {
                SendLocalMessage(string.Empty, $"Не удалось войти в чат по причине: {ex.Message}");
            }
        }
        public async Task Disconnect()
        {
            if (!IsConnected) return;
            await hubConnection.StopAsync();
            IsConnected = false;
            SendLocalMessage(string.Empty, "Вы вышли из чата");
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
                SendLocalMessage(string.Empty, $"Сообщение не отправлено потому, что: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private void SendLocalMessage(string user,string message)
        {
            Messages.Insert(0, new MessageData { Message = message, User = user });
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
