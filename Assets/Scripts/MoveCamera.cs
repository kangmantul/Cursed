using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform CamPos;

    private void Update()
    {
        transform.position = CamPos.position;
    }
}
