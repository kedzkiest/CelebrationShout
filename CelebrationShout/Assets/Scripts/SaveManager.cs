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

    // Implementation with json, which was not successful in WebGL build.
    // readonly string SAVEDATA_PATH = Application.persistentDataPath + "/savedata" + ".json";

    class SaveData
    {
        // The time span between celebration announcement and correct player input.
        // Smaller is better.
        public float quickestShoutTime;
    }

    private SaveData saveData;

    const string QUICKEST_SHOUT_TIME = "QUICKEST_SHOUT_TIME";

    // Implementation with json, which was not successful in WebGL build.
    // public float GetQuickestShoutTime() { return saveData.quickestShoutTime; }
    public float GetQuickestShoutTime()
    {
        return PlayerPrefs.GetFloat(QUICKEST_SHOUT_TIME, float.MaxValue);
    }

    // Implementation with json, which was not successful in WebGL build.
    // public void SetQuickestShoutTime(float _time) { saveData.quickestShoutTime = _time; }
    public void SetQuickestShoutTime(float _time)
    {
        PlayerPrefs.SetFloat(QUICKEST_SHOUT_TIME, _time);
    }


    public void Initialize()
    {
        saveData = new SaveData();
        Load();
    }

    public void Save()
    {
        return;

        // Implementation with json, which was not successful in WebGL build.

        /*
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

            Debug.Log("Saved: " + jsonSaveData);
        }
        */
    }

    public void Load()
    {
        return;

        // Implementation with json, which was not successful in WebGL build.

        /*
        try
        {
            using (FileStream fs = new FileStream(SAVEDATA_PATH, FileMode.Open))
            {
                using(StreamReader sr = new StreamReader(fs))
                {
                    string result = sr.ReadToEnd();
                    Debug.Log("Loaded: " + result);

                    saveData = JsonUtility.FromJson<SaveData>(result);

                    if(result == "")
                    {
                        Debug.Log("No data loaded. New one created.");
                    }

                    // Since float value is initialized to 0.0f, Change the initial value to
                    // float.MaxValue so that players can update the best score.
                    if (saveData.quickestShoutTime <= 0.0f)
                    {
                        saveData.quickestShoutTime = float.MaxValue;
                    }
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);

            // In the case read file is broken, newly create an instance of SaveData,
            // then save it to file, then load it so that we can modify its content later (normal system).
            saveData = new SaveData();
            Save();
            Load();
        }
        */
    }

    public void ResetSaveData()
    {
        // Implementation with json, which was not successful in WebGL build.
        // saveData.quickestShoutTime = float.MaxValue;

        PlayerPrefs.SetFloat(QUICKEST_SHOUT_TIME, float.MaxValue);
    }
}
