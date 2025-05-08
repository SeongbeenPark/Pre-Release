using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoveController : MonoBehaviour
{
    public float ballspeed = 10.0f;                 //공 이동 속도
    private Vector2 ballDirection;                  //공의 이동 방향
    private bool isBallReleased = false;            //공이 플레이어에서 떨어졌는지 판단

    CircleCollider2D cc;
    GameObject temp;
    bool isDel = false;
    int count = 0;
    private Vector2 ballPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            RemoveBrick(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Out"))
        {
            isBallReleased = false;
        }
    }

    void RemoveBrick(GameObject brick)
    {
        if (count >= 2)
        {
            count = 0;
            return;
        }

        ballPos = transform.position;
        Vector2 pos = Vector2.zero;

        Collider2D[] col = Physics2D.OverlapCircleAll(ballPos, cc.radius / 2, LayerMask.GetMask("Block"));

        count = col.Length;

        GameObject[] colObj = new GameObject[col.Length];

        float[] p = new float[2];

        if (col.Length == 3)
        {
            int sour = 0;

            if (ballDirection.y >= 0)
            {
                if (ballDirection.x >= 0)
                    {
                    for (int i = 0; i < 3; i++)
                    {
                        if (col[i].transform.position.x >= ballPos.x && col[i].transform.position.y >= ballPos.y)
                        {
                            sour = i;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (col[i].transform.position.x < ballPos.x && col[i].transform.position.y >= ballPos.y)
                        {
                            sour = i;
                        }
                    }
                }
            }
            else //아래로 이동
            {
                if (ballDirection.x >= 0) //오른쪽 이동
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (col[i].transform.position.x >= ballPos.x && col[i].transform.position.y < ballPos.y)
                        {
                            sour = i;
                        }
                    }
                }
                else //왼쪽 이동
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (col[i].transform.position.x < ballPos.x && col[i].transform.position.y < ballPos.y)
                        {
                            sour = i;
                        }
                    }
                }
            }
            int cnt = 0;

            for (int i = 0; i < 3; i++)
            {
                if (i != sour)
                {
                    p[cnt] = Vector2.Distance(ballPos, col[i].transform.position);

                    colObj[cnt] = col[i].gameObject;

                    cnt++;
                }
            }
            if (p[0] <= p[1]) temp = colObj[0];
            else temp = colObj[1];
        }
        else if (col.Length == 2)
        {
            colObj[0] = col[0].gameObject;
            colObj[1] = col[1].gameObject;

            p[0] = Vector2.Distance(ballPos, colObj[0].transform.position);
            p[1] = Vector2.Distance(ballPos, colObj[1].transform.position);

            if (p[0] <= p[1]) temp = colObj[0];
            else temp = colObj[1];
        }
        else temp = brick;

        BoxCollider2D bc = temp.GetComponent<BoxCollider2D>();

        if (col.Length > 1)
        {
            if (colObj[0].transform.position.y == colObj[1].transform.position.y)
            {
                if (ballDirection.y >= 0) pos = Vector2.down;
                else pos = Vector2.up;  
            }
            else if (colObj[0].transform.position.x == colObj[1].transform.position.x)
            {
                if (ballDirection.x >= 0) pos = Vector2.left;
                else pos = Vector2.right;
            }
            else
            {
                isDel = true;

                if (ballDirection.y >= 0)
                {
                    if (ballDirection.x >= 0) pos = (Vector2.left + Vector2.down).normalized;
                    else pos = (Vector2.right + Vector2.down).normalized;
                }
                else
                {
                    if (ballDirection.x >= 0) pos = (Vector2.left + Vector2.up).normalized;
                    else pos = (Vector2.right + Vector2.up).normalized ;
                }
            }
        }
        else
        {
            if (ballDirection.y >= 0)
            {
                if (temp.transform.position.x - (bc.size.x / 2) > ballPos.x && ballDirection.x >= 0)
                {
                    pos = Vector2.left;
                }
                else if (temp.transform.position.x + (bc.size.x / 2) <= ballPos.x && ballDirection.x < 0)
                {
                    pos = Vector2.right;
                }
                else pos = Vector2.down;
            }
            else
            {
                if (temp.transform.position.x - (bc.size.x / 2) > ballPos.x && ballDirection.x >= 0)
                {
                    pos = Vector2.left;
                }
                else if (temp.transform.position.x + (bc.size.x / 2) <= ballPos.x && ballDirection.x < 0)
                {
                    pos = Vector2.right;
                }
                else pos = Vector2.up;
            }
        }
        StartCoroutine(SetCol());

        ballDirection = Vector2.Reflect(ballDirection, pos);

        if(!isDel) temp.GetComponent<BlockComponent>().TakeDamage();
        else
        {
            colObj[0].GetComponent<BlockComponent>().TakeDamage();
            colObj[1].GetComponent<BlockComponent>().TakeDamage();
            isDel = false;
        }

        temp = null;
    }
    IEnumerator SetCol()
    {
        cc.enabled = false;
        yield return new WaitForSeconds(0.005f);
        cc.enabled = true;
        count = 0;
    }
}
