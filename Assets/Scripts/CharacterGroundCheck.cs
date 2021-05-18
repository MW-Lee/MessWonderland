using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundCheck : MonoBehaviour
{
    private PlayerController PC;

    private void Start()
    {
        PC = transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PC.bBCGroundCheck = true;
    }

    private void OnTriggerStay(Collider other)
    {
        PC.bBCGroundCheck = true;
    }

    private void OnTriggerExit(Collider other)
    {
        PC.bBCGroundCheck = false;
    }
}
