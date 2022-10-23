using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject stage = null;
    Transform[] stagePlates;

    //plate가 밑에서 위로 올라오는 효과를 주기 위함 
    [SerializeField] float offsetY = -3;
    [SerializeField] float plateSpeed = 10;

    int stepCount = 0;//큐브가 몇걸음 움직였는지 판단
    int totalPlateCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        stagePlates = stage.GetComponent<Stage>().plates;//비활성화된 애들이 자동으로 생성
        totalPlateCount = stagePlates.Length;

        for(int i =0; i < totalPlateCount; i++)
        {
            stagePlates[i].position = new Vector3(stagePlates[i].position.x, stagePlates[i].position.y + offsetY, stagePlates[i].position.z);
        }            
    }

    public void ShowNextPlate()
    {
        if (stepCount < totalPlateCount)
        {
            StartCoroutine(MovePlateCo(stepCount++));
            
        }
    }

    IEnumerator MovePlateCo(int p_num)
    {
        stagePlates[p_num].gameObject.SetActive(true);
        Vector3 t_desPos = new Vector3(stagePlates[p_num].position.x, stagePlates[p_num].position.y - offsetY, stagePlates[p_num].position.z);

        while(Vector3.SqrMagnitude(stagePlates[p_num].position - t_desPos) >= 0.001f)
        {
            stagePlates[p_num].position = Vector3.Lerp(stagePlates[p_num].position, t_desPos, plateSpeed * Time.deltaTime);
            yield return null;
        }

        stagePlates[p_num].position = t_desPos; 
    }
}
