using HslCommunication.MQTT;
using HslCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttClient
{
    public class MqttClient : IMqttable.IMqttable
    {
        string TopicSubcribeTopic = string.Empty;
        HslCommunication.MQTT.MqttClient mqttClient;
        public Action<string, byte[]> ReceiveMsg;
        public MqttClient(string Topic, string ServerIpAddress, int port, string UseName, string PassWord)
        {
            TopicSubcribeTopic = Topic;
            mqttClient = new HslCommunication.MQTT.MqttClient(new MqttConnectionOptions()
            {
                IpAddress = ServerIpAddress,
                Port = port,
                Credentials = new MqttCredential(UseName, PassWord),  // 增加用户名密码确认
                UseRSAProvider = true,  // 通信加密
            });
            mqttClient.OnMqttMessageReceived += MqttClient_OnMqttMessageReceived; ;  // 接收事件
            mqttClient.OnClientConnected += MqttClient_OnClientConnected; //连接时绑定Topic
           
            OperateResult connect = mqttClient.ConnectServer();
            if (!connect.IsSuccess)
            { 
                Console.WriteLine("连接失败: " + connect.Message); 
            }
            else
            {
                Console.WriteLine("连接成功: " + connect.Message);
            }
        }

        private void MqttClient_OnClientConnected(HslCommunication.MQTT.MqttClient client)
        {
            client.SubscribeMessage(TopicSubcribeTopic);
        }

        private void MqttClient_OnMqttMessageReceived(HslCommunication.MQTT.MqttClient client, MqttApplicationMessage message)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "=====>" + "Mqtt客户端接收:" + message.Topic + ":  " + Encoding.UTF8.GetString(message.Payload)); 
            ReceiveMsg?.Invoke(message.Topic,message.Payload);
        }
        public override void SendMessage(byte[] SendData)
        {
            mqttClient.PublishMessage(new MqttApplicationMessage() { Topic = TopicSubcribeTopic, Payload=SendData,QualityOfServiceLevel=MqttQualityOfServiceLevel.OnlyTransfer,Retain=false });
        } 

        public override void SendMessage(string Topic, byte[] SendData)
        {
            mqttClient.PublishMessage(new MqttApplicationMessage() { Topic = TopicSubcribeTopic, Payload = SendData, QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce, Retain = false });
        }
    }
}
