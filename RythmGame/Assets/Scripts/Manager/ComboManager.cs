using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] GameObject goComboImage = null;
    [SerializeField] UnityEngine.UI.Text txtCombo = null;

    int currentCombo = 0;

    Animator myAnime;
    string comboUpAnime = "ComboUp";
    void Start()
    {
        goComboImage.SetActive(false);
        txtCombo.gameObject.SetActive(false);
        myAnime = GetComponent<Animator>();
    }
    public void IncreaseCombo(int p_num = 1)//default값 설정

    {
        currentCombo += p_num;
        txtCombo.text = string.Format("{0:#,##0}", currentCombo);

        if(currentCombo > 2)
        {
            goComboImage.SetActive(true);
            txtCombo.gameObject.SetActive(true);
            myAnime.SetTrigger(comboUpAnime);
        }
    }
    public int GetCurrentCombo()
    {
        return currentCombo;
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        txtCombo.text = "0";
        txtCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }
}
