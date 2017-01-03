using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TerminalAPI
{
    public abstract class Terminal
    {
        //TODO: Add API functions here

        public static void WriteLine(string str)
        {
            PipeManager.SendMessage(new Message(MessageType.PrintLn, str));
        }

        public static void WriteLine(string format, params object[] args)
        {
            PipeManager.SendMessage(new Message(MessageType.PrintLn, string.Format(format, args)));
        }

        public static ConsoleKey Read()
        {
            PipeManager.SendMessage(new Message(MessageType.Read, "0"));
            Message m = PipeManager.RecieveMessage();
            //System.Windows.Forms.MessageBox.Show(m.data);
            return (ConsoleKey)Enum.Parse(typeof(ConsoleKey), m.data);
        }

        public static ConsoleKey Read(bool showKey)
        {
            PipeManager.SendMessage(new Message(MessageType.Read, !showKey ? "1" : "0"));
            Message m = PipeManager.RecieveMessage();
            //System.Windows.Forms.MessageBox.Show(m.data);
            return (ConsoleKey)Enum.Parse(typeof(ConsoleKey), m.data);
        }

        public static void Clear()
        {
            PipeManager.SendMessage(new Message(MessageType.Clear));
        }
    }

    public class PipeManager
    {
        public static void SendMessage(Message message, Process to)
        {
            to.StandardInput.WriteLine(message.ToString());
        }

        public static void SendMessage(Message message)
        {
            Console.WriteLine(message.ToString());
        }

        public static Message RecieveMessage()
        {
            return Console.ReadLine().ParseMessage();
        }

        public static Message RecieveMessage(Process from)
        {
            return from.StandardOutput.ReadLine().ParseMessage();
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
        public bool hasData;
        public string data;

        public static Response None
        {
            get
            {
                return new Response();
            }
        }

        public Response(string data)
        {
            this.data = data;
            if (string.IsNullOrWhiteSpace(data) || string.IsNullOrEmpty(data))
            {
                hasData = false;
            }
            else hasData = true;
        }
    }

    public struct Message
    {
        public MessageType messageType;
        public bool hasData;
        public string data;

        public static Message None
        {
            get
            {
                return new Message(MessageType.None, null);
            }
        }

        public Message(MessageType messageType, string data)
        {
            this.messageType = messageType;
            this.data = data;
            if (string.IsNullOrWhiteSpace(data) || string.IsNullOrEmpty(data))
            {
                hasData = false;
            }
            else hasData = true;
        }

        public Message(MessageType messageType)
        {
            this.messageType = messageType;
            data = null;
            hasData = false;
        }

        public override string ToString()
        {
            return (int)messageType + (hasData || data != null ? ("|" + data) : "");
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
        Clear = 0x07,
        Acknowledge = 0xAF,
        Error = 0xFA,
        None = 0xFF,
        Response = ReadLineResponse | ReadResponse,
        NoResponse = 0xAA
    }

    static class StringE
    {
        public static Message ParseMessage(this string message)
        {
            //TODO: Implement all the added commands here

            string[] msg = message.Split(new char[] { '|' }, 2);

            string data = msg.LastOrDefault();

            return new Message((MessageType)int.Parse(msg[0]), data);
        }
    }
}
