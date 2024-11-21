using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : BaseState
{
    public PlayerController player;

    public PlayerState(PlayerController player)
    {
        this.player = player;
    }
}
