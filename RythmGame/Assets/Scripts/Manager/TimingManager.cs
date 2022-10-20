using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();// 생성된 노트를 담는 List : 판정범위에 있는 모든 노트를 비교해야하기 때문

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null;//다양한 판정범위를 보여줄 변수
    Vector2[] timingBoxes = null;//판정범위의 최솟값과 최댓

    EffectManager theEffect;

    private void Start()
    {
        timingBoxes = new Vector2[timingRect.Length];
        theEffect = FindObjectOfType<EffectManager>();

        for(int i =0; i < timingRect.Length; i++)
        {
            timingBoxes[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2, Center.localPosition.x + timingRect[i].rect.width / 2);//판정범위의 최소값과 최댓값
            //0번째의 범위가 perfect
            //3번째의 범위가 bad
        }
    }

    public void CheckTiming()
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

                    //이펙트 연
                    if (j < timingBoxes.Length - 1)
                        theEffect.NoteHitEffect();
                    theEffect.JudgementEffect(j);

                    Debug.Log("Hit" + j);

                    return;
                }
            }
        }
        theEffect.JudgementEffect(timingBoxes.Length);//4로 해도 상관없음
        Debug.Log("Miss");
    }
}
