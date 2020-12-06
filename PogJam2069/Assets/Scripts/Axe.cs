using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask AxeHitLayer;
    public float range;
    Animator anim;
    public float attackSpeed;
    private float timer;
    bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        canAttack = true;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (canAttack == true)
            {
                Attack();
            }
            canAttack = false;
        }

        if(canAttack == false)
        {
            timer += Time.deltaTime;
        }

        if(timer >= attackSpeed)
        {
            canAttack = true;
            timer = 0;
        }

    }



    void Attack()
    {

        anim.SetTrigger("Attack");

       Collider2D[] hitobjects = Physics2D.OverlapCircleAll(attackPoint.position, range,AxeHitLayer);

        int hittables = 0;

        foreach(Collider2D thingsHit in hitobjects)
        {
            thingsHit.GetComponent<AxeHitabble>().axeHit();
            //Debug.Log(thingsHit.name);
            hittables++;
        }

        if (hittables > 0) {
            if (GameObject.Find("Tree1").GetComponent<Tree>().isGivingTree) {
                AudioManager.Amanager.givingTreeChop();
            }
            else {
                AudioManager.Amanager.axeChop();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, range);
    }
}
