using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpPlace : MonoBehaviour
{
    //layers to pick up
    public LayerMask pickLayer;
    public LayerMask groundLayer;
    //pickedup object and dropmarker
    public Transform pickedObject;
    [SerializeField] private Transform dropMarker;
    [SerializeField] private float SmoothSpeed;
    //dropmarker script refrence
    DropMarker dropMarkerComponent;
    float SwapCurrent;
    [SerializeField] private float SwapRate = 0.15f;
    private Vector3 velocity = Vector3.zero;
    private AudioSource aud;
    [SerializeField] private AudioClip dropClip;
    public Transform tileMarker;
    // Start is called before the first frame update
    public enum ObjectType
    {
        Capsule,
        Cube,
        Sphere,
    }
    void Start()
    {
        dropMarkerComponent = dropMarker.GetComponent<DropMarker>();
        dropMarker.gameObject.SetActive(false);
        tileMarker.gameObject.SetActive(false);
        SmoothSpeed = 0.2f;
        if (aud == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
        aud = gameObject.GetComponent<AudioSource>();
        aud.loop = false;
        aud.playOnAwake = false;

    }

    // Update is called once per frame
    void Update()
    {
        //if we press left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            if (pickedObject == null)
            {
                //raycast to pick up the object
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, pickLayer))
                {
                    //we pick up the object if it's a capsule
                    Debug.Log("Hit object: " + hit.collider.gameObject.name);

                    if (hit.collider.CompareTag(ObjectType.Capsule.ToString()))
                    {
                        pickedObject = hit.transform.root;
                        pickedObject.GetComponent<Rigidbody>().isKinematic = true;
                        dropMarker.gameObject.SetActive(true);
                        tileMarker.gameObject.SetActive(true);
                    }
                     if (hit.collider.CompareTag(ObjectType.Cube.ToString()))
                    {
                        pickedObject = hit.transform.root;
                        pickedObject.GetComponent<Rigidbody>().isKinematic = true;
                        dropMarker.gameObject.SetActive(true);
                        tileMarker.gameObject.SetActive(true);
                    }
                     if (hit.collider.CompareTag(ObjectType.Sphere.ToString()))
                    {
                        pickedObject = hit.transform.root;
                        pickedObject.GetComponent<Rigidbody>().isKinematic = true;
                        dropMarker.gameObject.SetActive(true);
                        tileMarker.gameObject.SetActive(true);
                    }

                    Debug.DrawLine(ray.origin, hit.point, Color.green);
                }
            }

        
        else
            {
                //we drop the object if the marker is on a free tile
                if (dropMarkerComponent.FreeTile)
                {
                    int x = Mathf.CeilToInt(dropMarker.position.x / 0.33f);
                    float xf = x * 0.33f;
                    xf -= 0.165f;
                    int z = Mathf.CeilToInt(dropMarker.position.z / 0.33f);
                    float xz = z * 0.33f;
                    xz -= 0.165f;

                    pickedObject.position = new Vector3(xf, pickedObject.position.y, xz);
                    pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                    pickedObject = null;
                    dropMarker.gameObject.SetActive(false);
                    tileMarker.gameObject.SetActive(false);
                    DroppedObject(x, z);
              }
               
            }

           
        }
        //we handle the movement and rotation if an object is picked up
        if (pickedObject != null)
        {
            //raycast to get the position off the ground
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int x = Mathf.CeilToInt(dropMarker.position.x / 0.33f);
            float xf = x * 0.33f;
            xf -= 0.165f;
            int z = Mathf.CeilToInt(dropMarker.position.z / 0.33f);
            float xz = z * 0.33f;
            xz -= 0.165f;
            tileMarker.position = new Vector3(xf, 0.002f, xz);


            if (Physics.Raycast(ray, out hit, 100, groundLayer))
            {
                //we move the picked up object to the ground position
                dropMarker.position = hit.point;

                pickedObject.position = Vector3.SmoothDamp(pickedObject.position, hit.point + Vector3.up / 2, ref velocity, SmoothSpeed);
            
            }
            //rotationHandling with mouse scrollwheel
            SwapCurrent -= Time.deltaTime;

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (SwapCurrent <= 0 )
                {
                    pickedObject.Rotate(0, 0, -90);
                    SwapCurrent = SwapRate;
                  
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (SwapCurrent <= 0)
                {
                    pickedObject.Rotate(0, 0, 90);
                    SwapCurrent = SwapRate;
                    
                }
            }
        }
        else
        {
            SwapCurrent -= Time.deltaTime;
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (SwapCurrent <= 0)
                {
                    transform.position += transform.forward;
                    SwapCurrent = SwapRate/4;
                    
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (SwapCurrent <= 0)
                {
                    transform.position -= transform.forward;
                    SwapCurrent = SwapRate/4;
                 
                }
            }
        }
    }
    //function that gets called when a figure is being dropped
    public void DroppedObject(int DropX,int DropZ)
    {
        aud.clip = dropClip;
        aud.Play();
        Debug.Log("Dropped on tile: " + DropX + "," + DropZ);
    }
}
