using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{

    Collider2D col;
    public List<Collider2D> detectedColliders = new List<Collider2D>();

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collision detected! Object: {collision.name}, Tag: {collision.tag}, Layer: {LayerMask.LayerToName(collision.gameObject.layer)}");

        if (!detectedColliders.Contains(collision))
        {
            detectedColliders.Add(collision);
            Debug.Log($"Detected {collision.name} entering {gameObject.name}");
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedColliders.Remove(collision);
    }
}
