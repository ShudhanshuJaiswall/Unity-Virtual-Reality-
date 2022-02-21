using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZB : GazePointer
{
    // Start is called before the first frame update
    public GameObject particleEffect;
    public float speed;

    public Animator enemyModel;
    bullets bulletspawn;
    Vector3 endPos;
    void Start()
    {
        endPos = 1.5f * (transform.position - Vector3.zero).normalized;
        bulletspawn = GameObject.FindObjectOfType<bullets>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, endPos, speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        Attack();
        else if (other.CompareTag("enemy")) ;
        Death();
        
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        bulletspawn.ShootBullet();
        
        enemyModel.SetBool("hit", true);
        Death();
    }
    public void Death()
    {
        particleEffect.SetActive(true);
        particleEffect.transform.SetParent(null);
        
        Destroy(this.gameObject,5);
        PlayerManagerZ.currnetScore += 100;
    }
    public void Attack()
    {
        enemyModel.SetTrigger("attack");
        
        PlayerManagerZ.playerHealth -= 0.4f;
    }
}
