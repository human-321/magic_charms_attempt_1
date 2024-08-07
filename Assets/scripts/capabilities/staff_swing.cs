using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class staff_swing : MonoBehaviour
{
    [SerializeField] private input_controller input = null;
    [SerializeField] private EdgeCollider2D staff_hitbox = null;
    [SerializeField] private Rigidbody2D my_rigidbody = null;
    [SerializeField] private Sprite staff_sprite = null;
    [SerializeField] private float staff_damage;
    [SerializeField] private float staff_length;
    [SerializeField] private float attack_knockback;
    [SerializeField] private float staff_attack_cooldown;
    [SerializeField] private float staff_attack_time;

    private float attack_cooldown_counter = 0;
    private float attack_time_counter = 0;
    private bool attack_input, can_attack, attacking;


    private Vector2 velocity, staff_attack_dir = Vector2.right;

    private jump jump_script = null;
    private dash dash_script = null;
    private collision_data_retriever ground = null;
    

    // Start is called before the first frame update
    void Start()
    {
        jump_script = GetComponentInParent<jump>();
        dash_script = GetComponentInParent<dash>();
        ground = GetComponentInParent<collision_data_retriever>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region // setup

        velocity = my_rigidbody.velocity;

        attack_input = input.get_staff_attack_input(this.gameObject);

        

        var staff_check = input.get_direction_input(this.gameObject);
        if(staff_check.x != 0 && staff_check.y != 0) staff_check.y = 0;
        if(staff_check.magnitude != 0 && !(staff_check == Vector2.down && ground.get_on_ground())) 
        {
            staff_attack_dir = staff_check;
        }
        
        

        #endregion

        #region state logic

        if(!can_attack)
        {
            attack_cooldown_counter += Time.deltaTime;
            can_attack = attack_cooldown_counter >= staff_attack_cooldown;
        } 
        if(attacking) attack_time_counter += Time.deltaTime;

        

        if(attack_time_counter >= staff_attack_time && attacking)
        {
            attacking = false;
            attack_cooldown_counter = 0;
            can_attack = false;
        }
        if(can_attack && attack_input && !attacking)
        {
            attacking = true;
            attack_time_counter = staff_attack_time;
            
            
        }
            
            
        

 
        #endregion

        #region //perform attack logic


        if(attacking)
        {
            staff_hitbox.enabled = true;
            var staff_hitbox_points = new List<Vector2>();
            staff_hitbox_points.Add(staff_attack_dir * staff_length);
            staff_hitbox_points.Add(Vector2.zero);

            staff_hitbox.SetPoints(staff_hitbox_points);

            Debug.DrawRay(transform.position,staff_length * staff_attack_dir,Color.green);
            
        }
        else
        {
            staff_hitbox.enabled = false;
        }
        #endregion

        #region //on staff hit

        




        #endregion
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("staff knockback"))
        {
            Debug.Log("backoff");
            my_rigidbody.velocity = staff_attack_dir * -attack_knockback;
            dash_script.dash_time_cooldown_counter = 0;
            if(jump_script.max_air_jumps > 0)jump_script.jump_phase = 0;
        }

        var enemy_damage_script = other.gameObject.GetComponent<damage_handler>();
        if(enemy_damage_script != null)
        {
            if(enemy_damage_script.damage_hitbox == other)
            {
                Debug.Log("hit");
                enemy_damage_script.get_hit(staff_damage);
            } 
            
        }

        
        
    }
}
