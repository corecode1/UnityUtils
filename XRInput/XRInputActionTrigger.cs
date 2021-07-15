using System;
using UnityEngine.XR;

public class XRInputActionTrigger
{
    public Action OnTriggered;
    public Action OnThisFrameTriggered;
    public Action OnUntriggered;

    private readonly InputDevice _device;
    private readonly Predicate<InputDevice> inputPredicate;
    
    public bool LastFrameDown {get; private set; }
    public bool IsDown {get; private set; }
    public bool ThisFrameDown {get; private set; }
    
    public XRInputActionTrigger(InputDevice device, Predicate<InputDevice> inputPredicate)
    {
        _device = device;
        this.inputPredicate = inputPredicate;
    }

    // Should be called once per frame.
    public void Update()
    {
        IsDown = inputPredicate.Invoke(_device);
        ThisFrameDown = false;

        if (IsDown)
        {
            OnTriggered?.Invoke();

            if (!LastFrameDown)
            {
                ThisFrameDown = true;
                OnThisFrameTriggered?.Invoke();
            }
        }
        else if (LastFrameDown)
        {
            OnUntriggered?.Invoke();
        }

        LastFrameDown = IsDown;
    }
}
