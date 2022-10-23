using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;//노트 생성을 위한 시간을 체크할 변수 오차 줄이기 위해 double

    bool noteActive = true;//노트 생성 관련

    [SerializeField] Transform tfNoteAppear = null;//노트 생성 위치 
    
    TimingManager theTimingManager;//참조
    EffectManager theEffect;
    ComboManager theComboManager;

    void Start()
    {
        theTimingManager = GetComponent<TimingManager>();
        theEffect = FindObjectOfType<EffectManager>();
        theComboManager = FindObjectOfType<ComboManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (noteActive)
        {
            currentTime += Time.deltaTime; //1초에 1씩 증가시.

            if (currentTime >= 60d / bpm)
            {
                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
                t_note.transform.position = tfNoteAppear.position;
                t_note.SetActive(true);

                theTimingManager.boxNoteList.Add(t_note);//생겼을 때 바로 List에 넣을 수 있도록
                currentTime -= 60d / bpm; // 0으로 만들면 안됨 왜냐하면 소수점시간의 오차가 생겨서 손실이 생김 -> bpm 밀려버려 

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)//collider에 들어오거나 나가면 발생하는 함
    {
        if (collision.CompareTag("Note"))//Note라는 이름의 태그를 가진 것에 부딪혔을 때
        {
             
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                theTimingManager.MissRecord();
                theEffect.JudgementEffect(4);//화면 바깥으로 나갔을 때 Miss나오게하랴
                theComboManager.ResetCombo();
            }

            theTimingManager.boxNoteList.Remove(collision.gameObject);

            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);
            
        }
    }
    public void RemoveNote()
    {
        noteActive = false;
        for(int i =0; i < theTimingManager.boxNoteList.Count; i++)
        {
            theTimingManager.boxNoteList[i].SetActive(false);
            ObjectPool.instance.noteQueue.Enqueue(theTimingManager.boxNoteList[i]);
        }
    }
}
