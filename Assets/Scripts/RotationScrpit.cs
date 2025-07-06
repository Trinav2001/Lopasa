using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScrpit : MonoBehaviour
{
    private float power = 10f;

    private void Update()
    {
        transform.Rotate(Vector3.forward*power* Time.deltaTime);
    }
}
