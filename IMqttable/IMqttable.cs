using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMqttable
{
    public abstract class IMqttable
    {
        public IMqttable()
        {
            if (!HslCommunication.Authorization.SetAuthorizationCode("4496337a-969d-44bf-8352-98502281bedb"))
            {
                Console.WriteLine("Initial Fail"); 
            }
            else
            {
                Console.WriteLine("Initial Succrss");
            }

        }
        public Action<string, byte[]> ReceiveMsg;
        public abstract void SendMessage(byte[] SendData);
        public abstract void SendMessage(string Topic, byte[] SendData);
    }
}
