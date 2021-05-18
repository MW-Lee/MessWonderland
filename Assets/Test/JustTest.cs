using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustTest : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(transform.position.x - (3 * Time.deltaTime),
            transform.position.y,
            transform.position.z);
    }
}
