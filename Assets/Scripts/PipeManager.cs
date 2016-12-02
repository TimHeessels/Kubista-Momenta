using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO.Ports;

public class PipeManager : MonoBehaviour
{
    public  int[] pipeID;
    public GameObject[] Pipes;
    private Transform IndicatorSphere;
    private Transform[] ColorLocations;
    private bool IsClicking;
    private Slider fillSlider;
    private Arduino arduino;

    void Start ()
    {
        arduino = GameObject.Find("ArduinoManager").GetComponent<Arduino>();
        ColorLocations = new Transform[7];
        ColorLocations[0] = GameObject.Find("RedPosition").transform;
        ColorLocations[1] = GameObject.Find("GreenPosition").transform;
        ColorLocations[2] = GameObject.Find("BluePosition").transform;
        IndicatorSphere = GameObject.Find("IndicatorSphere").transform;
        fillSlider = GameObject.Find("PipeCanvas").transform.FindChild("FillSlider").GetComponent<Slider>();

        Pipes = new GameObject[transform.childCount];
        pipeID = new int[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Pipes[i] = transform.GetChild(i).gameObject;
            if (Pipes[i].tag == "Red") { pipeID[i] = 0; }
            if (Pipes[i].tag == "Green") { pipeID[i] = 1; }
            if (Pipes[i].tag == "Blue") { pipeID[i] = 2; }
            if (Pipes[i].tag == "LightBlue") { pipeID[i] = 3; }
            if (Pipes[i].tag == "Pink") { pipeID[i] = 4; }
            if (Pipes[i].tag == "Yellow") { pipeID[i] = 5; }
        }
        Randomize();
    }

    void Randomize()
    {
        for (int i = 0; i < 15; i++)
        {
            PressedKey(Random.Range(0,5));
        }
    }

	void Update ()
    {
        int count = 0;
        Vector3 newPosition = new Vector3(0, 0, 0);
        newPosition = ((ColorLocations[0].position + ColorLocations[1].position + ColorLocations[2].position) / 3);
        if (arduino.inputNumber == 0 || arduino.inputNumber == 1 || arduino.inputNumber == 2) { if (!IsClicking) { IsClicking = true; } }
        if (arduino.inputNumber == 0)
        {
            newPosition = ColorLocations[0].position;
            count ++;
        }
        if (arduino.inputNumber == 1)
        {
            newPosition = ColorLocations[1].position;
            count++;
        }
        if (arduino.inputNumber  == 2)
        {
            newPosition = ColorLocations[2].position;
            count++;
        }
        if (arduino.inputNumber == 3)
        {
            newPosition = (ColorLocations[1].position + ColorLocations[2].position)/2;
            count++;
        }
        if (arduino.inputNumber == 4)
        {
            newPosition = (ColorLocations[0].position + ColorLocations[2].position) / 2;
            count++;
        }
        if (arduino.inputNumber == 5)
        {
            newPosition = (ColorLocations[0].position + ColorLocations[1].position) / 2;
            count++;
        }
        if (arduino.inputNumber == 6 || arduino.inputNumber == 7)
        {
            newPosition = (ColorLocations[0].position + ColorLocations[1].position + ColorLocations[2].position) / 3;
            count++;
            fillSlider.value = 0;
        }
        IndicatorSphere.position = Vector3.Lerp(IndicatorSphere.position, newPosition, Time.deltaTime * 10);
    }

    void FixedUpdate()
    {
        if (IsClicking)
        {
            if (fillSlider.value < 1)
            {
                fillSlider.value += 0.03f;
            }
            else
            {
                if (arduino.inputNumber == 0)
                {
                    PressedKey(0); //red
                }
                if (arduino.inputNumber == 1)
                {
                    PressedKey(1); //green
                }
                if (arduino.inputNumber == 2)
                {
                    PressedKey(2); //blue
                }
                if (arduino.inputNumber == 3)
                {
                    PressedKey(3); //cyan
                }
                if (arduino.inputNumber == 4)
                {
                    PressedKey(4); //magenta
                }
                if (arduino.inputNumber == 5)
                {
                    PressedKey(5); //yellow
                }
                if (arduino.inputNumber == 6)
                {
                    PressedKey(6); //white
                }
                if (arduino.inputNumber == 7)
                {
                    PressedKey(6); //white
                }

                IsClicking = false;
                fillSlider.value = 0;
            }
        }
    }

    /*
    IEnumerator Timer()
    {
        IsClicking = true;
        yield return new WaitForSeconds(1f);
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.B))
        {
            PressedKey(6); //white
        }
        if (!Input.GetKey(KeyCode.R) && !Input.GetKey(KeyCode.G) && !Input.GetKey(KeyCode.B))
        {
            PressedKey(6); //white
        }
        if (!Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.B))
        {
            PressedKey(3); //Lightblue
        }
        if (Input.GetKey(KeyCode.R) && !Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.B))
        {
            PressedKey(4); //pink
        }
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.G) && !Input.GetKey(KeyCode.B))
        {
            PressedKey(5); //yellow
        }
        if (Input.GetKey(KeyCode.R) && !Input.GetKey(KeyCode.G) && !Input.GetKey(KeyCode.B))
        {
            PressedKey(0); //red
        }
        if (!Input.GetKey(KeyCode.R) && !Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.B))
        {
            PressedKey(2); //blue
        }
        if (!Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.G) && !Input.GetKey(KeyCode.B))
        {
            PressedKey(1); //green
        }

        IsClicking = false;
    }
    */

    void PressedKey(int keyID)
    {
        for (int i = 0; i < Pipes.Length; i++)
        {
            var rotated = false;
            if (pipeID[i] == keyID)
            {
                if (!rotated) { rotated = true; Pipes[i].transform.Rotate(new Vector3(0, 90, 0)); }                
            }
        }
    }
}