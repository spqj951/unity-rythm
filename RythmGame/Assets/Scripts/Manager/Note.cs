using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float noteSpeed = 400;

    UnityEngine.UI.Image noteImage;

    private void Start()
    {
        noteImage = GetComponent<UnityEngine.UI.Image>();
    }
    void Update()
    {
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;//localPosition이 아니라 position이면 canvas내가 아닌 전체를 기준으로 움직임으로 주의
    }
    public void HideNote()
    {
        noteImage.enabled = false;//사라지게 만드는 함(없애는것 대신)
    }

    public bool GetNoteFlag()
    {
        return noteImage.enabled;
    }
}
