using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class dash : MonoBehaviour
{
    [SerializeField] private input_controller input = null;
    [SerializeField, Range(0f,35f)] private float dash_speed;
    [SerializeField, Range(0f,2f)] private float dash_time; // secs
    [SerializeField, Range(0f,2f)] private float dash_cooldown_time; // secs

    public bool in_dash;
   
    private float dash_time_counter;
    public float dash_time_cooldown_counter;

    private Vector2 direction, velocity;
    private Rigidbody2D my_rigidbody;
    private collision_data_retriever ground; 

    //scripts to turn off
    private move move_script;
    private slime_bounce slime_bounce_script;
    private jump jump_script;
    private wall_interactor wall_interactor_script;


    // Start is called before the first frame update
    void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();
        move_script = GetComponent<move>();
        slime_bounce_script = GetComponent<slime_bounce>();
        jump_script = GetComponent<jump>();
        wall_interactor_script = GetComponent<wall_interactor>();
        ground = GetComponent<collision_data_retriever>();
    }

    void stop_dash()
    {
        in_dash = false;
        dash_time_cooldown_counter = dash_cooldown_time;

        //activate movement script
        move_script.enabled = true;
        slime_bounce_script.enabled = true;
        jump_script.enabled = true;
        wall_interactor_script.enabled = true;

        velocity *= 0;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = my_rigidbody.velocity;
        if(!in_dash) direction = input.get_direction_input(this.gameObject);
        
        bool desire_dash = input.get_dash_input(this.gameObject);

        if(in_dash)
        {
            if(dash_time_counter > 0 && !ground.get_on_wall()) 
            {
                velocity = direction * (dash_speed);
                dash_time_counter -= Time.deltaTime;
            }
            else stop_dash();
 
        }
        else
        {
            dash_time_cooldown_counter -= Time.deltaTime;

            if(desire_dash && dash_time_cooldown_counter <= 0 && direction.magnitude > 0)
            {
                velocity = direction * (dash_speed);
                dash_time_counter = dash_time;
                in_dash = true;

                //deactivate movement scripts
                move_script.enabled = false;
                slime_bounce_script.enabled = false;
                jump_script.enabled = false;
                wall_interactor_script.enabled = false;
            }

            
        }

        my_rigidbody.velocity = velocity;
    }
}
