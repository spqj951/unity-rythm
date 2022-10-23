using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool s_canPresskey = true;

    //이동 
    [SerializeField] float moveSpeed = 3;
    Vector3 dir = new Vector3();
    public Vector3 desPos = new Vector3();
    Vector3 originPos = new Vector3();

    //회전 
    [SerializeField] float spinSpeed = 270;//회전시키기
    Vector3 rotDir = new Vector3();
    Quaternion desRot = new Quaternion();//목표 회전값

    //반동 
    [SerializeField] float recoilPosY = 0.25f;//들썩이게 만들기 
    [SerializeField] float recoilSpeed = 1.5f;//얼마나 빠르게 들썩일지

    bool canMove = true;
    bool isFalling = false; //떨어지는 중인 
    [SerializeField] Transform fakeCube = null;
    [SerializeField] Transform realCube = null;

    TimingManager theTimingManager;
    CameraController theCam;
    Rigidbody myRigid;
    StatusManager theStatus;

    void Start()
    {
        theTimingManager = FindObjectOfType<TimingManager>();
        theCam = FindObjectOfType<CameraController>();
        myRigid = GetComponentInChildren<Rigidbody>();//player객체에 없고 자식인 cube객체에 있기 때문에
        originPos = transform.position; //내 첫 위치 기억
        theStatus = FindObjectOfType<StatusManager>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckFalling();

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) )
        {
            if (canMove && s_canPresskey && !isFalling)
            {
                Calc();
                if (theTimingManager.CheckTiming())
                {
                    
                    StartAction();
                }
            }
        }
    }

    void Calc()
    {
        //방향 
        dir.Set(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));//x,y,z

        //이동 목표
        desPos = transform.position + new Vector3(-dir.x, 0, dir.z);
        StartCoroutine(MoveCo());

        //회전 목표값
        rotDir = new Vector3(-dir.z, 0f, -dir.x);
        fakeCube.RotateAround(transform.position, rotDir, spinSpeed);
        desRot = fakeCube.rotation;
    }

    void StartAction()
    {
       

        StartCoroutine(SpinCo());

        StartCoroutine(RecoilCo());
        StartCoroutine(theCam.ZoomCam());
            
    }

    IEnumerator MoveCo()
    {
        canMove = false;

        while(Vector3.SqrMagnitude(transform.position - desPos) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, desPos, moveSpeed * Time.deltaTime);
            yield return null; //한프레임씩 쉬면서
        }

        transform.position = desPos;//근소한 차이 남은거 없앰

        canMove = true;
    }

    IEnumerator SpinCo()
    {
        while(Quaternion.Angle(realCube.rotation, desRot) > 0.5f)//두 개값의 차를 구
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, desRot, spinSpeed * Time.deltaTime);
            yield return null;
        }
        realCube.rotation = desRot; 
    }

    IEnumerator RecoilCo()
    {
        while(realCube.position.y < recoilPosY)//최고 반동높이 
        {
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }
        while(realCube.position.y > 0) 
        {
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        realCube.localPosition = new Vector3(0, 0, 0);
    }

    void CheckFalling()
    {
        //player아랫방향으로 레이저를 쏴서 Plate가 있나 없나 확인
        if (!isFalling && canMove)//이미 추락중인데도 안의 함수를 실행시킬 필요가 없어서 && 움직임이 완전히 멈춘 상태에서 판단하기 
        {
            if (!Physics.Raycast(transform.position, Vector3.down, 1.1f))//충돌한게 없으면 떨어트려
            {
                Falling();
            }
        }
    }

    void Falling()
    {
        isFalling = true;
        myRigid.useGravity = true;
        myRigid.isKinematic = false;//물리 효과를 킴 
    }

    public void ResetFalling()
    {
        theStatus.DecreaseHp(1);//떨어지면 체력감소
        if (!theStatus.IsDead())//죽지 않았다면
        {

        isFalling = false;
        myRigid.useGravity = false;
        myRigid.isKinematic = true;
        transform.position = originPos;//부모만 되돌린 것 
        realCube.localPosition = new Vector3(0, 0, 0);//Rigidbody가 없는 부모객체는 낭떠러지 위에있, 자식 객체는 추락중이므로  
        }
    }
}

