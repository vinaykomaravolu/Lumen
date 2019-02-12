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
    public const string mushroomTag = "Mushroom";
    
    public const string mainMenuName = "mainMenu";
    
    public const string jumpButton = "Jump";
    public const string moveHorizontalButton = "Horizontal";
    public const string moveVerticalButton = "Vertical";
    public const string pauseButton = "Cancel";
    public const string confirmButton = "Submit";
    public const string camHorizontalMouse = "Mouse X";
    public const string camVerticalMouse = "Mouse Y";
    public static readonly string camHorizontalStick = "Camera X " + (isMac ? "Mac" : "Win");
    public static readonly string camVerticalStick = "Camera Y " + (isMac ? "Mac" : "Win");

    public static float gravity;
    
    public static GameControl gameControl;
    public static SoundControl soundControl;
}