using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

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
    // Start is called before the first frame update
    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        if (!Directory.Exists(currentSceneName + ".txt"))
        {
            using (StreamWriter sw = File.CreateText(currentSceneName + ".txt"))
            {
                sw.WriteLine("HeatMapData: X   Z");
            }
        }
        player = GameObject.FindGameObjectWithTag("Player");
        currentCount = 0;
        maxCount = 60 / LogsPerSecond;
    }

    // Update is called once per frame
    void Update()
    {
        currentCount++;
        if (currentCount == maxCount)
        {
            using (StreamWriter sw = File.AppendText(currentSceneName + ".txt"))
            {
                Debug.Log("Write New Data");
                if (player.transform.position.x == 0)
                    playerX = 0.0.ToString();
                else
                     playerX = player.transform.position.x.ToString().Substring(0, 4);
                if (player.transform.position.z == 0)
                    playerZ = 0.0.ToString();
                else
                    playerZ = player.transform.position.z.ToString().Substring(0, 4);
                sw.WriteLine(playerX + "," + playerZ);
                currentCount = 0;
            }
        }
    }
    private void OnApplicationQuit()
    {
        using (StreamWriter sw = File.AppendText(currentSceneName + ".txt"))
            sw.Close();
        //TO.DO
        //Add loading to Google Drive part
    }
    public void CloseFile()
    {
        using (StreamWriter sw = File.AppendText(currentSceneName + ".txt"))
        {
            sw.Close();
        }
    }
}
