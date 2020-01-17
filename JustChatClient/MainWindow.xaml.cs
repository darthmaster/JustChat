using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNetCore.SignalR.Client;

namespace JustChatClient
{
    public partial class MainWindow : Window
    {
        public ChatViewModel chat = new ChatViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = chat;            
        }

        private async void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                chat.UserName = userName.Text;
                chat.Message = inputBox.Text;
                await chat.SendMessage();
                inputBox.Text = string.Empty;
            }
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await chat.Disconnect();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            await chat.Connect();
        }
    }
}
