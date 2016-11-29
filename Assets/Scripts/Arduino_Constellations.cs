using UnityEngine;
using System.IO.Ports;
using System.Collections;

public class Arduino_Constellations : MonoBehaviour
{
    public SerialPort sp = new SerialPort("COM3", 9600);    

    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 100;
        sp.WriteTimeout = 100;        
    }    

    void Update()
    {
        try
        {
            //readbyte
        }
        catch (System.Exception)
        {            
            return;
        }    
    }    
}