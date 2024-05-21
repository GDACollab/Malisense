using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JonahDumbFix : MonoBehaviour
{

    public PlayerInventory playerInventory;
    public Artifact artifactToGive;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory.artifact2 = artifactToGive;
    }
}
