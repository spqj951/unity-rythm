  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();// 생성된 노트를 담는 List : 판정범위에 있는 모든 노트를 비교해야하기 때문

    //판정기록용 
    int[] judgementRecord = new int[5];

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null;//다양한 판정범위를 보여줄 변수
    Vector2[] timingBoxes = null;//판정범위의 최솟값과 최댓

    EffectManager theEffect;
    ScoreManager theScoreManager;
    ComboManager theComboManager;
    StageManager theStageManager;
    PlayerController thePlayer;
    StatusManager theStatus;
    AudioManager theAudioManager;

    private void Start()
    {
        theAudioManager = AudioManager.instance;
        theEffect = FindObjectOfType<EffectManager>();
        theScoreManager = FindObjectOfType<ScoreManager>();
        theComboManager = FindObjectOfType<ComboManager>();
        theStageManager = FindObjectOfType<StageManager>();
        thePlayer = FindObjectOfType<PlayerController>();
        theStatus = FindObjectOfType<StatusManager>();

        //타이밍 박스 설정
        timingBoxes = new Vector2[timingRect.Length];
        for(int i =0; i < timingRect.Length; i++)
        {
            timingBoxes[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2, Center.localPosition.x + timingRect[i].rect.width / 2);//판정범위의 최소값과 최댓값
            //0번째의 범위가 perfect
            //3번째의 범위가 bad
        }
    }

    public bool CheckTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i++)//리스트에 있는 노트들을 확인해서 판정 박스에 있는 노트를 찾아야리
        {
            float t_notePosX = boxNoteList[i].transform.localPosition.x;
            for (int j = 0; j < timingBoxes.Length; j++)
            {
                if (timingBoxes[j].x <= t_notePosX && t_notePosX <= timingBoxes[j].y)
                {
                    //노트 제거
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);//범위에 들어와서 누르면 삭제

                    //이펙트 연출 
                    if (j < timingBoxes.Length - 1)
                        theEffect.NoteHitEffect();
                 


                    

                    if (CheckCanNextPlate())
                    {
                        theScoreManager.IncreaseScore(j);//점수 증가 
                        theStageManager.ShowNextPlate();//판때기 등장
                        theEffect.JudgementEffect(j);
                        judgementRecord[j]++;//판정기록
                        theStatus.CheckShield();//쉴드 게이지채우기 

                    }
                    else//normal : 같은 곳으로 다시 갔을 경우 : normal표시만 나오고 점수 콤보 아무것도 없음 
                    {
                        
                        theEffect.JudgementEffect(5);
                    }

                    theAudioManager.PlaySFX("Clap");

                    return true;
                }
            }
        }
        theComboManager.ResetCombo();
        theEffect.JudgementEffect(timingBoxes.Length);//4로 해도 상관없음
        MissRecord();
        return false;
    }

    bool CheckCanNextPlate()
    {
        if (Physics.Raycast(thePlayer.desPos, Vector3.down, out RaycastHit t_hitInfo, 1.1f))//어느 위치에서 광선을 쏠지,방향,부딪힌 결과 객체정보 저장 변수, 길이 
        {
            if (t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate t_plate = t_hitInfo.transform.GetComponent<BasicPlate>();
                if (t_plate.flag)
                {
                    t_plate.flag = false;
                    return true;//다음 발판이 나올 수 있다.
                }
            }
        }
        
        return false;
    }

    public int[] GetJudgementRecord()
    {
        return judgementRecord;
    }

    public void MissRecord()
    {
        theStatus.ResetShieldCombo();
        judgementRecord[4]++;
    }
}
