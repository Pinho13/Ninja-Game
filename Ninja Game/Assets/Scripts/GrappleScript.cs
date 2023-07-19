using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrappleState{ Idle, Shot , Grappled};
public class GrappleScript : MonoBehaviour
{
    [SerializeField] GameObject grapple;
    [SerializeField] float maxDistance;
    [SerializeField] float grappleForce;
    [HideInInspector] public GrappleState grappleState;




///////////////PlayerReferences///////////////
    Rigidbody2D rb;
    LineRenderer lr;
    Vector2 PlayerPos;

///////////////GrappleReferences///////////////
    GameObject GrappleSpawned;

///////////////MouseReference///////////////
    Vector2 mousePos;



    void Start()
    {
        GetPlayerComponents();
    }

    void GetPlayerComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }


    void Update()
    {
        Grappling();
        GrappleShot();
    }



    void Grappling()
    {
        PlayerPos = transform.position;
        lr.SetPosition (0, PlayerPos);
        if(GrappleSpawned != null)
        {
            lr.SetPosition (1, GrappleSpawned.transform.position);
        }else
        {
            lr.SetPosition (1, PlayerPos);
        }
    }

    void GrappleShot()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(grappleState == GrappleState.Idle)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector2 dir = mousePos - PlayerPos;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                GrappleSpawned = Instantiate(grapple, PlayerPos, rotation);
                GrappleSpawned.GetComponent<Rigidbody2D>().velocity = dir.normalized * grappleForce;
                grappleState = GrappleState.Shot;
            }
        }

        if(grappleState == GrappleState.Shot && Vector2.Distance(PlayerPos, GrappleSpawned.transform.position) > maxDistance)
        {
            Destroy(GrappleSpawned);
            grappleState = GrappleState.Idle;
        }
    }
}
