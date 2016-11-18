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
        static void SendMessage(Message message)
        {
            Console.WriteLine(message.ToString());
        }

        static Message RecieveMessage()
        {
            return Console.ReadLine().ParseMessage();
        }
    }

    public struct Message
    {
        MessageType messageType;
        bool hasData;
        string data;

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
        Print = 0x01, PrintLn = 0x02, Read = 0x03, ReadLine = 0x04, SetColorBG = 0x05, SetColorFG = 0x06, None = 0xFF
    }

    static class StringE
    {
        //TODO: Make this better
        public static Message ParseMessage(this string message)
        {
            Message m = Message.Default;
            string[] msg = message.Split('§');
            string data = (from Match match in Regex.Matches(message, "\"([^\"]*)\"") select match.ToString()).ElementAt(1);

            switch (int.Parse(message))
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
