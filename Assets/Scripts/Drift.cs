using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//why only left right directions, not 360 degrees
//drift keeps object in motion and controls speed?
//what is the parent to the child? inherit properties
public class Drift : MonoBehaviour
{
    public float speed = 5.0f;
    public enum DriftDirection
    {
        LEFT = -1,
        RIGHT = 1
    }
    // Start is called before the first frame update
    public DriftDirection driftDirection = DriftDirection.LEFT;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*switch (driftDirection)
        {
            case DriftDirection.LEFT:
                transform.Translate(Vector3.left * Time.deltaTime * speed);
                break;
            case DriftDirection.RIGHT:
                transform.Translate(Vector3.right * Time.deltaTime * speed);
                break;
        }*/
        transform.Translate((int)driftDirection * new Vector3(1, 0, 0) * Time.deltaTime * speed);


        if(transform.position.x < -80 || transform.position.x > 80)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
      if(collision.gameObject.CompareTag("Player"))
        {
            GameObject child = collision.gameObject;
            child.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision apple)
    {
        if (apple.gameObject.CompareTag("Player"))
        {
            GameObject child = apple.gameObject;
            child.transform.SetParent(null);
        }

    }


}
