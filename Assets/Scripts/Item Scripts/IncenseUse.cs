using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsenseUse : MonoBehaviour
{
    //No scent system is in place, replace the placeholder vairable with it when implemented
    public scr_noise noiseMaker;
    public float noiseSize = 15;
    public float noiseInterval = 1;
    private float noiseDuration;

    // Start is called before the first frame update
    void Start()
    {
        var temp = FindObjectOfType<ScentDetection>();
        temp.usedIncense = true;
        noiseDuration = temp.incenseDuration;
        
        StartCoroutine(MakeNoise());
    }
    
    IEnumerator MakeNoise(){
        int count = Mathf.RoundToInt(noiseDuration/noiseInterval);
        while(count > 0){
            count --;
            noiseMaker.MakeSound(transform.position, noiseSize);
            yield return new WaitForSeconds(noiseInterval);
        }
        Destroy(gameObject);
    }
}
