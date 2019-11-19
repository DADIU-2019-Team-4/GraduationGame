using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class PlayerQALogs : MonoBehaviour
{
    public int LogsPerSecond; // As game aims to run at 60fps, means how many logs there should be during these 60 frames;
    private string currentSceneName;
    private GameObject player;
    private int currentCount;
    private int maxCount;
    private StreamWriter sv;
    private string playerX;
    private string playerZ;
    private string mobilePath;
    private StreamWriter streamWriter;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        player = GameObject.FindGameObjectWithTag("Player");
        currentCount = 0;
        maxCount = 60 / LogsPerSecond;
        CreateFile();
    }
    private void CreateFile()
    { 
#if UNITY_EDITOR || UNITY_STANDALONE
        PcCreateFile();
#elif UNITY_ANDROID || UNITY_IOS
        MobileCreateFile();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        currentCount++;
        if (currentCount == maxCount)
        {
            if (player.transform.position.x == 0)
                playerX = 0.0.ToString();
            else
                playerX = player.transform.position.x.ToString().Substring(0, 4);
            if (player.transform.position.z == 0)
                playerZ = 0.0.ToString();
            else
                playerZ = player.transform.position.z.ToString().Substring(0, 4);
            currentCount = 0;
            WritePosition(playerX + "," + playerZ);
        }
    }
    private void WritePosition(string message)
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        WriteToPC(message);
#elif UNITY_ANDROID || UNITY_IOS
        WriteToMobile(message);
#endif
    }
    private void OnApplicationQuit()
    {
        using (StreamWriter sw = File.AppendText(currentSceneName + ".txt"))
            sw.Close();
    }
    public void CloseFile()
    {
        using (StreamWriter sw = File.AppendText(currentSceneName + ".txt"))
        {
            sw.Close();
        }
    }
    private void PcCreateFile()
    {
        if (!Directory.Exists(currentSceneName + ".txt"))
        {
            using (StreamWriter sw = File.CreateText(currentSceneName + ".txt"))
            {
                sw.WriteLine("HeatMapData: X   Z");
            }
        }
    }
    private void MobileCreateFile()
    {
        mobilePath = Application.persistentDataPath;
        string filewriter = mobilePath + "/" + currentSceneName + ".txt";
        streamWriter = File.CreateText(filewriter);
        streamWriter.WriteLine("HeatMapData: X   Z");

    }
    private void WriteToPC(string message)
    {
        using (StreamWriter sw = File.AppendText(currentSceneName + ".txt"))
        {
            sw.WriteLine(message);
        }
    }
    public void WriteToMobile(string message)
    {
        streamWriter.WriteLine(message);
    }
}
