using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;

using System.IO;
//using UnityEditorInternal;

public class StateObject
{
    public Socket workSocket = null;

    public const int BufferSize = 1024;

    public byte[] buffer = new byte[BufferSize];

    public StringBuilder sb = new StringBuilder();
}

public class AsyncClient : MonoBehaviour
{
    public static AsyncClient instance;

    public Socket socket;

    private string ipAdress = "125.177.191.32";
    //private string ipAdress = "127.0.0.1";
    //private string ipAdress = "192.168.219.1";
    //private string ipAdress = "192.168.43.35";
    //private string ipAdress = "192.168.219.102";

    //private int port = 31400;
    private int port = 9000;

    private IPEndPoint ipep;

    //public ManualResetEvent connectDone;
    //public ManualResetEvent sendDone;
    //public ManualResetEvent receiveDone;

    string response;

    private JsonMgr jsonmgr = new JsonMgr();
    string datapath;

    public byte[] sendBuffer = new byte[512];

    private void Start()
    {
        instance = this;

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            NoDelay = true
        };

        IPAddress ipAddr = IPAddress.Parse(ipAdress);

        ipep = new IPEndPoint(ipAddr, port);

        Connect(ipep, socket);

        //datapath = Application.dataPath + "/Resources/Json";
        //datapath = Application.streamingAssetsPath + "/Json";
        datapath = Application.persistentDataPath;
    }

    private void Update()
    {
        if (PlayerController.bIsOnline && DisPlayerController.bIsOnline)
            return;

        // Same Move Key
        //if (Input.GetKeyUp(KeyCode.Z))
        //{
        //    Send(socket, sendBuffer);
        //}

        //if (Input.GetKeyUp(KeyCode.X))
        //{
        //    Receive(socket);
        //}
    }

    public void Connect(IPEndPoint ipEP, Socket client)
    {
        client.BeginConnect(ipEP, new AsyncCallback(ConnectCallback), client);
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;

            client.EndConnect(ar);

            //connectDone.Set();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public void Send(Socket client, byte[] datas)
    {
        JsonClass json = new JsonClass(this.gameObject.transform, State.Idle, Vector3.zero);
        string jsonData = jsonmgr.ObjectToJson(json);

        // 해더를 추가한 만큼 데이터를 보내야 하기때문에 아래쪽에서 연산한 temp.nSize 만큼 크기를 보내주기로 변경.
        //int i = Encoding.Default.GetByteCount(jsonData);

        //byte[] data = Encoding.UTF8.GetBytes(jsonData);
        string temp = "User";
        byte[] data = Encoding.UTF8.GetBytes(temp);

        byte[] buffer = new byte[8 + data.Length];

        byte[] Header = StructToByte(new PACKET_HEADER(Constant.REQ_IN, buffer.Length));
        //byte[] Header = StructToByte(new PACKET_HEADER(Constant.REQ_ROOM, buffer.Length));

        Array.Copy(Header, 0, buffer, 0, Header.Length);
        Array.Copy(data, 0, buffer, Header.Length, data.Length);
        
        
        client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None,
            new AsyncCallback(SendCallback), client);
    }

    public void Send(byte[] _inputData, int _inputID)
    {
        byte[] Buffer = new byte[8 + _inputData.Length];

        byte[] Header = StructToByte(new PACKET_HEADER(_inputID, Buffer.Length));

        Array.Copy(Header, 0, Buffer, 0, Header.Length);
        Array.Copy(_inputData, 0, Buffer, Header.Length, _inputData.Length);

        socket.BeginSend(Buffer, 0, Buffer.Length, SocketFlags.None,
            new AsyncCallback(SendCallback), socket);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;

            int bytesSend = client.EndSend(ar);

            //Console.WriteLine("Send {0} bytes to server.", bytesSend);
            //Debug.Log("Send " + bytesSend + " bytes to server.");

            //sendDone.Set();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public void Receive(Socket client)
    {
        try
        {
            StateObject state = new StateObject
            {
                workSocket = client
            };

            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
             new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            StateObject state = (StateObject)ar.AsyncState;

            Socket client = state.workSocket;

            int bytesRead = client.EndReceive(ar);

            if(bytesRead > 0)
            {
                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

                // 받은 Packet을 분류하기 위한 함수로 이동
                ProcessPacket(state.buffer);

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
                if(state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                }

                //receiveDone.Set();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    /// <summary>
    /// 받은 Packet을 분류하는 함수
    /// </summary>
    /// <param name="_inputData">받은 Data</param>
    private void ProcessPacket(byte[] _inputData)
    {
        PACKET_HEADER tempPH = new PACKET_HEADER();
        byte[] tempByte = new byte[1024];

        Array.Copy(_inputData, 0, tempByte, 0, Marshal.SizeOf<PACKET_HEADER>());
        tempPH = ByteToStruct<PACKET_HEADER>(tempByte, Marshal.SizeOf<PACKET_HEADER>());
        Array.Clear(tempByte, 0, tempByte.Length);

        switch (tempPH.nID)
        {
            case Constant.RES_IN:
                PKT_RESULT_IN tempPRI = new PKT_RESULT_IN();

                Array.Copy(_inputData, Marshal.SizeOf<PACKET_HEADER>(), tempByte, 0, Marshal.SizeOf<PKT_RESULT_IN>());
                tempPRI = ByteToStruct<PKT_RESULT_IN>(tempByte, Marshal.SizeOf<PKT_RESULT_IN>());
                Array.Clear(tempByte, 0, tempByte.Length);

                PlayerController.bIsOnline = true;
                //PlayerController.iUserNum = tempPRI.nFlag;
                //DisPlayerController.bIsOnline = true;
                Debug.Log("Connect Complete");
                return;

            case Constant.NOTICE_CHAT:
                PKT_NOTICE_CHAT tempPNC = new PKT_NOTICE_CHAT();

                Array.Copy(_inputData, Marshal.SizeOf<PACKET_HEADER>(), tempByte, 0, Marshal.SizeOf<PKT_NOTICE_CHAT>());
                tempPNC = ByteToStruct<PKT_NOTICE_CHAT>(tempByte, Marshal.SizeOf<PKT_NOTICE_CHAT>());
                Array.Clear(tempByte, 0, tempByte.Length);

                string msg = new string(tempPNC.szMessage);
                string[] DummyMsg = msg.Split('\0');

                jsonmgr.CreateJsonFile(datapath, "Displayer", DummyMsg[0]);
                
                return;

            case Constant.RES_CREATE:
                PKT_RES_CREATE tempPRC = new PKT_RES_CREATE();
                tempPRC.bSuccess = false;

                Array.Copy(_inputData, Marshal.SizeOf<PACKET_HEADER>(), tempByte, 0, Marshal.SizeOf<PKT_RES_CREATE>());
                tempPRC = ByteToStruct<PKT_RES_CREATE>(tempByte, Marshal.SizeOf<PKT_RES_CREATE>());
                Array.Clear(tempByte, 0, tempByte.Length);

                Debug.Log("create" + tempPRC.bSuccess);

                CreateWindow.instance.bSuccess = tempPRC.bSuccess;
                CreateWindow.instance.iRoomNum = tempPRC.nNum;

                return;

            case Constant.RES_JOIN:
                PKT_RES_JOIN tempPRJ = new PKT_RES_JOIN();

                Array.Copy(_inputData, Marshal.SizeOf<PACKET_HEADER>(), tempByte, 0, Marshal.SizeOf<PKT_RES_JOIN>());
                tempPRJ = ByteToStruct<PKT_RES_JOIN>(tempByte, Marshal.SizeOf<PKT_RES_JOIN>());
                Array.Clear(tempByte, 0, tempByte.Length);

                Debug.Log("join" + tempPRJ.bSuccess);

                if (DontDestroy.instance.iSeq == 0)
                    CreateWindow.instance.bJoin = tempPRJ.bSuccess;
                else
                    JoinWindow.instance.bJoin = tempPRJ.bSuccess;
                return;

            case Constant.NOTICE_PUZZLE:
                PKT_NOTICE_PUZZLE tempPNP = new PKT_NOTICE_PUZZLE();

                Array.Copy(_inputData, Marshal.SizeOf<PACKET_HEADER>(), tempByte, 0, Marshal.SizeOf<PKT_NOTICE_PUZZLE>());
                tempPNP = ByteToStruct<PKT_NOTICE_PUZZLE>(tempByte, Marshal.SizeOf<PKT_NOTICE_PUZZLE>());
                Array.Clear(tempByte, 0, tempByte.Length);

                //_ingame.JsonReceive(tempPNP.cPuzzleID);
                //IngameManager.instance.iActivate = (int)char.GetNumericValue(tempPNP.cPuzzleID);
                IngameManager.instance.iActivate = tempPNP.nPuzzleID;
                return;
        }
    }

    public static byte[] StructToByte(object _obj)
    {
        int size = Marshal.SizeOf(_obj);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(_obj, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }

    private T ByteToStruct<T>(byte[] _inputHeader, int size) where T : new()
    {
        IntPtr ptr = Marshal.AllocHGlobal(size);
        T oResult = new T();

        Marshal.Copy(_inputHeader, 0, ptr, size);
        try
        {
            oResult = (T)Marshal.PtrToStructure(ptr, typeof(T));
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
        
        Marshal.FreeHGlobal(ptr);

        return oResult;
    }

}
