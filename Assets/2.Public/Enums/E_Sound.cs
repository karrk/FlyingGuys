public enum E_SoundType
{
    Master,
    BGM,
    UISFX,
    StageSFX,
}

// 해당 enum 순서 변경을 금지합니다.
public enum E_BGM
{
    None = -1,
    //BG_Test_1,
    //BG_Test_2,

    BG_Login,
    BG_MainMenu,
    BG_Loading,
    BG_GameResult,

    BG_GamePlay_01,
    BG_GamePlay_02,
    BG_GamePlay_03,
    BG_GamePlay_04,

    Size,
}

// 해당 enum 순서 변경을 금지합니다.
public enum E_UISFX
{
    None = -1,
    S_Highlighted,
    S_Pressed,
    S_Exit,
    S_GamePlay,
    S_LoadingArrowLeft,
    S_LoadingArrowRight,

    S_Winner,
    S_Loser,

    Size,
}

// 해당 enum 순서 변경을 금지합니다.
public enum E_StageSFX
{
    S_Bounce,
    S_FloorImpact,
    S_Grabbed,
    S_Grabbing,
    S_Jump
}