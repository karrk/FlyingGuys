public enum E_SoundType
{
    Master,
    BGM,
    SFX,
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

    BG_GamePlay_01_Start,
    BG_GamePlay_01_LoopOne,
    BG_GamePlay_01_LoopTwo,

    BG_GamePlay_02_Start,
    BG_GamePlay_02_Loop,

    BG_GamePlay_03_Start,
    BG_GamePlay_03_Loop,

    BG_GamePlay_04_Start,
    BG_GamePlay_04_Loop,

    Size,
}

// 해당 enum 순서 변경을 금지합니다.
public enum E_SFX
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