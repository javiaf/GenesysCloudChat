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
using System.Windows.Shapes;

namespace GenesysCloudChat
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ChatFormWindow : Window
    {
        public ChatFormWindow()
        {
            InitializeComponent();
        }

        private bool Check_Fields() {
            if (NameTb.Text.Equals(String.Empty) || LastNameTb.Text.Equals(String.Empty)
                || EmailTb.Text.Equals(String.Empty) || PhoneTb.Text.Equals(String.Empty) || QueueCB.SelectedIndex < 0)
            {
                ErrorLabel.Content = "ERROR: Some of the fields are empty.Fill them before starting Chat.";
                ErrorLabel.Visibility = Visibility.Visible;
                return false;
            }

            ErrorLabel.Content = "";
            ErrorLabel.Visibility = Visibility.Hidden;
            return true;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (Check_Fields()) {
                ChatWindow chatWindow = new ChatWindow(false);
                Chat chat = new Chat(chatWindow);
                chatWindow.Init(chat, null);
                chat.CreateChat(NameTb.Text+" "+LastNameTb.Text, NameTb.Text, LastNameTb.Text, PhoneTb.Text, EmailTb.Text, QueueCB.Text);
                chatWindow.Visibility = Visibility.Visible;
                this.Close();
            }
        }

        private void BotFirst_Click(object sender, RoutedEventArgs e)
        {
            if (Check_Fields())
            {
                ChatWindow chatWindow = new ChatWindow(true);
                Chat chat = new Chat(chatWindow);
                ChatBot chatBot = new ChatBot(chatWindow, chat);
                chatWindow.Init(chat, chatBot);
                chatBot.CreateChat(NameTb.Text + " " + LastNameTb.Text, NameTb.Text, LastNameTb.Text, PhoneTb.Text, EmailTb.Text, QueueCB.Text);
                chatWindow.Visibility = Visibility.Visible;
                this.Close();
            }
        }
    }
}
