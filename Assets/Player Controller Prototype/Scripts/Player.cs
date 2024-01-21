using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // string is name of item, int is amount in inv
    public Dictionary<string, int> items = new Dictionary<string, int>();
    [SerializeField] public ItemDataBase consumables;
        
    // Start is called before the first frame update
    void Start()
    {
        items.Add("smoke_bomb", 3);
        print(items["smoke_bomb"]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
