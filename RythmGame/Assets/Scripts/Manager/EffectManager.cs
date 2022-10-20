using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] Animator noteHitAnimator = null;//만든 animation 통제
    string hit = "Hit";
    [SerializeField] Animator judgementAnimator = null;
    [SerializeField] UnityEngine.UI.Image judgementImage = null;
    [SerializeField] Sprite[] judgementSprite = null;//눌린 타이밍마다 다르게 나올 애들을 넣을 배열

    public void JudgementEffect(int p_num)
    {
        judgementImage.sprite = judgementSprite[p_num];//파라미터 값에 따라 맞는 판정 이미지 스프라이트로 교체
        judgementAnimator.SetTrigger(hit);

    }
    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger(hit);
    }
}
