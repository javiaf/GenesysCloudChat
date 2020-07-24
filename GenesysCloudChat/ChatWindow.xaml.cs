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
        private string lastSender = String.Empty;
        public ChatWindow()
        {
            InitializeComponent();
            chat = new Chat(this);
            
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
            chat.CreateChat();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            chat.SendMessage(tbMessage.Text);
            if (!lastSender.Equals("me")) {
                tbConversation.Text += "Me:\n";
                lastSender = "me";
            }
            tbConversation.Text += tbMessage.Text+"\n";
            tbMessage.Clear();

        }
    }
}
