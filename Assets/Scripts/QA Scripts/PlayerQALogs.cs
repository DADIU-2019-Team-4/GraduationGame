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
            int xLenght = player.transform.position.x.ToString().Length;
            int zLenght = player.transform.position.z.ToString().Length;
            if (xLenght < 4)
                playerX = player.transform.position.x.ToString().Substring(0, xLenght-1);
            else
                playerX = player.transform.position.x.ToString().Substring(0, 4);
            if (zLenght<4)
                playerZ = player.transform.position.z.ToString().Substring(0, zLenght - 1);
            else
                playerZ = player.transform.position.z.ToString().Substring(0, 4);
            currentCount = 0;
            WritePosition(playerX + "," + playerZ);
        }
    }
    private void WritePosition(string message)
    {
        Debug.Log("Text to write:" + message);
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
    public void Close()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        ClosePC();
#elif UNITY_ANDROID || UNITY_IOS
        CloseMobile();
#endif
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
        mobilePath = Application.persistentDataPath + "/" + currentSceneName + ".txt";
        streamWriter = File.CreateText(mobilePath);
        File.SetAttributes(mobilePath, FileAttributes.Normal);
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
    private void ClosePC()
    {
        using (StreamWriter sw = File.AppendText(currentSceneName + ".txt"))
        {
            sw.Close();
        }
    }
    private void CloseMobile()
    {
        streamWriter.Close();
    }
}
