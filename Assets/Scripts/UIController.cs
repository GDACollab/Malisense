using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField]
    private FNRPanel _fnr;

    public FNRPanel FNRWindow => _fnr;

    private void Awake()
    {
        Instance = this;
    }
}
