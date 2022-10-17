using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class PersistenceData : MonoBehaviour
{
    public static PersistenceData instance;

    public float bestTime;

    private void Awake()
    {
        if (PersistenceData.instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        Load();
    }

    [System.Serializable]
    class SaveData
    {
        public float bestTime;
    }

    public void Save()
    {
        SaveData data = new SaveData();
        data.bestTime= bestTime;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestTime = data.bestTime;
        }
    }
}
