using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;
using PureCloudPlatform.Client.V2.Model;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;

namespace GenesysCloudChat
{
    public class Chat
    {
        private string organizationId = ""; // YOUR ORGANIZATION ID
        private string deploymentId = ""; // YOUR DEPLOYMENT ID
        private string queueName = "Logitravel_Bookings";
        private string conversationId = String.Empty;
        private string memberId = String.Empty;
        private string bearerToken = String.Empty;
        private bool chatStarted = false;
        private ChatWindow chatWindow;
        public bool ChatStarted
        {
            get {
                return chatStarted;
            }
            set {
                chatStarted = value;
            }
        }
        private WebChatApi apiInstance = null;
        public Chat(ChatWindow chatWindow) {
            this.apiInstance = new WebChatApi();
            this.chatWindow = chatWindow;
        }

        public void SendMessage(string message) {
            CreateWebChatMessageRequest msgRequest = new CreateWebChatMessageRequest();
            msgRequest.Body = message;
            msgRequest.BodyType = CreateWebChatMessageRequest.BodyTypeEnum.Standard;
            apiInstance.PostWebchatGuestConversationMemberMessages(conversationId, memberId, msgRequest);
        }

        public void EndChat()
        {
            apiInstance.DeleteWebchatGuestConversationMember(conversationId, memberId);

        }

        private void ProcessMessage(Dictionary<string, object> notification)
        {
            object eventBody, bodyType, body, sendr;
            if (notification.TryGetValue("eventBody", out eventBody))
            {
                Dictionary<string, object> eBody = ((JObject)eventBody).ToObject<Dictionary<string, object>>();
                if (eBody.TryGetValue("bodyType", out bodyType) &&
            eBody.TryGetValue("body", out body) &&
            eBody.TryGetValue("sender", out sendr))
                {
                    Dictionary<string, object> senderOb = ((JObject)sendr).ToObject<Dictionary<string, object>>();
                    object senderId;

                    if (bodyType.ToString().Equals("standard") &&
                    senderOb.TryGetValue("id", out senderId) && !senderId.ToString().Equals(memberId))
                    {
                        Application.Current.Dispatcher.Invoke(() => {
                            chatWindow.PutMessage(body.ToString());
                        });

                    }
                }
            }
        }

        public void CreateChat() {
            PureCloudRegionHosts region = PureCloudRegionHosts.eu_west_1;
            Configuration.Default.ApiClient.setBasePath(region);
            CreateWebChatConversationRequest request = new CreateWebChatConversationRequest()
            {
                OrganizationId = this.organizationId,
                DeploymentId = this.deploymentId,
                RoutingTarget = new WebChatRoutingTarget() {
                    TargetType = WebChatRoutingTarget.TargetTypeEnum.Queue,
                    TargetAddress = this.queueName
                },
                MemberInfo = new GuestMemberInfo()
                {
                    DisplayName = "Test Name",
                    CustomFields = new Dictionary<string, string>()
                }
            };

            request.MemberInfo.CustomFields.Add("firstName", "Javier");
            request.MemberInfo.CustomFields.Add("lastName", "Arguello");
            request.MemberInfo.CustomFields.Add("phoneNumber", "637986292");
           
            try
            {
                // Create an ACD chat conversation from an external customer.
                CreateWebChatConversationResponse result = apiInstance.PostWebchatGuestConversations(request);
                conversationId = result.Id;
                memberId = result.Member.Id;
                Configuration.Default.ApiKey.Add("Authorization", "Bearer " + result.Jwt);
                WebSocket ws = new WebSocket(result.EventStreamUri);
                

                
                    ws.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    ws.OnOpen += (sender, e) => {
                        Console.WriteLine("Connection Opened");
                    };

                    ws.OnMessage += (sender, e) =>
                    {
                        Dictionary<string, object> notification = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Data);
                        object metadata;
                        if (notification.TryGetValue("metadata", out metadata)) {
                            Dictionary<string,object> mdObject = ((JObject)metadata).ToObject<Dictionary<string, object>>();
                            object type;
                            if (mdObject.TryGetValue("type", out type)) {
                                switch (type.ToString()) {
                                    case "message":
                                        ProcessMessage(notification);
                                            break;
                                    case "member-join":
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                            
                  
                        

                    };

                    ws.OnError += (sender, e) => {
                        Console.WriteLine("Error: "+ e.Message);
                    };

                    ws.OnClose += (sender, e) => {
                        Console.WriteLine("Closed ");
                    };
                    ws.Connect();
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception when calling WebChatApi.PostWebchatGuestConversations: " + e.Message);
            }

        }
    }
}
