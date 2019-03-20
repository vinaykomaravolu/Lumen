using UnityEngine;

public static class Global{

    public static readonly bool isMac = Application.platform == RuntimePlatform.OSXPlayer 
                               || Application.platform == RuntimePlatform.OSXEditor;
    
    public const string playerTag = "Player";
    public const string endPointTag = "Finish";
    public const string killZoneTag = "KillZone";
    public const string collectibleTag = "Collectible";
    public const string groundTag = "Ground";
    public const string sizeChangerTag = "SizeChanger";
    public const string superJumpTag = "SuperJump";
    public const string wallTag = "Wall";
    public const string instantKillTag = "InstantKill";
    
    public const string mainMenuName = "Main Menu";
    public const string tutorialName = "Tutorial Level";
    public const string levelOneName = "Level 1";
    public const string loadingSceneName = "Loading Scene";
    
    public const string jumpButton = "Jump";
    public const string moveHorizontalButton = "Horizontal";
    public const string moveVerticalButton = "Vertical";
    public const string pauseButton = "Cancel";
    public const string confirmButton = "Submit";
    public const string camHorizontalMouse = "Mouse X";
    public const string camVerticalMouse = "Mouse Y";
    public const string dashButton = "Dash";
    public static readonly string camHorizontalStick = "Camera X " + (isMac ? "Mac" : "Win");
    public static readonly string camVerticalStick = "Camera Y " + (isMac ? "Mac" : "Win");
    public static readonly string altPauseButton = "Cancel " + (isMac ? "Mac" : "Win");

    public static float gravity;
    
    public static GameControl gameControl;
    public static SoundControl soundControl;
}