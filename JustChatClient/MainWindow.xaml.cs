using System;
using System.Windows;
using System.Windows.Input;

namespace JustChatClient
{
    public partial class MainWindow : Window
    {
        ChatViewModel chat;
        //int MaxInputStringLength = 35;
        //int MaxChatStringLength = 35;
        public MainWindow()
        {
            InitializeComponent();
            isConnected.IsChecked = false;
        }

        private void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.Enter)
            {
                inputBox.AppendText("\n");
                inputBox.SelectionStart = inputBox.Text.Length;
            }
            else if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, e);
            }
        }
       
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)isConnected.IsChecked)
            {
                await chat.Disconnect();
                loginButton.Content = "Connect";
            }
            else
            {
                chat = new ChatViewModel(serverIP.Text);
                await chat.Connect();
                this.DataContext = chat;
                if (chat.IsConnected) loginButton.Content = "Disconnect";
            }
        }
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            chat.UserName = userName.Text;
            chat.Message = inputBox.Text.Replace(" \n", " ");
            await chat.SendMessage();
            inputBox.Text = string.Empty;
        }
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await chat.Disconnect();
        }

        //private void JCC_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    MaxChatStringLength = (int)chatLog.ActualWidth / 7;
        //    MaxInputStringLength = (int)inputBox.ActualWidth / 7;

        //}

        //public void messageFormat()
        //{//7 на пиксель
        //    int maxLen = MaxChatStringLength - (chat.Messages[-1].User.Length + 1);
        //    if (chat.Messages[-1].Message.Length > maxLen)
        //    {
        //        string new_msg = string.Empty;
        //        var msg = chat.Messages[-1].Message.Split(" ");
        //        foreach (string str in msg)
        //        {
        //            if ((new_msg.Length - new_msg.LastIndexOf("\n") + str.Length) <= maxLen)
        //            {
        //                new_msg += " " + str;
        //            }
        //            else
        //            {
        //                new_msg += "\n" + str;
        //            }
        //        }
        //        chat.Messages[-1].Message = new_msg;
        //    }

        //}

        //private void chatLog_Selected(object sender, RoutedEventArgs e)
        //{
        //    messageFormat();      
        //}
    }
}
