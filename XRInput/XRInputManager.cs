using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRInputManager : MonoBehaviour 
{
	private XRInputActionTrigger _controllerDownTrigger;
	private XRInputActionTrigger _controllerGripTrigger;
	private List<XRInputActionTrigger> _inputTriggers;

	private void Start()
	{
		_inputTriggers = new List<XRInputActionTrigger>();
		List<InputDevice> controllers = new List<InputDevice>();
		InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, controllers);

		if (controllers.Count >= 1)
		{
			InitRightControllerTriggers(controllers[1]);
		}
		else
		{
			Debug.LogError("Right controller is not enabled");
		}
	}

	private void InitRightControllerTriggers(InputDevice controller)
	{
		_controllerDownTrigger = new XRInputActionTrigger(controller, ControllerTriggerPredicate);
		_controllerGripTrigger = new XRInputActionTrigger(controller, ControllerGripPredicate);
			
		_inputTriggers.Add(_controllerDownTrigger);
		_inputTriggers.Add(_controllerGripTrigger);

		_controllerDownTrigger.OnThisFrameTriggered += OnTriggerDown;
		_controllerDownTrigger.OnUntriggered += OnTriggerRelease;
	}

	private void OnTriggerDown()
	{
		Debug.Log("Down");
	}

	private void OnTriggerRelease()
	{
		Debug.Log("Release");
	}

	private bool ControllerTriggerPredicate(InputDevice device)
	{
		device.TryGetFeatureValue(CommonUsages.trigger, out float pressedAmount);
		return pressedAmount > 0.7;
	}

	private bool ControllerGripPredicate(InputDevice device)
	{
		device.TryGetFeatureValue(CommonUsages.gripButton, out bool gripButtonIsPressed);
		return gripButtonIsPressed;
	}

	private void Update()
	{
		foreach (XRInputActionTrigger trigger in _inputTriggers)
		{
			trigger.Update();
		}
	}
}