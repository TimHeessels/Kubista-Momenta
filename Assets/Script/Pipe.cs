using UnityEngine;
using System.Collections;

public class Pipe : MonoBehaviour
{
    public int pipeID;
    public bool up;
    public bool left;
    public bool right;
    public bool down;

    void Start ()
    {
        if ((gameObject.name.Length > 3))
        {
            if (gameObject.name.Substring(0, 4) == "HOEK")
            {
                pipeID = 0;
            }
        }
        if ((gameObject.name.Length > 4))
        {
            if (gameObject.name.Substring(0, 5) == "RECHT")
            {
                pipeID = 1;
            }
        }
        if ((gameObject.name.Length > 10))
        {
            if (gameObject.name.Substring(0, 11) == "T_SPLITSING")
            {
                pipeID = 2;
            }
        }
        if ((gameObject.name.Length > 8))
        {
            if (gameObject.name.Substring(0, 9) == "KRUISPUNT")
            {
                pipeID = 3;
            }
        }
    }	

	void Update()
    {
        if (pipeID == 0) //Bocht gedeelte
        {
            if ((transform.eulerAngles.x > 315 || transform.eulerAngles.x < 45))
            {
                up = false;
                left = true; 
                down = true; 
                right = false; 
            }
            if (transform.eulerAngles.x > 45 && transform.eulerAngles.x < 135)
            {
                up = false;
                left = false;
                down = true;
                right = true;
            }
            if (transform.eulerAngles.x > 135 && transform.eulerAngles.x < 225)
            {
                up = true;
                left = false;
                down = false;
                right = true;
            }
            if (transform.eulerAngles.x > 225 && transform.eulerAngles.x < 315)
            {
                up = true;
                left = true;
                down = false;
                right = false;
            }
        }
        if (pipeID == 1) //Recht gedeelte
        {
            if ((transform.eulerAngles.x > 315 || transform.eulerAngles.x < 45))
            {
                up = false;
                left = true;
                down = false;
                right = true;
            }
            if (transform.eulerAngles.x > 45 && transform.eulerAngles.x < 135)
            {
                up = true;
                left = false;
                down = true;
                right = false;
            }
            if (transform.eulerAngles.x > 135 && transform.eulerAngles.x < 225)
            {
                up = false;
                left = true;
                down = false;
                right = true;
            }
            if (transform.eulerAngles.x > 225 && transform.eulerAngles.x < 315)
            {
                up = true;
                left = false;
                down = true;
                right = false;
            }
        }
        if (pipeID == 2) //T splitsing gedeelte
        {
            if ((transform.eulerAngles.x > 315 || transform.eulerAngles.x < 45))
            {
                up = true;
                left = true;
                down = true;
                right = false;
            }
            if (transform.eulerAngles.x > 45 && transform.eulerAngles.x < 135)
            {
                up = false;
                left = true;
                down = true;
                right = true;
            }
            if (transform.eulerAngles.x > 135 && transform.eulerAngles.x < 225)
            {
                up = true;
                left = false;
                down = true;
                right = true;
            }
            if (transform.eulerAngles.x > 225 && transform.eulerAngles.x < 315)
            {
                up = true;
                left = true;
                down = false;
                right = true;
            }
        }
    }
}
