
using System.Text;

MqttClient.MqttClient mqttClient = new MqttClient.MqttClient("A", "127.0.0.1", 11520, "admin", "admin123");
MqttClient.MqttClient mqttClientB = new MqttClient.MqttClient("B", "127.0.0.1", 11520, "admin", "admin123");


while (true)
{
    await Task.Delay(10);
    mqttClient.SendMessage(  Encoding.UTF8.GetBytes("Server are you ok?"));
    mqttClientB.SendMessage(Encoding.UTF8.GetBytes("Server are you ok?"));
}
 

Console.ReadKey();
