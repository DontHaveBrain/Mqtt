using System.Text; 

MqttServer.MqttServer mqttServer = new MqttServer.MqttServer (11520);
mqttServer.ReceiveMsg += ReceiveMsg; 

Console.ReadKey();
 void ReceiveMsg(string topic, byte[] ReceiveData)
{
    mqttServer.SendMessage(topic, Encoding.UTF8.GetBytes("Client are you ok?"));
}
 


 