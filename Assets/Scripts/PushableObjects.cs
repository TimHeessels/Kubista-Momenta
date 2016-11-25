using UnityEngine;
using System.Collections;

public class PushableObjects : MonoBehaviour
{
    private BoxCollider collider;
    private Animator boxAnimator;
    private PlayerLamp playerLamp;
    public bool HitFloor;
    public bool HitWater;
    public bool HitPit;

    void Start()
    {
        collider = GetComponent<BoxCollider>();
        if (transform.childCount > 0)
        {
            boxAnimator = transform.GetChild(0).GetComponent<Animator>();
        }
        playerLamp = GameObject.Find("Player").GetComponent<PlayerLamp>();
    }

    void Update()
    {
        if (HitWater && !HitFloor && !HitPit)
        {
            StartCoroutine(SinkInHole(true));
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "PIT")
        {
            Destroy(collider);
            Destroy(other.GetComponent<BoxCollider>());
            StartCoroutine(SinkInHole(false));
        }
        if (other.gameObject.tag == "Floor") HitFloor = true;
        if (other.gameObject.tag == "Water") HitWater = true;
        if (other.gameObject.name == "PIT") HitPit = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Floor") HitFloor = false;
        if (other.gameObject.tag == "Water") HitWater = false;
        if (other.gameObject.name == "PIT") HitPit = false;
    }

    IEnumerator SinkInHole(bool destroy)
    {
        playerLamp.freezeMovement = true;
        boxAnimator.Play("SinkBox");
        yield return new WaitForSeconds(.8f);
        playerLamp.ResetTriggerState();
        playerLamp.freezeMovement = false;
        if (destroy) Destroy(this.gameObject);
    }
}
