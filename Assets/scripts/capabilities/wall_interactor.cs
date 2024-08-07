using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
// using System.Numerics;
using UnityEngine;

public class wall_interactor : MonoBehaviour
{
    public bool wall_jumping;

    [SerializeField] private input_controller input = null;
    [Header("wall slide")]
    [SerializeField][Range(0.1f,5f)] private float wall_slide_max_speed = 2f;
    [Header("wall jump")]
    [SerializeField] private Vector2 wall_jump_climb = new Vector2(4f,12f);
    [SerializeField] private Vector2 wall_jump_bounce = new Vector2(10f,10f);
    [SerializeField] private Vector2 wall_jump_leap = new Vector2(20f,12f);
    [Header("wall stick")]
    [SerializeField, Range(0f,0.5f)] private float wall_stick_time = .25f;

    private collision_data_retriever collision_data_getter;
    private Rigidbody2D my_body;
    // private input_controller input;

    private Vector2 velocity;
    private bool on_wall, on_ground, desired_jump;
    private float wall_dir_x, wall_stick_counter;

    // Start is called before the first frame update
    void Start()
    {
        collision_data_getter = GetComponent<collision_data_retriever>();
        my_body = GetComponent<Rigidbody2D>();
        // input = GetComponent<input_controller>();
    }

    // Update is called once per frame
    void Update()
    {
        on_wall = collision_data_getter.get_on_wall();
        on_ground = collision_data_getter.on_ground;
        if(on_wall && !on_ground) desired_jump = input.get_jump_input(this.gameObject);
    }

    private void FixedUpdate()
    {
        velocity = my_body.velocity;
        on_wall = collision_data_getter.on_wall;
        on_ground = collision_data_getter.on_ground;

        wall_dir_x = collision_data_getter.get_wall_x_dir();

        #region wall stick

        if(collision_data_getter.on_wall && !collision_data_getter.on_ground && !wall_jumping)
        {
            if(wall_stick_counter > 0)
            {
                velocity.x = 0;

                if(input.get_x_move_input(this.gameObject) == collision_data_getter.get_wall_x_dir())
                {
                    wall_stick_counter -= Time.deltaTime;
                }
                else
                {
                    wall_stick_counter = wall_stick_time;
                }
            }
            else
            {
                wall_stick_counter = wall_stick_time;
            }
        }

        #endregion

        #region wall slide
        if(on_wall)
        {
            if(velocity.y < -wall_slide_max_speed) velocity.y = -wall_slide_max_speed;
        }


        #endregion

        #region  wall jump

        if((on_wall && velocity.x == 0) || on_ground) wall_jumping = false;

        if(desired_jump)
        {
            if(-wall_dir_x == input.get_x_move_input(this.gameObject))
            {
                velocity = new Vector2(wall_jump_climb.x * wall_dir_x, wall_jump_climb.y);
                wall_jumping = true;
                desired_jump = false;

            }
            else if(input.get_x_move_input(this.gameObject) == 0)
            {
                velocity = new Vector2(wall_jump_bounce.x * wall_dir_x, wall_jump_bounce.y);
                wall_jumping = true;
                desired_jump = false;

            }
            else{
                velocity = new Vector2(wall_jump_leap.x * wall_dir_x, wall_jump_leap.y);
                wall_jumping = true;
                desired_jump = false;

            }

            
        }

        #endregion


        my_body.velocity = velocity;
    }
}
