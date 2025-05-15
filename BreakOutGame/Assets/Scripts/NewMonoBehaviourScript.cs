using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject[] Blocks;
    public GameObject[] Bullet;

    void Start()
    {
        foreach (GameObject block in Blocks)
            block.name = "Block";
        foreach (GameObject bullet in Bullet)
            bullet.name = "Bullet";
    }
}
