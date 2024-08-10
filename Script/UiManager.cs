using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject Playpanel;
    public GameObject Homepanel;

    public void Start()
    {
        Closeallpanel();
        Homepanel.SetActive(true);

    }
    
    public void Closeallpanel()
    {
       // SoundManager.instance.BTNClick();
        Playpanel.SetActive(false);
        Homepanel.SetActive(false);
    }

    public void PlayBtn()
    {
        SoundManager.instance.RightClickbtn();
        Closeallpanel();
        Playpanel.SetActive(true);
    }
    public void HomeBtn()
    {
        SoundManager.instance.RightClickbtn();
        Closeallpanel();
        Homepanel.SetActive(true);
    }
}
