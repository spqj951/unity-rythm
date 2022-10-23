using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//이미지 받아오기 라이브러리 

public class StatusManager : MonoBehaviour
{
    [SerializeField] float blinkSpeed = 0.1f;//죽고나서 되살아날때 깜빡임 속도
    [SerializeField] int blinkCount = 10;
    int currentBlinkCount = 0;
    bool isBlink = false; 

    bool isDead = false; //죽었니 살았니 
    int maxHp = 3;
    int currentHp = 3;

    int maxShield = 3;
    int currentShield = 0;

    [SerializeField] Image[] hpImage = null;
    [SerializeField] Image[] shieldImage = null;

    [SerializeField] int shieldIncreaseCombo = 5;
    int currentShieldCombo = 0;
    [SerializeField] Image shieldGauge = null;//게이지 조절용 

    Result theResult;
    NoteManager theNote;
    [SerializeField] MeshRenderer playerMesh = null;//메쉬렌더러 껐다켰다해서 깜빡임 효과 

    private void Start()
    {
        theResult = FindObjectOfType<Result>();
        theNote = FindObjectOfType<NoteManager>();
    }

    public void CheckShield()
    {
        currentShieldCombo++;

        if(currentShieldCombo >= shieldIncreaseCombo)
        {
            IncreaseShield();
            currentShieldCombo = 0;
        }

        shieldGauge.fillAmount = (float)currentShieldCombo / shieldIncreaseCombo; //float을 붙임으로서 0~1
    }

    public void ResetShieldCombo()
    {
        currentShieldCombo = 0;
        shieldGauge.fillAmount = (float)currentShieldCombo / shieldIncreaseCombo;
    }

    public void IncreaseShield()
    {
        currentShield++;

        if(currentShield >= maxShield)
        {
            currentShield = maxShield;
        }
        SettingShieldImage();
    }



    public void DecreaseShield(int p_num)
    {
        currentShield -= p_num;

        if (currentShield <= 0)
        {
            currentShield = 0;
        }
        SettingShieldImage();
    }

    public void IncreseShield(int p_num)
    {
        currentHp += p_num;
        if(currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
        SettingHPImage();
    }

    public void DecreaseHp(int p_num)
    {
        if (!isBlink)//무적 타임을 줌 - 깜빡일때는 하트 안 깎이게
        {
            if(currentShield > 0)//실드가 있으면 실드먼
            {
                DecreaseShield(p_num);
            }
            else
            {

            currentHp -= p_num;

            if(currentHp <= 0)
            {
                isDead = true;
                theResult.ShowResult();
                theNote.RemoveNote();//게임끝나면 노트 사라짐

            }
            else
            {

            StartCoroutine(BlinkCo());//체력이 남아있을 때만 깜빡이	
            }
            SettingHPImage();
            }
        }
    }

    void SettingHPImage()
    {
        for(int i= 0; i < hpImage.Length; i++)
        {
            if(i < currentHp)
            {
                hpImage[i].gameObject.SetActive(true);//현재 체력보다 적으면 그에 해당하는 하트 이미지만 띄워줌

            }
            else
            {
                hpImage[i].gameObject.SetActive(false);
            }
        }
    }
    void SettingShieldImage()
    {
        for (int i = 0; i < shieldImage.Length; i++)
        {
            if (i < currentShield)
            {
                shieldImage[i].gameObject.SetActive(true);//현재 실드보다 적으면 그에 해당하는 실드 이미지만 띄워줌
                
            }
            else
            {
                shieldImage[i].gameObject.SetActive(false);
            }
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    IEnumerator BlinkCo()
    {
        isBlink = true;

        while(currentBlinkCount <= blinkCount)
        {
            playerMesh.enabled = !playerMesh.enabled;
            yield return new WaitForSeconds(blinkSpeed);//설정해놓은 값단위로 실행
            currentBlinkCount++;
        }

        playerMesh.enabled = true;//원위치
        currentBlinkCount = 0;
        isBlink = false;
    }
}
