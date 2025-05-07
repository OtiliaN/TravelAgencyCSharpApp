using System.Net;
using System.Net.Sockets;
using log4net;

namespace TravelNetworking.networking;

public abstract class AbstractServer
{
    private TcpListener server;
    private String host;
    private int port;
    
    private static readonly ILog logger = LogManager.GetLogger(typeof(AbstractServer));

    public AbstractServer(String host, int port)
    {
        this.host = host;
        this.port = port;
    }

    public void Start()
    {
        IPAddress address = IPAddress.Parse(host);
        IPEndPoint localEndPoint = new IPEndPoint(address, port);
        
        server = new TcpListener(localEndPoint);
        server.Start();

        while (true)
        {
            logger.Debug("Waiting for a connection...");
            TcpClient client = server.AcceptTcpClient();
            logger.Debug("Connected to client");
            processRequest(client);
        }
    }
    
    public abstract void processRequest(TcpClient client);  
}

public abstract class ConcurrentServer : AbstractServer
{
    protected ConcurrentServer (String host, int port) : base(host, port)
    {
    }
    
    public override void processRequest(TcpClient client)
    {
        Thread thread = createWorker(client);
        thread.Start();
    }
    
    protected abstract Thread createWorker(TcpClient client);
}