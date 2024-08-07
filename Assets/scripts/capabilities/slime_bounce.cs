using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class slime_bounce : MonoBehaviour
{
    
    [SerializeField, Range(0f,50f)] private float bounce_height;
    [SerializeField, Range(0f,50f)] private float max_bounce_height;
    [SerializeField, Range(0f,50f)] private float min_vertical_speed_for_boost;
    [SerializeField, Range(0f,1f)] private float min_to_max_boost_slope;
    
    
    private Rigidbody2D my_rigidbody;
    private collision_data_retriever my_collision_data;
    private Vector2 velocity;

    private bool last_did_bounce;

    private int bounce_count = 0;

    // Start is called before the first frame update
    void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();
        my_collision_data = GetComponent<collision_data_retriever>();
    }

    // Update is called once per frame
    void Update()
    {
  
        bool on_ground = my_collision_data.get_on_ground();
        velocity = my_rigidbody.velocity;

        if(on_ground || my_collision_data.on_wall) bounce_count = 0;

        if(my_collision_data.get_first_touch_bounce_object())
        {
            var force_in = Mathf.Abs(velocity.y);

            var force_requried_for_max_boost = max_bounce_height * min_to_max_boost_slope - bounce_height * min_to_max_boost_slope + min_vertical_speed_for_boost;

            //https://www.desmos.com/calculator/xxnfgs9gnk
            //that link explains the below
            if(force_in < min_vertical_speed_for_boost)
            {
                velocity.y = bounce_height;
            }
            if(bounce_count <= 0)
            {
                if(min_vertical_speed_for_boost <= force_in && force_in < force_requried_for_max_boost)
                {
                    velocity.y = min_to_max_boost_slope * (force_in - min_vertical_speed_for_boost)+bounce_height;
                }
                if(force_in >= force_requried_for_max_boost)
                {
                    velocity.y = max_bounce_height;
                }
            }
            else
            {
                velocity.y = bounce_height;
                
            }
            
            last_did_bounce = true;
            //to prevent infinte bounceing if the last thing you touched was a bouncer then you  get min boost
            Debug.Log(my_collision_data.get_last_touch());

            bounce_count += 1;
        }



        my_rigidbody.velocity = velocity;
    }
}
