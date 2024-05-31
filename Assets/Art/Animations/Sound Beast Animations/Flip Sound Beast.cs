using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class FlipSoundBeast : MonoBehaviour
{
    // simply checks the whether the transform is moving left or right
    // using the x position, then rotates the model on the y axis accordingly

    private float oldPositionx;
    private float newPositionx;

    private void Start()
    {
        oldPositionx = transform.position.x;
    }

    private void Update()
    {
        newPositionx = transform.position.x;
        if (newPositionx > oldPositionx) // moving right
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (newPositionx < oldPositionx) // moving left
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        oldPositionx = newPositionx;
    }
}
