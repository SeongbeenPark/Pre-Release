using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 6;
    float x = 0;

    private Vector3 playerPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");            //키 입력. 방향키 좌우 또는 A D
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        playerPosition.x += x * speed * Time.deltaTime;
        playerPosition.x = Mathf.Clamp(playerPosition.x, -3f, 3f);          //플레이어의 이동범위 제한
        transform.position = playerPosition;
    }
}
