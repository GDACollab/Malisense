using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsenseUse : MonoBehaviour
{
    //No scent system is in place, replace the placeholder vairable with it when implemented
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<ScentDetection>().usedIncense = true;
    }
}
