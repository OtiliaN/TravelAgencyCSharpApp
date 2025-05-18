using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using log4net;
using log4net.Config;
using TravelModel.domain;
using TravelNetworking.networking;
using TravelNetworking.networking.jsonprotocol;
using TravelNetworking.networking.protobuffer;
using TravelPersistence.persistence.impl;
using TravelPersistence.persistence.interfaces;
using TravelServices.service;

namespace TravelServer
{
    class StartServer
    {
        private static int DefaultPort = 55556;
        private static String DefaultIp = "127.0.0.1";
        private static readonly ILog logger = LogManager.GetLogger(typeof(StartServer));

        private static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }
        /*
        public class JsonServer : ConcurrentServer
        {
            private readonly IService _server;
            private TravelClientJsonWorker _worker;

            public JsonServer(string host, int port, IService server) : base(host, port)
            {
                this._server = server;
                logger.Info("Starting JSON server...");
            }

            protected override Thread createWorker(TcpClient tcpClient)
            {
                _worker = new TravelClientJsonWorker(_server, tcpClient);
                return new Thread(new ThreadStart(_worker.Run));
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            var log = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(log, new FileInfo("log4net.config"));

            logger.Info("Starting server...");
            logger.Info("Reading properties...");

            int port = DefaultPort;
            String ip = DefaultIp;

            String portT = ConfigurationManager.AppSettings["port"];
            if (portT == null)
            {
                logger.Debug("Port not set in properties file, using default port: " + DefaultPort);
            }
            else
            {
                bool result = Int32.TryParse(portT, out port);
                if (!result)
                {
                    logger.Debug("Port is not a number, using default port: " + DefaultPort);
                    port = DefaultPort;
                }
            }

            String ipT = ConfigurationManager.AppSettings["ip"];
            if (ipT == null)
            {
                logger.Debug("Ip not set in properties file, using default ip: " + DefaultIp);
            }

            logger.InfoFormat("Configuration Settings for database: {0}", GetConnectionStringByName("TravelAgency"));

            IDictionary<String, string> properties = new SortedList<String, String>();
            properties.Add("connectionString", GetConnectionStringByName("travel_agency"));
            
            IFlightRepository flightRepository = new FlightRepositoryImpl(properties);
            IAgentRepository agentRepository = new AgentRepositoryImpl(properties);
            IBookingRepository bookingRepository = new BookingRepositoryImpl(properties);
           
            // Create services
            IService travelServices = new ServiceImpl(agentRepository, flightRepository, bookingRepository);

            logger.DebugFormat("Starting server on {0}:{1}", ip, port);
            JsonServer server = new JsonServer(ip, port, travelServices);
            server.Start();
            logger.Debug("Server started");
            Console.ReadLine();
        }*/
        
         public class ProtobufferServer : ConcurrentServer
        {
            private readonly IService _server;
            private ProtobufferWorker _worker;

            public ProtobufferServer(string host, int port, IService server) : base(host, port)
            {
                this._server = server;
                logger.Info("Starting Protobuffer server...");
            }

            protected override Thread createWorker(TcpClient tcpClient)
            {
                _worker = new ProtobufferWorker(_server, tcpClient);
                return new Thread(new ThreadStart(_worker.Run));
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            var log = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(log, new FileInfo("log4net.config"));

            logger.Info("Starting server...");
            logger.Info("Reading properties...");

            int port = DefaultPort;
            String ip = DefaultIp;

            String portT = ConfigurationManager.AppSettings["port"];
            if (portT == null)
            {
                logger.Debug("Port not set in properties file, using default port: " + DefaultPort);
            }
            else
            {
                bool result = Int32.TryParse(portT, out port);
                if (!result)
                {
                    logger.Debug("Port is not a number, using default port: " + DefaultPort);
                    port = DefaultPort;
                }
            }

            String ipT = ConfigurationManager.AppSettings["ip"];
            if (ipT == null)
            {
                logger.Debug("Ip not set in properties file, using default ip: " + DefaultIp);
            }

            logger.InfoFormat("Configuration Settings for database: {0}", GetConnectionStringByName("TravelAgency"));

            IDictionary<String, string> properties = new SortedList<String, String>();
            properties.Add("connectionString", GetConnectionStringByName("travel_agency"));
            
            IFlightRepository flightRepository = new FlightRepositoryImpl(properties);
            IAgentRepository agentRepository = new AgentRepositoryImpl(properties);
            IBookingRepository bookingRepository = new BookingRepositoryImpl(properties);
           
            // Create services
            IService travelServices = new ServiceImpl(agentRepository, flightRepository, bookingRepository);

            logger.DebugFormat("Starting server on {0}:{1}", ip, port);
            ProtobufferServer server = new ProtobufferServer(ip, port, travelServices);
            server.Start();
            logger.Debug("Server started");
            Console.ReadLine();
        }
    }

}