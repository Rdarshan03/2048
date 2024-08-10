using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("----Sprite----")]
    public Sprite SoundOn;
    public Sprite SoundOff;
    public Sprite MusicOn;
    public Sprite MusicOff;



    [Header("----BTN----")]
    public Button SoundBtn;
    public Button MusicBtn;

    [Header("----Clip----")]
    public AudioClip BtnClickSound;
    public AudioClip RightClick;
    public AudioClip WrongClick;
    public AudioClip PlayClick;

    public AudioClip WinSound;
    public AudioClip BalloonSound;
    public AudioClip YellowLineSound;
    public AudioClip BoxSound;

    [Header("----AudioSource----")]
    public AudioSource audiosource;
    public AudioSource BGaudioSource;

    void Awake()
    {
        instance = this;

        if (!PlayerPrefs.HasKey("Sound"))
            PlayerPrefs.SetInt("Sound", 1);
        if (!PlayerPrefs.HasKey("Haptic"))
            PlayerPrefs.SetInt("Haptic", 1);
        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music", 1);
        Application.targetFrameRate = 300;
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundBtn.image.sprite = SoundOn;
            audiosource.Play();
        }
        else
        {
            SoundBtn.image.sprite = SoundOff;
            audiosource.Stop();
        }


        if (PlayerPrefs.GetInt("Music") == 1)
        {
            MusicBtn.image.sprite = MusicOn;
            BGaudioSource.Play();
        }
        else 
        {
            MusicBtn.image.sprite = MusicOff;
            BGaudioSource.Stop();
        }
          
    }

    void playsound(AudioClip clipofplay)
    {
    

        if (PlayerPrefs.GetInt("Sound") == 1)
            audiosource.PlayOneShot(clipofplay);
    }

    public void BTNClick()
    {
        playsound(BtnClickSound);
    }

    public void RightClickbtn()
    {
        playsound(RightClick);
    }

    public void WrongClickbtn()
    {
        playsound(WrongClick);
    }

    public void PlayClickBtn()
    {
        playsound(PlayClick);
    }
    public void YellowClick()
    {
        playsound(YellowLineSound);
    }

    public void BoxClick()
    {
        playsound(BoxSound);
    }

    public void BalloonClick()
    {
        playsound(BalloonSound);
    }

    public void WinClick()
    {
        playsound(WinSound);
    }

    #region BtnClick

    public void Sound()
    {

        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            SoundBtn.image.sprite = SoundOff;
            PlayerPrefs.SetInt("Sound", 0);
        }
        else
        {
            SoundBtn.image.sprite = SoundOn;
            PlayerPrefs.SetInt("Sound", 1);
        }
        BTNClick();
    }

    public void Music()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            BGaudioSource.Stop();
            MusicBtn.image.sprite = MusicOff;
            PlayerPrefs.SetInt("Music", 0);
        }
        else
        {
            BGaudioSource.Play();
            MusicBtn.image.sprite = MusicOn;
            PlayerPrefs.SetInt("Music", 1);
        }
        BTNClick();
    }

    #endregion

}