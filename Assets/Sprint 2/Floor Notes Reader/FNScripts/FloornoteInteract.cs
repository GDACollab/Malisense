using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FloornoteInteract : MonoBehaviour
{
    public bool FNInRange;
    public KeyCode FNinteractKey;
    public UnityEvent FNinteractAction;
    public GameObject popUpFloorNote;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FNInRange = true;
            Debug.Log("Player in range of Floornote");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FNInRange = false;
            Debug.Log("Player NO LONGER in range of Floornote");
        }
    }
    void Update()
    {
        if(FNInRange)
        {
            if(Input.GetKeyDown(FNinteractKey)) 
            {
                popUpFloorNote.SetActive(true);
                FNinteractAction.Invoke();
                Debug.Log("Player interacted with Floornote");
            }
        }
 
    }

    
}
