using UnityEngine;
using System.Collections;

public class CircleScript : MonoBehaviour
{
    private Vector3 startPosition;
    private GameObject cameraObject;
    private float rotation;
    private bool isDead;
    private bool finishedGame;
    private Vector3 velocity = Vector3.zero;
    private bool canMove;

    void Start ()
    {
        cameraObject = GameObject.Find("Main_Camera");
        startPosition = transform.position;
        canMove = true;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!isDead)
        {
            canMove = false;
            bool cleared;
            cleared = false;
            if (rotation > 350 || rotation < 5)
            {
                if (collision.tag == "North") { if (collision.name == "Star") { cleared = true; } }
                if (collision.tag == "East") { if (collision.name == "Box") { cleared = true; } }
                if (collision.tag == "South") { if (collision.name == "Tee") { cleared = true; } }
                if (collision.tag == "West") { if (collision.name == "Cross") { cleared = true; } }
            }
            if (rotation > 85 && rotation < 95)
            {
                if (collision.tag == "North") { if (collision.name == "Cross") { cleared = true; } }
                if (collision.tag == "East") { if (collision.name == "Star") { cleared = true; } }
                if (collision.tag == "South") { if (collision.name == "Box") { cleared = true; } }
                if (collision.tag == "West") { if (collision.name == "Tee") { cleared = true; } }
            }
            if (rotation > 175 && rotation < 185)
            {
                if (collision.tag == "North") { if (collision.name == "Tee") { cleared = true; } }
                if (collision.tag == "East") { if (collision.name == "Cross") { cleared = true; } }
                if (collision.tag == "South") { if (collision.name == "Star") { cleared = true; } }
                if (collision.tag == "West") { if (collision.name == "Box") { cleared = true; } }
            }
            if (rotation > 265 && rotation < 275)
            {
                if (collision.tag == "North") { if (collision.name == "Box") { cleared = true; } }
                if (collision.tag == "East") { if (collision.name == "Tee") { cleared = true; } }
                if (collision.tag == "South") { if (collision.name == "Cross") { cleared = true; } }
                if (collision.tag == "West") { if (collision.name == "Star") { cleared = true; } }
            }
            if (!cleared)
            {
                if (collision.tag == "Finished")
                {
                    finishedGame = true;
                    StartCoroutine(Finished());
                }
                else
                {
                    isDead = true;
                    StartCoroutine(Reset());
                }
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        canMove = true;
    }

	void Update ()
    {
        if (!finishedGame)
        {
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, transform.position.y + 2f, cameraObject.transform.position.z);
            transform.Translate((new Vector3(0, 0, -1) * Time.deltaTime) * 0.4f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-90, 0, rotation), Time.deltaTime * 3);
            if (!isDead)
            {
                if (Input.GetButtonDown("Right") && canMove)
                {
                    if (rotation > 179) rotation -= 90;
                    else rotation = 360;
                }
                if (Input.GetButtonDown("Left") && canMove)
                {
                    if (rotation < 269) rotation += 90;
                    else rotation = 0;
                }
            }
            else
            {
                Vector3 targetPosition = startPosition;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, Time.deltaTime * 5);
            }
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2f);
        isDead = false;
    }

    IEnumerator Finished()
    {
        yield return new WaitForSeconds(2f);
        //do something
    }
}
