using System;
using System.Configuration;

namespace RouterUtils
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0].ToUpper())
                {
                    case "R":
                        Restart();
                        break;
                    case "M":
                        MemoryCheck();
                        break;
                    case "P":
                        Ping();
                        break;
                    case "?":
                        Help();
                        break;
                }
            }else
                Help();

        }

        private static void Restart()
        {
            string Host = ConfigurationManager.AppSettings["Host"];
            string UserName = ConfigurationManager.AppSettings["UserName"];
            string Password = ConfigurationManager.AppSettings["Password"];

            try
            {
                Telnet _router = new Telnet(Host, 23);

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

        private static void MemoryCheck()
        {
            string Host = ConfigurationManager.AppSettings["Host"];
            string UserName = ConfigurationManager.AppSettings["UserName"];
            string Password = ConfigurationManager.AppSettings["Password"];

            try
            {
                Telnet _router = new Telnet(Host, 23);

                if (!_router.Connected)
                    _router.ConnectAsync(Host, 23);

                if (_router.Connected)
                {
                    if (_router.Login(UserName, Password))
                    {
                        _router.MemoryCheck();
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

        private static void Ping()
        {
            string Host = ConfigurationManager.AppSettings["Host"];
            string UserName = ConfigurationManager.AppSettings["UserName"];
            string Password = ConfigurationManager.AppSettings["Password"];

            try
            {
                Telnet _router = new Telnet(Host, 23);

                if (!_router.Connected)
                    _router.ConnectAsync(Host, 23);

                if (_router.Connected)
                {
                    if (_router.Login(UserName, Password))
                    {
                        _router.Ping();
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

        private static void Help()
        {
            Console.WriteLine("Debe añadir alguno de los siguientes parámetros para ejecutar la aplicación : ");
            Console.WriteLine("M => Test de Memoria (Reinicio sobre el 50% de uso) ");
            Console.WriteLine("R => Reinicio automático");
            Console.WriteLine("P => Ping Test");
        }

    }
}
