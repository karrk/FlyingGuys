using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
    public virtual void FixedUpdate() { }

    public virtual void LateUpdate() { }

    public virtual void PlayAnimation() { }
}
