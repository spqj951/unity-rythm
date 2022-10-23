using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//나한테 부딪힌 애가 이것이라면 
        {
            other.GetComponentInParent<PlayerController>().ResetFalling();//부딪힌 것은 자식 객체, component는 부모한테 있음 
        }
    }
}
