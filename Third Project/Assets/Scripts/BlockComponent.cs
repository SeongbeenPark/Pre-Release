using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockComponent : MonoBehaviour
{
    [SerializeField] int hp = 1;

    public void TakeDamage()
    {
        hp -= 1;
        if (hp == 0) StartCoroutine(Death());

    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(0.01f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            collision.GetComponent<BlockComponent>().TakeDamage();
        }
    }
}