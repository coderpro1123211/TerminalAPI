using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TerminalAPI
{
    public class Terminal
    {
        //TODO: Add API functions here
    }

    class PipeManager
    {
        //TODO: Verify that it is working

        private static Process p;
        private static Process CurrentProcess
        {
            get
            {
                if (p == null)
                {
                    p = Process.GetCurrentProcess();
                }
                return p;
            }
        }
        public static void SendMessage(Message message)
        {
            Console.WriteLine(message.ToString());
        }

        public static Message RecieveMessage()
        {
            return Console.ReadLine().ParseMessage();
        }

        public static Response GetResponse()
        {
            Message response = Console.ReadLine().ParseMessage();
            if (response.messageType == MessageType.Response)
            {
                return new Response(response.data);
            }
            else return new Response(null);
        }
    }

    public struct Response
    {
        //TODO: Implement this

        public bool hasData;
        public string data;

        public static Message Default
        {
            get
            {
                return new Message();
            }
        }

        public Response(string data)
        {
            this.data = data;
            hasData = data == null;
        }
    }

    public struct Message
    {
        public MessageType messageType;
        public bool hasData;
        public string data;

        public static Message Default
        {
            get
            {
                return new Message();
            }
        }

        public Message(MessageType messageType, string data)
        {
            this.messageType = messageType;
            this.data = data;
            hasData = data == null;
        }

        public Message(MessageType messageType)
        {
            this.messageType = messageType;
            data = null;
            hasData = false;
        }

        public override string ToString()
        {
            return (int)messageType + "§" + data;
        }
    }



    public enum MessageType
    {
        Print = 0x01,
        PrintLn = 0x02,
        Read = 0x03,
        ReadResponse = 0x13,
        ReadLine = 0x04,
        ReadLineResponse = 0x14,
        SetColorBG = 0x05,
        SetColorFG = 0x06,
        Acknowledge = 0xAF,
        Error = 0xFA,
        None = 0xFF,
        Response = ReadLineResponse | ReadResponse
    }

    static class StringE
    {
        //TODO: Make this better
        public static Message ParseMessage(this string message)
        {
            Message m = Message.Default;
            string[] msg = message.Split('|');
            //string data = (from Match match in Regex.Matches(message, "\"([^\"]*)\"") select match.ToString()).ElementAt(1);
            string[] d = null;
            try
            {
                msg.CopyTo(d, 1);
            }
            catch
            {
                //Message did not contain any data... Do nothing
            }
            string data = string.Join("|", d);

            switch (int.Parse(msg[0]))
            {
                default:
                    return m;
                case 0x01:
                    return new Message(MessageType.Print, data);
                case 0x02:
                    return new Message(MessageType.PrintLn, data);
                case 0x03:
                    return new Message(MessageType.Read);
                case 0x04:
                    return new Message(MessageType.ReadLine);
                case 0x05:
                    return new Message(MessageType.SetColorBG, data);
                case 0x06:
                    return new Message(MessageType.SetColorFG, data);
            }
        }
    }
}
