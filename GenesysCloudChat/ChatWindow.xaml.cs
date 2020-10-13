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

namespace GenesysCloudChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {

        private Chat chat;
        private ChatBot chatBot;
        private string lastSender = String.Empty;
        public Boolean IsBot;
        public ChatWindow(Boolean IsBot)
        {
            InitializeComponent();
            this.IsBot = IsBot;
            
        }

        public void Init(Chat chat, ChatBot chatBot) {
            this.chat = chat;
            this.chatBot = chatBot;
        }

        public void PutMessage(string message) {
            if (!lastSender.Equals("agent"))
            {
                tbConversation.Text += "Agent:\n";
                lastSender = "agent";
            }
            tbConversation.Text += message + "\n";
        }

        private void End_Click(object sender, RoutedEventArgs e)
        {
            chat.EndChat();
        }

        private void SendMessage()
        {
            if (!String.IsNullOrEmpty(tbMessage.Text))
            {
                chat.SendMessage(tbMessage.Text);
                if (!lastSender.Equals("me"))
                {
                    tbConversation.Text += "Me:\n";
                    lastSender = "me";
                }
                tbConversation.Text += tbMessage.Text + "\n";
                tbMessage.Clear();
            }
        }

        public void SendBotHistory(string botHistory) {
            chat.SendMessage(botHistory);
            tbConversation.Text += botHistory + "\n";
        }

        private void SendMessageBot()
        {
            if (!String.IsNullOrEmpty(tbMessage.Text))
            {
               
                if (!lastSender.Equals("me"))
                {
                    tbConversation.Text += "Me:\n";
                    lastSender = "me";
                }
                tbConversation.Text += tbMessage.Text + "\n";
                chatBot.SendMessage(tbMessage.Text);
                tbMessage.Clear();
            }
           
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (!IsBot)
            {
                SendMessage();
            }
            else {
                SendMessageBot();
            }
        }

        private void StopBot_Click(object sender, RoutedEventArgs e)
        {
            IsBot = false;
            chatBot.FinishBot();
        }

        private void TbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SendMessage();  
            }
        }
    }
}
