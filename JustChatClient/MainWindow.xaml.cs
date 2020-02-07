using System;
using System.Windows;
using System.Windows.Input;
using AdonisUI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustChatClient
{
    public partial class MainWindow : Window
    {
        ChatViewModel chat;
        public MainWindow()
        {
            InitializeComponent();
            isConnected.IsChecked = false;
            inputBox.IsEnabled = false;
            sendButton.IsEnabled = false;
            AdonisUI.ResourceLocator.SetColorScheme(Application.Current.Resources, ResourceLocator.DarkColorScheme);
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
        [Authorize]
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)isConnected.IsChecked)
            {
                chat.Disconnect();
                inputBox.IsEnabled = false;
                sendButton.IsEnabled = false;
                loginButton.Content = "Connect";
            }
            else
            {
                chat = new ChatViewModel(serverIP.Text);
                await chat.Connect();
                chat.Messages.CollectionChanged += Messages_CollectionChanged;
                this.DataContext = chat;
                if (chat.IsConnected)
                {
                    inputBox.IsEnabled = true;
                    sendButton.IsEnabled = true;
                    loginButton.Content = "Disconnect";
                }
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
            if ((bool)isConnected.IsChecked)
            {
                await chat.Disconnect();
            }
        }

        private void chatLog_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //chatLog.SelectedIndex = -1;
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            chatLog.SelectedItem = chatLog.Items[0];
            chatLog.ScrollIntoView(chatLog.Items[0]);
        }
    }
}
