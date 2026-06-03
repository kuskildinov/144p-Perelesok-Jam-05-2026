using UnityEngine;

public static class GlobalVars
{
    public static int LightRadius = 3;
    [Header("Level Scenes Names")]
    public static string MainMenuSceneName = "MainMenu";
    public static string Level_0_Name = "Level_0";
    public static string Level_1_Name = "Level_1";
    public static string Level_2_Name = "Level_2";
    public static string Level_3_Name = "Level_3";
    public static string Level_4_Name = "Level_4";
    [Header("Saved Data")]
    public static string CurrentStartedSceneSaveKey = "CurrentSceneIndex";
    public static int CurrentStartedSceneIndex = 0;
}

public class PlayerData
{

}
