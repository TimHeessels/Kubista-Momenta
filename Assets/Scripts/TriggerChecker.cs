using UnityEngine;
using System.Collections;

public class TriggerChecker : MonoBehaviour
{
    public string selectedButton;
    public bool hitWater;
    public bool hitCardboardBox;
    public bool hitGoal;
    public bool hitFloor;
    public bool movementBlocked;
    public bool hitBridge;
    public GameObject CardboardBox;
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Water") hitWater = true;
        if (other.gameObject.tag == "CardboardBox") { hitCardboardBox = true; CardboardBox = other.gameObject; }
        if (other.gameObject.tag == "Goal") hitGoal = true;
        if (other.gameObject.tag == "Floor") hitFloor = true;
        if (other.gameObject.tag == "Buttons") { movementBlocked = true; selectedButton = other.gameObject.name; }
        if (other.gameObject.tag == "CardboardBox") movementBlocked = true;
        if (other.gameObject.tag == "Goal") movementBlocked = true;
        if (other.gameObject.tag == "Bridges") hitBridge = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Water") hitWater = false;
        if (other.gameObject.tag == "CardboardBox") hitCardboardBox = false;
        if (other.gameObject.tag == "Goal") hitGoal = false;
        if (other.gameObject.tag == "Floor") hitFloor = false;
        if (other.gameObject.tag == "Buttons") { movementBlocked = false; selectedButton = ""; }
        if (other.gameObject.tag == "CardboardBox") movementBlocked = false;
        if (other.gameObject.tag == "Goal") movementBlocked = false;
        if (other.gameObject.tag == "Bridges") hitBridge = false;
    }

    public void ResetTriggers()
    {
        hitWater = false;
        hitCardboardBox = false;
        hitGoal = false;
        hitFloor = false;
        movementBlocked = false;
        movementBlocked = false;
        movementBlocked = false;
        selectedButton = "";
    }
}
