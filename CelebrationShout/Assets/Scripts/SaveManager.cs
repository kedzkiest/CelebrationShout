using System;
using System.IO;
using UnityEngine;

public class SaveManager
{
    // Make this class Singleton
    private static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveManager();
            }

            return instance;
        }
    }

    readonly string SAVEDATA_PATH = Application.persistentDataPath + "/savedata" + ".json";

    class SaveData
    {
        // The time span between celebration announcement and correct player input.
        // Smaller is better.
        public float quickestShoutTime;
    }

    private SaveData saveData;

    public void Initialize()
    {
        saveData = new SaveData();
        Load();
    }

    public void Save()
    {
        // Serialize Obj -> json
        string jsonSaveData = JsonUtility.ToJson(saveData);

        using(StreamWriter sw = new StreamWriter(SAVEDATA_PATH, false))
        {
            try
            {
                sw.Write(jsonSaveData);
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public void Load()
    {
        try
        {
            using (FileStream fs = new FileStream(SAVEDATA_PATH, FileMode.Open))
            {
                using(StreamReader sr = new StreamReader(fs))
                {
                    string result = sr.ReadToEnd();
                    Debug.Log(result);

                    saveData = JsonUtility.FromJson<SaveData>(result);
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
}
