using UnityEngine;
using System.Collections;

public class CameraWakeup : MonoBehaviour
{
	void Start ()
	{
        GetComponent<Camera>().enabled = true;
        Display.displays[0].SetRenderingResolution(1680, 1050);
	}
}
