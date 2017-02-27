using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silence : Ability
{

    private float timer;            // counts lifetime
    private PlayerController player;


    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        timer = 0f;
    }
    void Update()
    {



        // destroy after lifetime seconds and revert radius to normal
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {


            foreach (Enemy enemy in FindObjectsOfType<Enemy>())
            {
                enemy.distanceThreshold = 5f;
            }

            Destruct();

        }

        else
        {
            foreach (Enemy enemy in FindObjectsOfType<Enemy>())
            {
                enemy.distanceThreshold = 0f;
            }
        }


    }




    //want to destruct after time out 
    public override void Destruct()
    {
        //FindObjectOfType<PlayerController>().Mobile = true;
        base.Destruct();
    }

    public override void Init(CharacterTemplate chr)
    {
        //var player = FindObjectOfType<PlayerController>().transform;
        //transform.parent = player;
        player = chr.GetComponent<PlayerController>();
        base.Init(chr);
    }

}
