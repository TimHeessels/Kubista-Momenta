using UnityEngine;
using System.Collections;

public class CypherScript : MonoBehaviour
{
    private Light[] greenLights;
    private bool greenLightIsShrinking;
    private GameObject lightsFolder;
    public GameObject[] Rings;
    private float[] finishLocation;
    private float[] completedMessageLocation;
    private bool[] inPlace;
    private int selectedWheel;
    private float[] rotation;
    private float margin;
    private int finished;
    private bool activatedFinishTimer;
    private bool activateCorrectMessage;

    void Start ()
    {
        greenLights = new Light[5];
        margin = 5f;
        finishLocation = new float[5];
        completedMessageLocation = new float[5];
        inPlace = new bool[5];
        rotation = new float[5];
        lightsFolder = GameObject.Find("Lights");
        for (int i = 0;i<5;i++)
        {
            greenLights[i] = lightsFolder.transform.GetChild(i).GetComponent<Light>();
            rotation[i] = 21.189f;
        }
        finishLocation[0] = 121.214f;
        finishLocation[1] = 107.408f;
        finishLocation[2] = 179.358f;
        finishLocation[3] = 179.358f;
        finishLocation[4] = 207.389f;
        completedMessageLocation[0] = 252.376f;
        completedMessageLocation[1] = 92.395f;
        completedMessageLocation[2] = 265.778f;
        completedMessageLocation[3] = 121.939f;
        completedMessageLocation[4] = 107.597f;
        StartCoroutine(FlashGreenLights());
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) //switch selected 
        {
            Application.Quit();
        }
        for (int i = 0; i < 5; i++)
        {
            if (Rings[i].transform.localEulerAngles.z > (finishLocation[i] - margin) && Rings[i].transform.localEulerAngles.z < (finishLocation[i] + margin))
            {
                inPlace[i] = true;
            }
            else
            {
                inPlace[i] = false;
            }
        }
        finished = 0;
        for (int i = 0; i < 5; i++)
        {
            if (inPlace[i])
            {
                finished++;
            }
        }
        for (int i = 0; i < 5; i++)
        {
            if (activateCorrectMessage)
            {
                Rings[i].transform.rotation = Quaternion.Slerp(Rings[i].transform.rotation, Quaternion.Euler(0, -90, completedMessageLocation[i]), Time.deltaTime * 3);
            }
            else
            {
                if (finished < 5)
                {
                    Rings[i].transform.rotation = Quaternion.Slerp(Rings[i].transform.rotation, Quaternion.Euler(0, -90, rotation[i]), Time.deltaTime * 10);
                }
                else
                {
                    Rings[i].transform.rotation = Quaternion.Slerp(Rings[i].transform.rotation, Quaternion.Euler(0, -90, finishLocation[i]), Time.deltaTime * 3);
                }
                if (i == selectedWheel && finished < 5)
                {
                    Rings[i].transform.localScale = Vector3.Lerp(Rings[i].transform.localScale, new Vector3(1.1f, 1.1f, 1.1f), Time.deltaTime * 3);
                }
                else
                {
                    Rings[i].transform.localScale = Vector3.Lerp(Rings[i].transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 6);
                }
            }
        }
        if (finished < 5)
        {
            if (Input.GetButtonDown("Submit")) //switch selected 
            {
                if (selectedWheel < 4) selectedWheel++;
                else selectedWheel = 0;
            }
            rotation[selectedWheel] += (Input.GetAxis("Vertical") * 0.5f);
            if (rotation[selectedWheel] > 360)
            {
                rotation[selectedWheel] = 0;
            }
            if (rotation[selectedWheel] < 0)
            {
                rotation[selectedWheel] = 360;
            }
        }
        else
        {
            if (!activatedFinishTimer)
            {
                StartCoroutine(FlashGreenLights());
                activatedFinishTimer = true;
                StartCoroutine(FinishTimer());
            }
        }
    }

    IEnumerator FlashGreenLights()
    {
        yield return new WaitForSeconds(.01f);
        if (greenLights[0].intensity > 5)
        {
            greenLightIsShrinking = true;
        }
        for (int i = 0; i < 5; i++)
        {
            if (greenLightIsShrinking) greenLights[i].intensity -= .1f;
            else greenLights[i].intensity += .1f;
        }
        if (greenLights[0].intensity > 0)
        {
            StartCoroutine(FlashGreenLights());
        }        
    }

    IEnumerator FinishTimer()
    {
        yield return new WaitForSeconds(4f);
        activateCorrectMessage = true;
    }
}
