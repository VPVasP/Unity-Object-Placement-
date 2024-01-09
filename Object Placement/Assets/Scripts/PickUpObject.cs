using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private string pickupNameLayer;
    [SerializeField] private string objectTag;
    private void Start()
    {
        //we assign our pickupNameLayer to the PickObject and if we don't have a rigibody we add one.
        pickupNameLayer = "PickObject";
        objectTag = gameObject.name;
        if (rb == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }
        rb = GetComponent<Rigidbody>();
        pickupLayer = LayerMask.GetMask(pickupNameLayer);
        gameObject.tag = objectTag;
 }
   }
