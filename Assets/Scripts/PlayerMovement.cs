using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
//layer vs tag
public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody rbPlayer;
    private Vector3 direction = Vector3.zero;
    public float speed = 10.0f;
    public GameObject spawnPoint = null;
    private Dictionary<Item.VegetableType, int> ItemInventory = new Dictionary<Item.VegetableType, int>();
    
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
        
    
        rbPlayer = GetComponent<Rigidbody>();
        foreach(Item.VegetableType item in System.Enum.GetValues(typeof(Item.VegetableType)))
        {
            ItemInventory.Add(item, 0);
        }
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
        rbPlayer.MovePosition(spawnPoint.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            Item item = other.gameObject.GetComponent<Item>();
            AddToInventory(item);
            PrintInventory();

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
        if(other.CompareTag("Hazard"))
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
