using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public Rigidbody2D enemyBody;
    public Collider2D enemyCollider;
    bool gameover = false;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D enemybody = GetComponent<Rigidbody2D>();
        Collider2D enemyCollider = GetComponent<Collider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //애니메이션 상태를 읽어와서 때리기 직전일 상태에만 발동하도록 함
            /*
            if(때리기 직전 상태){
            SceneManager.LoadScene("Stop");
            else(그 외상태){
            SceneManager.LoadScene("DontStop");
            }
                */
            SceneManager.LoadScene("Stop");
        }
    }
    private void OnTriggerEnter(Collider other)

    {

        // 이 컴포넌트가 부착된 게임 오브젝트의 콜라이더와 충돌한 게임 오브젝트 가져오기

        var obj = other.gameObject.tag;
        

        Debug.Log(other.name + "감지 시작!");
        
        
        /* 충돌 발생시 씬을 넘길것인지
         * 아니면 화면밖에서 미리 만들어놓고 제일 앞에보이도록 설정하여 가져올것인지는
         * 좀더 깔끔하게 전환되는것으로 선택할 예정
         */


        if (obj == "Player")
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene("DontStop");
        }
        gameover = true;

    }
    private void OnMouseDown()
    {

        //애니메이션 상태를 읽어와서 때리기 직전일 상태에만 발동하도록 함
        /*
        if(때리기 직전 상태){
        SceneManager.LoadScene("Stop");

            */
        SceneManager.LoadScene("Stop");
    }






}
