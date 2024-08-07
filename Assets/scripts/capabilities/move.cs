using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TreeEditor;
using Unity.Mathematics;

// using System.Numerics;
using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField] private input_controller my_input = null;
    [SerializeField, Range(0f,100f)] private float max_speed = 4f;
    [SerializeField, Range(0f,100f)] private float max_acceleration = 35f;
    [SerializeField, Range(0f,100f)] private float max_air_acceleration = 20f;
    

    
    private GameObject my_object;
    private Vector2 direction, desired_velocity,velocity;
    private Rigidbody2D my_rigidbody;
    private collision_data_retriever ground; 


    private float max_speed_change, acceleration;
    private bool on_ground;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();
        ground = GetComponent<collision_data_retriever>();
        my_object = gameObject;//for some reason when a jumper uses this script is freaks out when i use gameObject normally so this taboo var will work ig :(
    }
 


    private void FixedUpdate()
    {
        // Debug.Log(gameObject);
        direction.x = my_input.get_x_move_input(this.gameObject);
        
        desired_velocity = new Vector2(direction.x,0f) * MathF.Max(max_speed - ground.get_friction(),0f);


        on_ground = ground.get_on_ground();
        velocity = my_rigidbody.velocity;


        acceleration = on_ground ? max_acceleration : max_air_acceleration;
        max_speed_change = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x,desired_velocity.x,max_speed_change);

        // Debug.Log($"on_ground = {on_ground} on_wall = {ground.get_on_wall()} velocity = {velocity}" );



        my_rigidbody.velocity = velocity; 
    }
}
