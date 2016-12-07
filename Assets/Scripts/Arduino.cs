using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

public class Arduino : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM3", 9600);
    public CypherScript cypherScript;
    public LazerGameManager lazerGameManager;
    public int buttonPressed;
    public int test;

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
        sp.Open();
        sp.ReadTimeout = 1;
    }

    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                buttonPressed = int.Parse(sp.ReadLine());
                SendButtonInfo(); 
            }
            catch (System.Exception)
            {
                return;
            }
        }
    }

    void SendButtonInfo()
    {
        if (buttonPressed == 0)
        {
            cypherScript.Rotate(true);
        }
        if (buttonPressed == 1)
        {
            cypherScript.Rotate(false);
        }
        if (buttonPressed == 2)
        {
            cypherScript.SwitchRing(true);
        }
        if (buttonPressed == 3)
        {
            cypherScript.SwitchRing(false);
        }
        if (buttonPressed == 4)
        {
            //all buttons pressed (red green and blue)
        }
        if (buttonPressed == 5)
        {
            //red and blue pressed
        }
        if (buttonPressed == 6)
        {
            //red and green pressed
        }
        if (buttonPressed == 7)
        {
            //green and blue pressed
        }
        if (buttonPressed == 8)
        {
            lazerGameManager.Button1();
        }
        if (buttonPressed == 9)
        {
            lazerGameManager.Button2();
        }
        if (buttonPressed == 10)
        {
            lazerGameManager.Button3();
        }
    }
}