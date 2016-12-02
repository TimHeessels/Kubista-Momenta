using UnityEngine;
using System.IO.Ports;


public class Arduino : MonoBehaviour
{
    public SerialPort sp = new SerialPort("COM3", 9600);
    public int inputNumber;

    private static Arduino instance = null;

    // Game Instance Singleton
    public static Arduino Instance
    {
        get
        {
            return instance;
        }
    }

    void Start()
    {
        if (sp.IsOpen)
        {
            try
            {
                sp.Open();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        sp.ReadTimeout = 100;
    }
    
    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                inputNumber = sp.ReadByte();
            }
            catch (System.Exception)
            {
                throw;
            }
        }               
    }
}
