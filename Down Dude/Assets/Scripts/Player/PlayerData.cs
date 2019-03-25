using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public int m_coins;
    public int m_allTimeHS;
    public int m_allTimeCP;
    public bool[] m_unlockedSkins;
    //private bool[] m_unlockedAchievements;

    //Constructor for saving and loading
    public PlayerData (PlayerData player, int numberOfSkins)
    {
        m_coins = player.m_coins;
        m_allTimeHS = player.m_allTimeHS;
        m_allTimeCP = player.m_allTimeCP;

        //Check if number of skins have changed
        if (player.m_unlockedSkins.Length == numberOfSkins)
        {
            for (int i = 0; i < player.m_unlockedSkins.Length; i++)
            {
                m_unlockedSkins = new bool[numberOfSkins];
                m_unlockedSkins[i] = player.m_unlockedSkins[i];
            }
        }
        else
        {
            Debug.Log("Number of skins have changed");
            
            //If more skins is added
            m_unlockedSkins = new bool[numberOfSkins]; //Update number of skins

            //Unlocked default skin
            m_unlockedSkins[0] = true;

            //Set other unlocked skin
            if (numberOfSkins > 1)
            {
                for (int i = 1; i < numberOfSkins; i++)
                {
                    Debug.Log("Skin Index = " + i);
                    m_unlockedSkins[i] = player.m_unlockedSkins[i];
                }
            }
            

        }

    }

    //Constructor creating the object first time
    public PlayerData (int numberOfSkins)
    {
        m_coins = 0;
        m_allTimeHS = 0;
        m_allTimeCP = 0;
        m_unlockedSkins = new bool[numberOfSkins]; //Defaults is false
        //m_unlockedSkins[0] = true; //Setting default skins to true
    }

    /*
    
    #region Setter

    public void SetMoney (int money)
    {
        m_coins = money;
    }

    public void SetUnlockedSkin(int skinID)
    {
        m_unlockedSkins[skinID] = true;
    }

    #endregion

    #region Getter

    public int GetMoney ()
    {
        return m_coins;
    }

    public bool GetUnlockedSkins (int SkinID)
    {
        return m_unlockedSkins[SkinID];
    }

    #endregion

    #region Adjusting

    public void AddMoney (int moneyAmount)
    {
        m_coins += moneyAmount;
    }

    public void SubtractMoney (int moneyAmount)
    {
        m_coins -= moneyAmount;
    }

    #endregion
    
    */
}

public static class SaveLoadManager
{

    static string path = Application.persistentDataPath + "/DownDude.save";

    public static void SaveData (PlayerData player, int numberOfSkins)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(path , FileMode.Create);

        PlayerData saveData = new PlayerData(player, numberOfSkins);

        bf.Serialize(stream, saveData);
        stream.Close();

        //Debug.Log("Saves file created.");

    }

    //Return deserialized PlayerData or null
    public static PlayerData LoadData (int numberOfSkins)
    {
        
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path , FileMode.Open);
            PlayerData loadedData;

            //Check if file is empty
            if (stream.Length == 0)
            {
                loadedData = new PlayerData(numberOfSkins);
            }
            else
            {
                loadedData = (PlayerData)bf.Deserialize(stream);
                stream.Close();
            }

            return loadedData;
        }
        else
        {
            //Debug.LogError("No save files");
            return new PlayerData(numberOfSkins);
        }

    }
}

