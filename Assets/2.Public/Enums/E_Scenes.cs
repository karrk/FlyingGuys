// 씬 관련 Enum type

public enum E_Scenes
{
    None = -1,
    Open,
    Login,
    Loading,
    Menu,
    Game,
    Result,
    
    Size
}

// 플레이어 상태 관련 Enum type
public enum E_PlayeState
{
    Idle,
    Run,
    Jump,
    Fall,
    Diving,
    FallingImpact,
    StandUp,
    Bounced,
    Grabbing,
    Grabbed,
    Size
}

public enum E_AniParameters
{
    Idling, // bool
    Running, // bool
    Jumping,
    Falling,
    Diving,
    FallingImpact,
    StandingUp,
    Bouncing,
    Pushing, // bool
    Pulling, // bool
    Struggling, // bool
    Size
}