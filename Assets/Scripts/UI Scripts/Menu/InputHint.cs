using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;

public class InputHint : MonoBehaviour
{
    private TMP_Text inputHint;
    private DeviceInputs deviceInputs;
    private string originalText;

    // Start is called before the first frame update
    void Awake()
    {
        deviceInputs = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>().deviceInputs;
        inputHint = GetComponent<TMP_Text>();
        originalText = inputHint.text;
    }

    private void OnEnable()
    {
        SetInput();
        deviceInputs.OnDeviceChange.AddListener(SetInput);
    }

    private void OnDisable()
    {
        deviceInputs.OnDeviceChange.RemoveListener(SetInput);
    }

    public void SetInput()
    {
        inputHint.text = originalText;
        foreach (Match match in Regex.Matches(inputHint.text.ToLower(), @"\{(.*?)\}"))
        {
            inputHint.text = inputHint.text.Replace(match.ToString(), deviceInputs.GetInput(match.ToString()));
        }

    }
}
