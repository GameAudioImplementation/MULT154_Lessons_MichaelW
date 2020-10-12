using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
//<> brackets vs ()
//Vector3.zero
//why return exits method

/*ClassNm objDeclr = new classNm();
objDeclr.functionInClass();
build vs compile

what does "this" mean in the gameobjects "autos"
escape key is not working to quit game
*/


public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody rbPlayer;
    private Vector3 direction = Vector3.zero;
    public float speed = 10.0f;
    public GameObject[]spawnPoints = null;


    
    public AudioClip movementSound;
    public AudioClip splashSound;
    public AudioClip veggieSound;
    public AudioSource audioS;
    private AudioSource source;
    public float volLowRange = .5f;
    public float volHighRange = 1.0f;
    public AudioMixerSnapshot idleSnapshot;
    public AudioMixerSnapshot auxInSnapshot;
    public LayerMask lilyMask;
    bool veggiesNear;


    //TriggerEnger to play sound
  
    void Awake () 
    {
             source = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        rbPlayer = GetComponent<Rigidbody>(); //why is this not rbPlayer = Component(); rbPlayer.GetComponent<Rigidbody>();
        spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
    } 

   
    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        float horMove = Input.GetAxis("Horizontal");
        float verMove = Input.GetAxis("Vertical");

        direction = new Vector3(horMove, 0, verMove);

        /*
        RaycastHit[] landing = Physics.BoxCastAll(transform.position, 1f, transform.forward, 0f, lilyMask);
        if(landing.Length > 0)
        {
            if (!veggiesNear)
            {
                audioS.PlayOneShot(veggieSound);
                veggiesNear = true;
            }
        }
        else
        {
            if (veggiesNear)
            {
                veggiesNear = false;
            }
        }*/
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, direction * 10);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, rbPlayer.velocity * 5);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(6, 6, 6));
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        rbPlayer.AddForce(direction * speed, ForceMode.Force);
        if(transform.position.z > 40)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 40);
        }
        else if(transform.position.z < -40)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -40);
        }
    }
    private void Respawn()
    {
        int index = 0;
        while(Physics.CheckBox(spawnPoints[index].transform.position, new Vector3(1.5f, 1.5f, 1.5f)))
        {
            index++;
        }
        rbPlayer.MovePosition(spawnPoints[index].transform.position);
        rbPlayer.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (other.CompareTag("Hazard"))
        {
            audioS.PlayOneShot(splashSound);
        }
        if (other.CompareTag("TriggerAuxMusic"))
        {
            auxInSnapshot.TransitionTo(0.5f);
        }
 
    }
 
    private void OnTriggerExit(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (other.CompareTag("Hazard"))
        {
            Respawn();
        }
        if (other.CompareTag("LilyPad"))
        {
            audioS.PlayOneShot(splashSound);
        }
        if (other.CompareTag("TriggerAuxMusic"))
        {
            idleSnapshot.TransitionTo(0.5f);
        }
    }

}
