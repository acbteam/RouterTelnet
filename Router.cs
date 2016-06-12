using System;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Text;

namespace Coem.Telnet
{
    public class Router : TcpClient
    {
        #region Constructor

        public Router() {}

        public Router(string host, int port)
        {
            base.Connect(host, port);
            this.Host = host;
        }

        #endregion Constructor

        #region Properties

        public string Host {get; set;}

        public NetworkStream Stream { get { return this.GetStream();}}

        #endregion Properties

        #region Method

        public void AsyncConnect(string host, int port)
        {
            AutoResetEvent _ConnectedEvent = new AutoResetEvent(false);

            this.BeginConnect(host, port, new AsyncCallback(delegate (IAsyncResult ar)
            {
                this.EndConnect(ar);
                _ConnectedEvent.Set();
            }), this);

            if (!_ConnectedEvent.WaitOne(2000))
                throw new Exception("Network connection failed!");
        }

        public string Read()
        {
            Byte[] output = new Byte[1024];
            Thread.Sleep(1000);
            Int32 bytes = this.Stream.Read(output, 0, output.Length);
            string response = Encoding.ASCII.GetString(output, 0, bytes);

            Console.WriteLine(response);

            return response;
        }

        public void Write(string sentence)
        {
            var cmd = Encoding.ASCII.GetBytes(sentence + "\r");
            this.Stream.Write(cmd, 0, cmd.Length);
        }

        public bool CheckResponse(string rule)
        {
            string response = Read();

            Regex objToMatch = new Regex(rule);
            return (objToMatch.IsMatch(response));
        }

        public bool Login(string username, string password)
        {
            try
            {
                Write("\n");
                if (!CheckResponse("Login:"))
                    throw new Exception("Host Not Connected");

                Write(username);
                if (!CheckResponse("Password:"))
                    throw new Exception("Host Not Connected");

                Write(password);
                if (CheckResponse("Login incorrect"))
                    throw new Exception("Not Valid User");

                return true;
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return false;
            }
        }

        public void Restart()
        {
            try
            {
                string cmd = "sys reboot "; //STANDARD TELNET REBOOT cOMMAND
                Console.Write(cmd);

                Write(cmd);
                Thread.Sleep(10000);

                while (Ping())
                {
                    Ping();
                }
                this.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                this.Close();
            }
        }

        public bool Ping()
        {
            Ping ping = new Ping();
            PingReply pingresult = ping.Send(this.Host);
            bool result = (pingresult.Status == IPStatus.Success);

            if (result)
                Console.WriteLine("Ping " + Host + " : OK");
            else
                Console.WriteLine("Ping " + Host + " : Error");

            return result;
        }

        #endregion
    }
}
