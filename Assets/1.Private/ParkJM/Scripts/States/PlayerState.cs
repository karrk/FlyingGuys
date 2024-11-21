using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : BaseState
{
    public BeforePlayerController player;

    public PlayerState(BeforePlayerController player)
    {
        this.player = player;
    }
}
