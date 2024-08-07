using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage_handler : MonoBehaviour
{
    public Collider2D damage_hitbox;
    public float health;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void get_hit(float damage)
    { 
        health -= damage;
        if(health <= 0) die_logic();
    }

    public void die_logic()
    {
        Destroy(gameObject);
    }
}
