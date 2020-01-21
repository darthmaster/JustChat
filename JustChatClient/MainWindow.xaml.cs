using System;
using System.Windows;
using System.Windows.Input;

namespace JustChatClient
{
    public partial class MainWindow : Window
    {
        ChatViewModel chat;
        int MaxInputStringLength = 35;
        int MaxChatStringLength = 35;
        public MainWindow()
        {
            InitializeComponent();
            isConnected.IsChecked=false;
        }

        private void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            //if (inputBox.Text.Length - inputBox.Text.LastIndexOf("\n") == MaxInputStringLength)
            //{
            //    inputBox.AppendText(" \n");
            //    inputBox.SelectionStart = inputBox.Text.Length;
            //}
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.Enter)
            {
                inputBox.AppendText("\n");
                inputBox.SelectionStart = inputBox.Text.Length;
            }
            else if (e.Key == Key.Enter)
            {
                sendButton_Click(sender, e);
            }
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await chat.Disconnect();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            //await chat.Connect(serverIP.Text);
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            chat.UserName = userName.Text;
            chat.Message = inputBox.Text.Replace(" \n", " ");
            await chat.SendMessage();
            inputBox.Text = string.Empty;
        }

        private void JCC_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MaxChatStringLength = (int)chatLog.ActualWidth / 7;
            MaxInputStringLength = (int)inputBox.ActualWidth / 7;
            
        }

        public void messageFormat()
        {//7 на пиксель
            int maxLen = MaxChatStringLength - (chat.Messages[-1].User.Length + 1);
            if (chat.Messages[-1].Message.Length > maxLen)
            {
                string new_msg = string.Empty;
                var msg = chat.Messages[-1].Message.Split(" ");
                foreach (string str in msg)
                {
                    if ((new_msg.Length - new_msg.LastIndexOf("\n") + str.Length) <= maxLen)
                    {
                        new_msg += " " + str;
                    }
                    else
                    {
                        new_msg += "\n" + str;
                    }
                }
                chat.Messages[-1].Message = new_msg;
            }

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)isConnected.IsChecked)
            {
                await chat.Disconnect();
                loginButton.Content = "Connect";
            }
            else
            {
                chat = new ChatViewModel(serverIP.Text);
                try
                {
                    await chat.Connect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("jopa");
                }
                finally
                {
                    this.DataContext = chat;
                    loginButton.Content = "Disconnect";
                } 
            }
        }

        private void chatLog_Selected(object sender, RoutedEventArgs e)
        {
            messageFormat();
            
        }
    }
}
