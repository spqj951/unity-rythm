using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text txtScore = null;

    [SerializeField] int increaseScore = 10;
    int currentScore = 0;

    [SerializeField] float[] weight = null;//판정에 따라 달라질 점수 배열 
    [SerializeField] int comboBonusScore = 10;
    Animator myAnim;
    string animScoreUp = "ScoreUp";

    ComboManager theComboManager;

    // Start is called before the first frame update
    void Start()
    {
        theComboManager = FindObjectOfType<ComboManager>();
        myAnim = GetComponent<Animator>();
        currentScore = 0;
        txtScore.text = "0";
    }
    public void IncreaseScore(int p_JudgementState)
    {
        //콤보 증가 
        theComboManager.IncreaseCombo();

        //콤보 보너스 점수 계산
        int t_currentCombo = theComboManager.GetCurrentCombo();
        int t_bonusComboScore = (t_currentCombo / 10) * comboBonusScore;

        //가중치 계산
        int t_increaseScore = increaseScore+ t_bonusComboScore;

        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]);

        //점수 반영 
        currentScore += t_increaseScore;
        txtScore.text = string.Format("{0:#,##0}", currentScore);

        //애니메이션 실행	
        myAnim.SetTrigger(animScoreUp);
    }
}
