using UnityEngine;
using System.IO.Ports;

public class SerialManager : MonoBehaviour
{
    public static SerialManager Instance { get; private set; }

    private SerialPort sp;
    
    public char LastReceivedChar { get; private set; } = '\0';

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sp = new SerialPort("COM12", 115200);
        if (!sp.IsOpen)
        {
            try 
            {
                sp.Open();
                sp.ReadTimeout = 10;
                Debug.Log("Puerto COM12 Abierto Correctamente por el Manager.");
            } 
            catch (System.Exception e) 
            {
                Debug.LogError("Error al abrir el puerto: " + e.Message);
            }
        }
    }

    void Update()
    {
        LastReceivedChar = '\0'; 

        if (sp != null && sp.IsOpen && sp.BytesToRead > 0)
        {
            try
            {
                LastReceivedChar = (char)sp.ReadChar();
            }
            catch (System.TimeoutException) { }
        }
    }

    public void SendScore(int score)
    {
        if (sp != null && sp.IsOpen)
        {
            try
            {
                byte[] dataBuffer = new byte[1];
                dataBuffer[0] = (byte)score; 

                sp.Write(dataBuffer, 0, 1);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al enviar score: " + e.Message);
            }
        }
    }
    void OnApplicationQuit()
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Close();
            Debug.Log("Puerto COM12 Cerrado.");
        }
    }
}