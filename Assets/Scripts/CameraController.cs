using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private void FixedUpdate()
    {
        Vector3 newPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z - 11);
        transform.position = newPosition;
    }
}
