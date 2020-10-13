using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesysCloudChat
{
    public class ChatBot
    {
        private List<string> messages;
        private ChatWindow window;
        private Chat chat;
        private string displayName;
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string email;
        private string queue;

        public ChatBot(ChatWindow window, Chat chat) {
            this.window = window;
            this.chat = chat;
            this.messages = new List<string>();
        }


        public void CreateChat(string displayName, string firstName, string lastName, string phoneNumber, string email, string queue)
        {
            this.displayName = displayName;
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.queue = queue;
        }


            public string getTimestamp() {
            return DateTime.Now.ToString("h:mm:ss tt");
        }

        public void SendMessage(string text) {
            messages.Add("[" + getTimestamp() + "] Customer: " + text);
            BotResponse(text);

        }

        public void FinishBot() {
            string chatHistory = "--- BOT HISTORY --- \n";
            foreach (string message in messages) {
                chatHistory += message + "\n";
            }
            chatHistory += "--- END BOT HISTORY ---\n";
            chat.CreateChat(displayName, firstName, lastName, phoneNumber, email, queue);
            chat.SendMessage(chatHistory);
        }

        private void BotResponse(string text) {
            string message = "";
            if (text.ToLower().Contains("hola"))
            {
                message = "Hola, bienvenido al bot de prueba";
            }
            else if (text.ToLower().Contains("que tal") || text.ToLower().Contains("qué tal")) {
                message = "Yo me encuentro genial, espero que usted también.";
            } else{
                message = "Perdona, no le he entendido";
            }
            messages.Add("[" + getTimestamp() + "] Bot: " + message);
            window.PutMessage(message);
        }
    }
}
