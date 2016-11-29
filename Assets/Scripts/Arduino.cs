using UnityEngine;
using System.IO.Ports;


public class Arduino : MonoBehaviour
{
    public SerialPort sp = new SerialPort("COM3", 9600);
    public int buttonNumber;

    void Start()
    {
        sp.Open();        
        sp.ReadTimeout = 100;
    }
    
    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                buttonNumber = sp.ReadByte();
            }
            catch (System.Exception)
            {
                throw;
            }
        }               
    }
}
