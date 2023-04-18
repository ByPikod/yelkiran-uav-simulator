using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;
using TMPro;

public class ServerHolder
{

    private TcpListener tcpListener;
    private Thread tcpListenerThread;
    private TcpClient connectedTcpClient;
    private int port;
    private TcpClient pythonClient;

    public bool failed = false;
    public bool listening = false;
    public bool connected = false;
    public bool dropBall = false;

    public ServerHolder(int port)
    {
        this.port = port;
        tcpListenerThread = new Thread(new ThreadStart(ListenConnections));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start(); ;
    }

    private void ListenConnections()
    {

        try
        {
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), this.port);
            tcpListener.Start();
            Debug.Log("Server is listening");
            listening = true;
            AcceptConnection();
        } catch(System.Exception e)
        {
            Debug.Log("Failed to start listening: " + e.Message);
            this.failed = true;
            return;
        }

    }

    private void AcceptConnection()
    {
        
        while (true)
        {
            try
            {
                pythonClient = tcpListener.AcceptTcpClient();
                Debug.Log("Client accepted!");
                connected = true;
                ListenMessage();
                return;
            }
            catch (System.Exception e)
            {
                Debug.Log("Client failed to connect: " + e.Message);
                return;
            }

        }

    }

    private void ListenMessage()
    {

        while (true)
        {

            try
            {
                byte[] bytes = new byte[1];
                using (NetworkStream ns = pythonClient.GetStream())
                {
                    int length;
                    while ((length = ns.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        dropBall = true;
                        Debug.Log("client message received as: " + bytes[0]);
                    }
                }
            }
            catch
            {
                Debug.Log("Failed to read network stream from client. Connection over.");
                connected = false;
                break;
            }
            
        }
        
        AcceptConnection();

    }

}

public class Server : MonoBehaviour
{

    public const int PORT = 5710;

    ServerHolder serverHolder;

    [SerializeField]
    public GameObject plane;
    [SerializeField]
    public GameObject secondCamera;
    
    [SerializeField]
    public TextMeshProUGUI statusText;
    [SerializeField]
    public Image statusColor;

    [SerializeField]
    public Color warnColor;
    [SerializeField]
    public Color successColor;

    bool serverStatus = false;
    
    // Start is called before the first frame update
    void Start()
    {
        serverHolder = new ServerHolder(PORT);
    }

    // Update is called once per frame
    void Update()
    {

        if (serverHolder.failed)
        {
            statusText.text = "Failed to host simulator server. Please make sure the port 5710 is free.";
            statusText.color = this.warnColor;
        }

        // Hide menu if connection established with python image processor.
        if ( serverStatus != serverHolder.connected )
        {
            
            this.serverStatus = serverHolder.connected;
            secondCamera.SetActive(!serverStatus);
            plane.SetActive(this.serverStatus);
            
            if(serverStatus)
            {
                this.statusText.text = "Connection established!";
                this.statusColor.color = this.successColor;
            }
            else
            {
                this.statusText.text = "Waiting for client...";
                this.statusColor.color = this.warnColor;
            }

        }

        if (this.serverHolder.dropBall)
        {
            this.serverHolder.dropBall = false;
            this.plane.GetComponent<PlaneController>().DropTheBall();
        }

    }

}
