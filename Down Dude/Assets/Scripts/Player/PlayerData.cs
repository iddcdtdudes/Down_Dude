using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public uint m_version;
    public bool m_tutorialChunk;
    public int m_coins;
    public int m_playTime;
    public int m_usingSkin;
    public int m_allTimeCP;
    public float m_allTimeDist;
    public bool[] m_unlockedSkins;
    public bool[] m_unlockedAchievements;
    public bool[] m_achievementClaimed;

    //Constructor for saving and loading
    public PlayerData (uint playerData_Version, PlayerData player, int numberOfSkins, int numberOfAchievements)
    {
        m_version = playerData_Version;
        m_tutorialChunk = player.m_tutorialChunk;
        m_coins = player.m_coins;
        m_playTime = player.m_playTime;
        m_usingSkin = player.m_usingSkin;
        m_allTimeCP = player.m_allTimeCP;
        m_allTimeDist = player.m_allTimeDist;

        m_unlockedSkins = new bool[numberOfSkins];
        player.m_unlockedSkins.CopyTo(m_unlockedSkins, 0);

        m_unlockedAchievements = new bool[numberOfAchievements];
        player.m_unlockedAchievements.CopyTo(m_unlockedAchievements, 0);

        m_achievementClaimed = new bool[numberOfAchievements];
        player.m_achievementClaimed.CopyTo(m_achievementClaimed, 0);

        //Check if number of skins have changed
        //if (player.m_unlockedSkins.Length == numberOfSkins)
        //{
        //    //Debug.Log("Number of skin is the same");

        //    m_unlockedSkins = new bool[numberOfSkins];
        //    player.m_unlockedSkins.CopyTo(m_unlockedSkins, 0);
        //    m_unlockedSkins[0] = true;
        //    m_unlockedSkins[1] = true;
        //    m_unlockedSkins[2] = true;
        //    //if (m_unlockedSkins.Length > 1)
        //    //{
        //    //    for (int i = 1; i < player.m_unlockedSkins.Length; i++)
        //    //    {
        //    //        m_unlockedSkins[i] = true;
        //    //        //m_unlockedSkins[i] = player.m_unlockedSkins[i];                                               need to be change back
        //    //    }
        //    //}
            
        //}
        //else
        //{
        //    //Debug.Log("Number of skins have changed");
            
        //    //If more skins is added
        //    m_unlockedSkins = new bool[numberOfSkins]; //Update number of skins

        //    //Copy input player array
        //    player.m_unlockedSkins.CopyTo(m_unlockedSkins, 0);

        //    //Unlocked default skin
        //    m_unlockedSkins[0] = true;
        //    //bool[] tmp = new bool[m_unlockedSkins.Length];

        //    //m_unlockedSkins.CopyTo(tmp, 0);

        //    //m_unlockedSkins = new bool[numberOfSkins];

        //    ////Set other unlocked skin
        //    //if (numberOfSkins > 1)
        //    //{
        //    //    for (int i = 0; i < tmp.Length; i++)
        //    //    {
        //    //        m_unlockedSkins[i] = tmp[i];
        //    //    }

        //    //    for (int j = tmp.Length; j < numberOfSkins; j++)
        //    //    {
        //    //        //m_unlockedSkins[j] = false;                                                           need to be changed back
        //    //        m_unlockedSkins[j] = true;
        //    //    }
        //    //}

        //}

        //if (player.m_unlockedAchievements.Length == numberOfAchievements)
        //{
        //    m_unlockedAchievements = new bool[numberOfAchievements];

        //    player.m_unlockedAchievements.CopyTo(m_unlockedAchievements, 0);
        //}
        //else
        //{
        //    m_unlockedAchievements = new bool[numberOfAchievements];

        //    player.m_unlockedAchievements.CopyTo(m_unlockedAchievements, 0);
            

        //}

    }

    //Constructor creating the object first time
    public PlayerData (uint playerData_Version, int numberOfSkins, int numberOfAchievements)
    {
        m_version = playerData_Version;
        m_tutorialChunk = false;
        m_coins = 0;
        m_playTime = 0;
        m_usingSkin = 0;
        //m_allTimeHS = 0;
        m_allTimeCP = 0;
        m_allTimeDist = 0.0f;
        m_unlockedSkins = new bool[numberOfSkins]; //Defaults is false
        m_unlockedAchievements = new bool[numberOfAchievements];
        m_achievementClaimed = new bool[numberOfAchievements];

        m_unlockedSkins[0] = true;

        if (!PlayerPrefs.HasKey(PlayerDataManager.instance.m_playerPref_Control))
        {
            PlayerPrefs.SetInt(PlayerDataManager.instance.m_playerPref_Control, 0);
        }

        if (!PlayerPrefs.HasKey(PlayerDataManager.instance.m_playerPref_Music))
        {
            PlayerPrefs.SetInt(PlayerDataManager.instance.m_playerPref_Music, 1);
        }

    }
}

public static class SaveLoadManager
{

    static string path = Application.persistentDataPath + "/DownDude.sav";

    public static void SaveData (uint playerData_Version, PlayerData player, int numberOfSkins, int numberOfAchievements)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(path , FileMode.Create);

        PlayerData saveData = new PlayerData(playerData_Version , player, numberOfSkins, numberOfAchievements);

        bf.Serialize(stream, saveData);
        stream.Close();

        //Debug.Log("Saves file created.");

    }

    //Return deserialized PlayerData or null
    public static PlayerData LoadData (uint playerData_Version, int numberOfSkins, int numberOfAchievements)
    {
        //Check if path exist
        if (File.Exists(path))
        {
            //Initialize Formatter and File reading stream
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path , FileMode.Open);
            //Declare new variable to keep loaded data
            PlayerData loadedData;

            //Check if file is empty
            if (stream.Length == 0)
            {
                //Create new save data
                loadedData = new PlayerData(playerData_Version ,numberOfSkins, numberOfAchievements);
            }
            else
            {
                //Load save data
                loadedData = (PlayerData)bf.Deserialize(stream);
                
                if (loadedData.m_version != playerData_Version)
                {
                    Debug.Log("Different version of save data: " + loadedData.m_version);
                    switch (loadedData.m_version)
                    {
                        default:
                            loadedData = new PlayerData(playerData_Version, numberOfSkins, numberOfAchievements);
                            break;
                    }
                }
                else
                {
                    Debug.Log("Same version of save data: " + playerData_Version);
                    loadedData = new PlayerData(playerData_Version, loadedData, numberOfSkins, numberOfAchievements);
                }

                stream.Close();
            }

            return loadedData;
        }
        //Create new save files
        else
        {
            //Debug.LogError("No save files");
            return new PlayerData(playerData_Version ,numberOfSkins, numberOfAchievements);
        }

    }
}

