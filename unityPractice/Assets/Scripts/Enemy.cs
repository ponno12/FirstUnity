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
            //�ִϸ��̼� ���¸� �о�ͼ� ������ ������ ���¿��� �ߵ��ϵ��� ��
            /*
            if(������ ���� ����){
            SceneManager.LoadScene("Stop");
            else(�� �ܻ���){
            SceneManager.LoadScene("DontStop");
            }
                */
            SceneManager.LoadScene("Stop");
        }
    }
    private void OnTriggerEnter(Collider other)

    {

        // �� ������Ʈ�� ������ ���� ������Ʈ�� �ݶ��̴��� �浹�� ���� ������Ʈ ��������

        var obj = other.gameObject.tag;
        

        Debug.Log(other.name + "���� ����!");
        
        
        /* �浹 �߻��� ���� �ѱ������
         * �ƴϸ� ȭ��ۿ��� �̸� �������� ���� �տ����̵��� �����Ͽ� �����ð�������
         * ���� ����ϰ� ��ȯ�Ǵ°����� ������ ����
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

        //�ִϸ��̼� ���¸� �о�ͼ� ������ ������ ���¿��� �ߵ��ϵ��� ��
        /*
        if(������ ���� ����){
        SceneManager.LoadScene("Stop");

            */
        SceneManager.LoadScene("Stop");
    }






}
