using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public List<Effects> Effect{ get; set;}

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            
        }
    }
}
