using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;
using TeaCup = GlobalTeapot.TeaCup;

[CreateAssetMenu(fileName = "DeviceInputs", menuName = "Device Inputs")]
public class DeviceInputs : ScriptableObject
{
    [System.Serializable]
    public class DeviceInput
    {
        public string this[TeaCup key]
        {
            get
            {
                switch (key)
                {
                    case TeaCup.KEYBOARD:
                        return keyboard;
                    case TeaCup.XINPUT:
                        return xInput;
                    case TeaCup.DUALSHOCK:
                        return dualShock;
                    case TeaCup.SWITCH:
                        return switchPro;
                    default:
                        return keyboard;
                }
            }
        }

        public string inputType;
        [SerializeField] private string keyboard;
        [SerializeField] private string xInput;
        [SerializeField] private string dualShock;
        [SerializeField] private string switchPro;
    }

    public TeaCup currDevice
    {
        get { return _currDevice; }
        set
        {
            _currDevice = value;
            OnDeviceChange?.Invoke();
        }
    }

    private TeaCup _currDevice = TeaCup.KEYBOARD;
    public UnityEvent OnDeviceChange;
    [SerializeField] private List<DeviceInput> inputs = new List<DeviceInput>();

    public string GetInput(string inputType)
    {
        return inputs.Find(x => x.inputType == inputType)[currDevice];
    }

    public void Init()
    {
        InputSystem.onEvent += SetInputDevice;
    }

    private void SetInputDevice(InputEventPtr eventPtr, InputDevice device)
    {
        if (device is Keyboard && currDevice != TeaCup.KEYBOARD)
        {
            currDevice = TeaCup.KEYBOARD;
        }
        if (device is SwitchProControllerHID && currDevice != TeaCup.SWITCH)
        {
            currDevice = TeaCup.SWITCH;
        }
        if (device is XInputController && currDevice != TeaCup.XINPUT)
        {
            currDevice = TeaCup.XINPUT;
        }
        if (device is DualShockGamepad && currDevice != TeaCup.DUALSHOCK)
        {
            currDevice = TeaCup.DUALSHOCK;
        }
    }

    public void Deactivate()
    {
        InputSystem.onEvent -= SetInputDevice;
    }
}
