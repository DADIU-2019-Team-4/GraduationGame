using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class PlayerQALogs : MonoBehaviour
{
    public int LogsPerSecond; // As game aims to run at 60fps, means how many logs there should be during these 60 frames;
    private string _currentSceneName;
    private GameObject player;
    private int _currentCount;
    private int _maxCount;
    private string _playerX;
    private string _playerZ;
    private string _mobilePath;
    private StreamWriter _streamWriter;

    // Start is called before the first frame update
    void Start()
    {
        var index = SceneManager.sceneCount;
        _currentSceneName = SceneManager.GetSceneAt(index - 1).name;
        player = GameObject.FindGameObjectWithTag("Player");
        _currentCount = 0;
        _maxCount = 60 / LogsPerSecond;
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
        _currentCount++;
        if (_currentCount == _maxCount)
        {
            int xLenght = player.transform.position.x.ToString().Length;
            int zLenght = player.transform.position.z.ToString().Length;
            if (xLenght < 4)
                _playerX = player.transform.position.x.ToString().Substring(0, xLenght);
            else
                _playerX = player.transform.position.x.ToString().Substring(0, 4);
            if (zLenght<4)
                _playerZ = player.transform.position.z.ToString().Substring(0, zLenght);
            else
                _playerZ = player.transform.position.z.ToString().Substring(0, 4);
            _currentCount = 0;
            WritePosition(_playerX + "," + _playerZ);
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
        using (StreamWriter sw = File.AppendText(_currentSceneName + ".txt"))
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
        if (!Directory.Exists(_currentSceneName + ".txt"))
        {
            using (StreamWriter sw = File.CreateText(_currentSceneName + ".txt"))
            {
                sw.WriteLine("HeatMapData: X   Z");
            }
        }
    }
    private void MobileCreateFile()
    {
        _mobilePath = Application.persistentDataPath + "/" + _currentSceneName + ".txt";
        _streamWriter = File.CreateText(_mobilePath);
        File.SetAttributes(_mobilePath, FileAttributes.Normal);
        _streamWriter.WriteLine("HeatMapData: X   Z");

    }
    private void WriteToPC(string message)
    {
            using (StreamWriter sw = File.AppendText(_currentSceneName + ".txt"))
            {
                sw.WriteLine(message);
            }
    }
    public void WriteToMobile(string message)
        {
        _streamWriter.WriteLine(message);
        }
    private void ClosePC()
    {
        using (StreamWriter sw = File.AppendText(_currentSceneName + ".txt"))
        {
            sw.Close();
        }
    }
    private void CloseMobile()
    {
        _streamWriter.Close();
    }
}
