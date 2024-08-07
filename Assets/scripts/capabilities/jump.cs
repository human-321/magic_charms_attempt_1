using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class jump : MonoBehaviour
{
    [SerializeField] private input_controller input = null;
    [SerializeField, Range(0f,10f)] public float jump_height;
    [SerializeField, Range(0, 5)] public int max_air_jumps = 0;
    [SerializeField, Range(0f, 5f)] private float down_movement_multiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float up_movement_multiplier = 3f;
    [SerializeField, Range(0f, 1f)] private float coyote_time = .2f;
    [SerializeField, Range(0f, 1f)] private float jump_buffer_time = .2f;

    private Rigidbody2D my_rigidbody;
    private collision_data_retriever ground;
    private Vector2 velocity;

    public int jump_phase;
    private float default_gravity_scale;
    private float coyote_counter;
    private float jump_buffer_counter;

    private bool desired_jump, on_ground, is_jumping, jumped_from_ground;

    // Start is called before the first frame update
    void Awake()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();
        ground = GetComponent<collision_data_retriever>();

        default_gravity_scale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        desired_jump = input.get_jump_input(gameObject);
    }

    private void FixedUpdate()
    {


        on_ground = ground.get_on_ground();
        velocity = my_rigidbody.velocity;

        if(on_ground && my_rigidbody.velocity.y == 0)
        {
            jump_phase = 0;
            coyote_counter = coyote_time;
            is_jumping = false;
            jumped_from_ground = false;
        }
        else 
        {
            coyote_counter -= Time.deltaTime;
        }

        if(velocity.y <= 0) jumped_from_ground = false;

        if(desired_jump) // want to jump
        {
            desired_jump = false;
            jump_buffer_counter = jump_buffer_time;
        } 
        else if(!desired_jump && jump_buffer_counter > 0) // count down buffer
        {
            jump_buffer_counter -= Time.deltaTime;
        }

        if(jump_buffer_counter > 0)
        {
            jump_action();
        }

        if(jumped_from_ground)
        {
            if (input.get_jump_hold_input(this.gameObject) && my_rigidbody.velocity.y > 0) // up cuts off with no input
            { 
                my_rigidbody.gravityScale = up_movement_multiplier;
            }
            else if (!input.get_jump_hold_input(this.gameObject) || my_rigidbody.velocity.y < 0) // down same cutoff
            {
                my_rigidbody.gravityScale = down_movement_multiplier;
            }
            else if(my_rigidbody.velocity.y == 0) 
            {
                my_rigidbody.gravityScale = default_gravity_scale;
            }
        }
        else{
            if (my_rigidbody.velocity.y > 0) // if go up use up grv
            { 
                my_rigidbody.gravityScale = up_movement_multiplier;
            }
            else if(my_rigidbody.velocity.y < 0) // if go down use down grv
            {
                my_rigidbody.gravityScale = down_movement_multiplier;
            }

        }


            


            my_rigidbody.velocity = velocity;
    } 

    private void jump_action()
    {
        if(coyote_counter > 0f || (jump_phase < max_air_jumps && is_jumping))
        {
            if(is_jumping)
            {  
                jump_phase += 1;
            }
            
            jump_buffer_counter = 0;
            coyote_counter = 0;
            float jump_speed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jump_height * up_movement_multiplier);
            is_jumping = true;
            if(ground.on_ground)jumped_from_ground = true;

            if(velocity.y > 0f)
            {
                jump_speed = Mathf.Max(jump_speed - velocity.y, 0f);
            }

           if(jump_phase <= 0) velocity.y += jump_speed;
           else if(jump_phase > 0) velocity.y = jump_speed; // to make double jumps not get canceled out by gravity
        }
    }
}
