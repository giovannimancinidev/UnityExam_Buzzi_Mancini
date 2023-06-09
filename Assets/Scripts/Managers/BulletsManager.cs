using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsManager : MonoBehaviour
{
    [Header ("Bullets Parameters")]
    public GameObject BulletPrefab;
    public int amountOfBullets;

    private List<GameObject> bulletsPooled;

    public void Awake()
    {
        bulletsPooled = new List<GameObject>();
        GameObject tmp;

        for (int i = 0; i < amountOfBullets; i++)
        {
            tmp = Instantiate(BulletPrefab, Vector3.one, new Quaternion(0.7071068f, 0, 0, 0.7071068f), gameObject.transform);
            tmp.SetActive(false);
            bulletsPooled.Add(tmp);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < amountOfBullets; i++)
        {
            if (!bulletsPooled[i].activeInHierarchy)
            {
                return bulletsPooled[i];
            }
        }

        return null;
    }
}
