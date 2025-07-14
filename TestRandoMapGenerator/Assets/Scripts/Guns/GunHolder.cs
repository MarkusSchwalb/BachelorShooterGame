using System;
using UnityEngine;

public class GunHolder : MonoBehaviour
{
    public Player player;
    public Gun Gun;
    public Animator animator;


    public void GetNewGun(GameObject gunObject)
    {
        DeleteChildren();
        GameObject SpawnNewGun = Instantiate(gunObject, transform);
        Gun = SpawnNewGun.GetComponent<Gun>();
        Gun.gunAnimator = animator;
    }

    private void DeleteChildren()
    {
        if (transform.childCount != 0)
        {
            Transform parent = transform;

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }

        }
    }
}
