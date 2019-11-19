using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerActionsCollectorQA : MonoBehaviour
{
    public DataContainer DataConteiner;
    private string fileName = "PlayersData.txt";
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Data");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        PcSaveData();
#elif UNITY_ANDROID || UNITY_IOS
        MobileSaveData();
#endif
    }
    void PcSaveData()
    {
        if (!Directory.Exists(fileName))
        {
            using (StreamWriter sw = File.CreateText(fileName))
                SetData(sw);
        }
    }
    void MobileSaveData()
    {
        var mobilePath = Application.persistentDataPath + "/" + fileName;
        StreamWriter streamWriter = File.CreateText(mobilePath);
        SetData(streamWriter);
    }
    void SetData(StreamWriter writer)
    {
        writer.WriteLine("Normal Dash Count:" + DataConteiner.NormalDashCount);
        writer.WriteLine("Charged Dash Count:" + DataConteiner.ChargedDashCount);
        writer.WriteLine("Destroyed Object Count:" + DataConteiner.DestroyedObjectsCount);
        writer.WriteLine("Ropes Use Count:" + DataConteiner.RopeUseCount);
        writer.WriteLine("Deaths Count:" + DataConteiner.DeathsCount);
        for(int i = 0; i<= DataConteiner.levelName.Capacity-1;i++)
            writer.WriteLine("Died in level: " + DataConteiner.levelName[i] +"    Player's death position:" + DataConteiner.deathPlace[i]);
        writer.Close();
        ClearData();
    }
    void ClearData()
    {
        DataConteiner.NormalDashCount = 0;
        DataConteiner.ChargedDashCount = 0;
        DataConteiner.DestroyedObjectsCount = 0;
        DataConteiner.RopeUseCount = 0;
        DataConteiner.DeathsCount = 0;
        DataConteiner.levelName.Clear();
        DataConteiner.deathPlace.Clear();
    }
}
