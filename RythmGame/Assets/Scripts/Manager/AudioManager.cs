using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//inspector창에서 보이게 
public class Sound
{
    public string name;
    public AudioClip clip;
}
public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;//어디서든 호출할 수 있게 

    [SerializeField] Sound[] sfx = null;
    [SerializeField] Sound[] bgm = null;

    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource[] sfxPlayer = null;

    private void Start()
    {
        instance = this;
    }

    public void PlayBGM(string p_bgmName)
    {
        for(int i =0; i < bgm.Length; i++)
        {
            if(p_bgmName == bgm[i].name)
            {
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
            }
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }


    public void PlaySFX(string p_sfxName)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName ==sfx[i].name)
            {
                for(int x = 0; x < sfxPlayer.Length; x++)
                {
                    if (!sfxPlayer[x].isPlaying)//재생중인 아닌 것이라면 
                    {
                        sfxPlayer[x].clip = sfx[i].clip;
                        sfxPlayer[x].Play();
                        return;
                    }
                    

                }
                Debug.Log("모든 오디오 플레이어가 재생중 ");
                return;
            }
        }

        Debug.Log(p_sfxName + "이름의 효과음이 없소!");
    }

}