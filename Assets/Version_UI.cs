using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Version_UI : MonoBehaviour
{
    public TMP_Text ver;
    public GameObject updater;
    [HideInInspector]
    public string current_version = "";
    // Start is called before the first frame update
    void Start()
    {
        current_version = updater.GetComponent<Updater>().current_version;
    }

    // Update is called once per frame
    void Update()
    {
        ver.text = "v" + current_version;
    }
}
