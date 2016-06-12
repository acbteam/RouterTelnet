using System;
using System.Configuration;
using System.Net.Sockets;

namespace Coem.Telnet
{
    class Program
    {
        public static void Main(string[] args)
        {
            string Host = ConfigurationManager.AppSettings["Host"];
            string UserName = ConfigurationManager.AppSettings["UserName"];
            string Password = ConfigurationManager.AppSettings["Password"];

            try
            {
                Router _router = new Router(Host, 23);
            
                if (!_router.Connected)
                    _router.ConnectAsync(Host, 23);

                if (_router.Connected)
                {
                    if (_router.Login(UserName, Password))
                    {
                        _router.Restart();
                    }
                }

                _router.Close();
            }

            catch (Exception err)
            {
                Console.WriteLine(err);
                return;
            }
        }

    }
}
