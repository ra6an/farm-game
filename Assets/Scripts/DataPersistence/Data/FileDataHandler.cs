using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";

    private string dataFileName = "";

    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "gudra";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(string profileId)
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        GameData loadedData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //Decrypt data before deserializing it
                if(useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                //Deserialize data
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            } 
            catch (Exception e) 
            {
                Debug.LogError("Error occured when trying to load data fom file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data, string profileId)
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        try
        {
            //Create dir the file will be written to if doesn't alreadt exist!
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //serialize the C# gameData object into json
            string dataToStore = JsonUtility.ToJson(data, true);

            //Encrypt data if it's required
            if(useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            } 

            //write the srialized data to file
            using (FileStream stream = new(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        } 
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();

        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            Debug.Log(dirInfo.Name);
            string profileId = dirInfo.Name;

            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if(!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it doesnt contain data: " + profileId);
                continue;
            }

            GameData profileData = Load(profileId);

            if(profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            } else
            {
                Debug.LogError("Tried to load profile but something went wrong! Profile ID: " + profileId);
            }
        }

        return profileDictionary;
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }

        return modifiedData;
    }
 }
