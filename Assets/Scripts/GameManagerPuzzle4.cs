using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManagerPuzzle4 : MonoBehaviour
{
    void Start()
    {
        if (Display.displays.Length > 1)
        Display.displays[1].Activate();
        if (Display.displays.Length > 2)
        Display.displays[2].Activate();
    }
}