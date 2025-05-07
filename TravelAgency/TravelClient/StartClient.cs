using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using TravelClient.gui;
using TravelNetworking.networking.jsonprotocol;
using TravelServices.service;

namespace TravelClient
{
    public class StartClient
    {
        private static readonly int DEFAULT_PORT = 55556;
        private static readonly string DEFAULT_IP = "127.0.0.1";

        private static readonly ILog logger = LogManager.GetLogger(typeof(StartClient));

        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting client...");
            var log = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(log, new FileInfo("log4net.config"));

            logger.Info("Starting client...");
            logger.Info("Reading configuration properties...");

            int port = DEFAULT_PORT;
            string ip = DEFAULT_IP;

            string portT = ConfigurationManager.AppSettings["port"];
            if (!string.IsNullOrEmpty(portT))
            {
                bool isValidPort = int.TryParse(portT, out port);
                if (!isValidPort)
                {
                    logger.WarnFormat("Port {0} is invalid. Using default port: {1}", portT, DEFAULT_PORT);
                    port = DEFAULT_PORT;
                }
            }
            else
            {
                logger.DebugFormat("Port not set. Using default port: {0}", DEFAULT_PORT);
            }

            string ipT = ConfigurationManager.AppSettings["ip"];
            if (!string.IsNullOrEmpty(ipT))
            {
                ip = ipT;
            }
            else
            {
                logger.DebugFormat("IP not set. Using default IP: {0}", DEFAULT_IP);
            }

            logger.DebugFormat("Starting client on {0}:{1}", ip, port);

            try
            {
                IService server = new ServerJsonProxy(ip, port);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Assuming LoginForm takes the server as a parameter
                var loginForm = new LoginForm(server);
                Application.Run(loginForm);
            }
            catch (Exception ex)
            {
                logger.Error("Error starting client application", ex);
                MessageBox.Show("An error occurred while starting the client application. Please check the logs for more details.", 
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}