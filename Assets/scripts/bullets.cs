using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullets : MonoBehaviour
{
    public GameObject bullet;
    public float speed = 1;
    public Animator BulletModel;
    public void ShootBullet()
    {
        var bulletInstance = Instantiate(bullet);
        bulletInstance.transform.position = transform.position;
        bulletInstance.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        BulletModel.SetBool("Fire", true);
    }

}
