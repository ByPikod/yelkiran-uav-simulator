using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ServerHolder
{

    public readonly Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    private const int bufferSize = 1024;
    private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);

    public ServerHolder(int port)
    {
        server.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
        server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
        Listen();
    }

    public void Listen()
    {
        //server.BeginReceiveFrom();
    }

}

public class Server : MonoBehaviour
{

    public const int PORT = 5710;

    ServerHolder serverHolder;
    
    // Start is called before the first frame update
    void Start()
    {
        serverHolder = new ServerHolder(PORT);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
