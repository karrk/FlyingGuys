using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerModel
{
    public int playerNumber;
    public float moveSpeed;
    public float maxSpeed;
    public float maxAccel;
    public float moveSpeedInAir;
    public float jumpForce;
    public float divingForce;
    public float grabRadius;
    public float grabForce;


    public event Action OnPlayerJumped;
    public event Action OnPlayerDove;
    public event Action OnPlayerFloorImpacted;
    public event Action OnPlayerGrabbingObject;
    public event Action OnPlayerGrabbed;
    public event Action OnPlayerBounced;

    public void InvokePlayerJumped()
    {
        OnPlayerJumped?.Invoke();
    }

    public void InvokePlayerDove()
    {
        OnPlayerDove?.Invoke();
    }

    public void InvokePlayerFloorImpacted()
    {
        OnPlayerFloorImpacted?.Invoke();
    }

    public void InvokePlayerGrabbing()
    {
        OnPlayerGrabbingObject?.Invoke();
    }

    public void InvokePlayerGrabbed()
    {
        OnPlayerGrabbed?.Invoke();
    }

    public void InvokePlayerBounced()
    {
        OnPlayerBounced?.Invoke();
    }
}
