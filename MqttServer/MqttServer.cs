using HslCommunication;
using HslCommunication.MQTT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MqttServer
{
    public class MqttServer : IMqttable.IMqttable
    {
        private HslCommunication.MQTT.MqttServer mqttServer;
        public Action<string, byte[]> ReceiveMsg;
        public MqttServer(int port) : base()
        {
            mqttServer = new HslCommunication.MQTT.MqttServer();
            mqttServer.ClientVerification += MqttServer_ClientVerification;
            mqttServer.OnClientApplicationMessageReceive += MqttServer_OnClientApplicationMessageReceive;
            mqttServer.ServerStart(port);

            Console.WriteLine($"SocketServer Initial Success ip=0.0.0.0,port={port}");
        }

        /// 客户端信息验证 
        /// <param name="mqttSession"></param>
        /// <param name="clientId"></param>
        /// <param name="userName"></param>
        /// <param name="passwrod"></param>
        /// <returns></returns>
        private static int MqttServer_ClientVerification(MqttSession mqttSession, string clientId, string userName, string passwrod)
        {

            if (userName == "admin" && passwrod == "admin123") return 0;
            return 4;
            // 返回0 表示验证成功，返回非0 表示失败
            // 0: connect success
            // 返回错误码说明 Return error code description
            // 1: unacceptable protocol version
            // 2: identifier rejected
            // 3: server unavailable
            // 4: bad user name or password
            // 5: not authorized
        }

        /// 服务器发送消息 
        /// <param name="topic"></param>
        /// <param name="SendData"></param>
        public override void SendMessage(string topic, byte[] SendData)
        {
            Task.Factory.StartNew(() => { mqttServer.PublishTopicPayload(topic, SendData); });
        }


        public override async void SendMessage(byte[] SendData)
        {
            foreach (var topic in mqttServer.GetAllRetainTopics())
            {
                await Task.Factory.StartNew(() => { mqttServer.PublishTopicPayload(topic, SendData); });
            }

        }
        private void MqttServer_OnClientApplicationMessageReceive(MqttSession session, MqttClientApplicationMessage message)
        {
            Console.WriteLine(message.CreateTime + "=====>" + "Mqtt服务器接收:" + message.Topic + ":  " + Encoding.UTF8.GetString(message.Payload));
            ReceiveMsg?.Invoke(message.Topic, message.Payload); 
        }
    }
}
