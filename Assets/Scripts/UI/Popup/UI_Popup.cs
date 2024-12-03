using UnityEngine;

public abstract class UI_Popup : UI_Base
{
    public bool IsActive { get; private set; }

    public override void Active()
    {
        base.Active();
        IsActive = true;
    }

    public override void Deactive()
    {
        IsActive = false;
        base.Deactive();
    }
}
