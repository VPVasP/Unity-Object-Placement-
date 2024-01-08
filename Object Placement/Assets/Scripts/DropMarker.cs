using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMarker : MonoBehaviour
{
    //bool that checks if the tile is free
    public bool FreeTile;
    //materials
    public Material Green;
    public Material Red;

    Renderer thisRenderer;

    // Start is called before the first frame update
    void Start()
    {
        FreeTile = true;
        thisRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        //we check if the colldier that is entered is a capsule
        if (other.CompareTag("Capsule"))
        {
            //we change the material and the free tile to false
            thisRenderer.material = Red;
            FreeTile = false;
        }
        
    }

    private void OnTriggerStay (Collider other)
    {
        if (other.CompareTag("Capsule"))
        {
            //we change the material and the free tile to false
            thisRenderer.material = Red;
            FreeTile = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Capsule"))
        {
            //we change the material and the free tile to true
            thisRenderer.material = Green;
            FreeTile = true;
        }

    }
}
