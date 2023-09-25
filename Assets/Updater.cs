using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using UnityEngine;

public class Updater : MonoBehaviour
{
    public string Updater_version_url = "";
    public string Updater_zip_url = "";
    [HideInInspector]
    public string current_version = "";
    private string latest_version = "";
    // Start is called before the first frame update
    void Start()
    {
        if (Updater_version_url != "" && Updater_zip_url != "")
        {
            if (File.Exists("version.txt"))
            {
                string[] lines = File.ReadAllLines("version.txt");
                current_version = lines[0];
            }
            else
            {
                current_version = "0.0.0";
            }
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(Updater_version_url, "ver.txt");
            }
            if (File.Exists("ver.txt"))
            {
                string[] lines = File.ReadAllLines("ver.txt");
                latest_version = lines[0];
            }
            else
            {
                UnityEngine.Debug.LogError("SERVER IS DOWN!");
                latest_version = "0.0.0";
            }
            if (current_version == latest_version)
            {
                UnityEngine.Debug.Log("Uptodate!");
            }
            else
            {
                string[] lines = { Updater_version_url, Updater_zip_url };
                File.WriteAllLines("sv.s", lines);
                Process.Start("Updater.exe");
                Application.Quit();
            }
        }
        else
        {
            UnityEngine.Debug.LogError("No URL was given to version and zip!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
