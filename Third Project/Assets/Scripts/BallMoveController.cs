using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoveController : MonoBehaviour
{
    public float ballspeed = 10.0f;                 //공 이동 속도
    private Vector2 ballDirection;                  //공의 이동 방향
    private bool isBallReleased = false;            //공이 플레이어에서 떨어졌는지 판단

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballDirection = Vector2.up.normalized;      //초기 공 이동 방향 설정
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBallReleased)
        {
            Vector3 playerPosition = GameObject.Find("Player").transform.position;
            //패들 오브젝트를 찾아 위치를 반환

            Vector3 ballPosition = playerPosition;                  //공의 위치를 플레이어의 위치로 변경
            ballPosition.y += 0.185f;                               //플레이어와 공 사이의 간격
            transform.position = ballPosition;                      //플레이어 위에 공을 고정

            if (Input.GetButtonDown("Fire1"))                       //EX) 마우스 좌클릭으로 공 발사
            {
                isBallReleased = true;
                //무작위 방향으로 공 발사
                ballDirection = new Vector2(Random.Range(-1f, 1f), 1).normalized;
            }
        }
        else
        {
            //공을 이동
            transform.Translate(ballDirection * ballspeed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            //벽과 충돌할 때 방향 반전
            ballDirection = Vector2.Reflect(ballDirection, collision.contacts[0].normal);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            //플레이어와 충돌할 때 방향 조절
            float hitPoint = collision.contacts[0].point.x;
            float playerCenter = collision.transform.position.x;
            float angle = (hitPoint - playerCenter) * 2.0f;
            ballDirection = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)).normalized;
        }
    }
}
