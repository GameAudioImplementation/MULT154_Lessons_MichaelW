using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
//client network dev build not working. carrots text box diff location
public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody rbPlayer;
    private Vector3 direction = Vector3.zero;
    public float speed = 10.0f;
    public GameObject[]spawnPoints = null;


    
    public AudioClip movementSound;
    private AudioSource source;
    public AudioClip splashSound;
    public AudioClip veggieSound;
    public AudioSource audioS;
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

        rbPlayer = GetComponent<Rigidbody>();
        spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
    } 

    private void AddToInventory(Item item)
    {
        ItemInventory[item.typeOfVeggie]++;
    }

    private void PrintInventory()
    {
        string output = "";

        foreach(KeyValuePair<Item.VegetableType, int> kvp in ItemInventory)
        {
            output += string.Format("{0}: {1} ", kvp.Key, kvp.Value);
        }
        Debug.Log(output);
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


    // Update is called once per frame
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
