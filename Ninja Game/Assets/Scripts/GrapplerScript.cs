using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplerScript : MonoBehaviour
{
    Rigidbody2D rb;
    Rigidbody2D PlayerRb;
    GameObject player;

    [SerializeField] float PlayerForce;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>().gameObject;
        PlayerRb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "GrapplePoint")
        {
            player.GetComponent<GrappleScript>().grappleState = GrappleState.Grappled;
            rb.velocity = Vector2.zero;
            PlayerRb.AddForce((transform.position - player.transform.position).normalized * PlayerForce, ForceMode2D.Impulse);
            Destroy(this.gameObject, 0.3f);
            player.GetComponent<GrappleScript>().grappleState = GrappleState.Idle;
        }
    }
}
