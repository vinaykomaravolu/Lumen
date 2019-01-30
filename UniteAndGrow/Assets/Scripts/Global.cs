using System;
using System.Collections.Generic;
using UnityEngine;

public static class Global{
    
    public const string sizeChangerTag = "SizeChanger";
    public const string playerTag = "Player";
    public const string endPointTag = "EndPoint";
    public const string killZoneTag = "KillZone";
    public const string collectibleTag = "Collectible";
    
    public const string mainMenuName = "mainMenu";
    
    public const string jumpButton = "Jump";
    public const string moveHorizontalButton = "Horizontal";
    public const string moveVerticalButton = "Vertical";
    
    public static GameControl gameControl;
    
    public static Dictionary<String, bool> collectionStatus;

    public static readonly string[] validCollectionNames = {
        "Collectible 1-1",
        "Collectible 2-1"
    };
    
    public static void loadCollectionStatus(){
        collectionStatus = new Dictionary<string, bool>();
        foreach (string name in validCollectionNames){
            collectionStatus[name] = PlayerPrefs.HasKey(name) && PlayerPrefs.GetInt(name) == 1;
        }
    }

    public static void saveCollectionStatus(){
        foreach (string name in validCollectionNames){
            PlayerPrefs.SetInt(name, collectionStatus[name] ? 1 : 0);
        }
    }
}