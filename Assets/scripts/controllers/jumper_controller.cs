using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


// [CreateAssetMenu(fileName = "ai_controller", menuName = "input_controller/ai_controller")]
[CreateAssetMenu(fileName = "jumper_controller", menuName = "input_controller/jumper_controller")]

public class jumper_controller : input_controller
{
 
    [SerializeField] private LayerMask my_layer_mask = -1;

    
    private float x_dir = 1f;

    public override bool get_jump_input(GameObject my_object)
    {
        
        return my_object.GetComponent<collision_data_retriever>().on_ground;
    }

    public override float get_x_move_input(GameObject my_object)
    {
        var on_switcher = my_object.GetComponent<collision_data_retriever>().get_first_touch_jumper_switcher();
        if(on_switcher)
        {
            x_dir *= -1f;

        }

        return x_dir;
        
    }


    public override float get_y_move_input(GameObject gameObject)
    {
        return 0f;
    }

    public override bool get_jump_hold_input(GameObject gameObject)
    {
        return false;
    }

    public override bool get_dash_input(GameObject gameObject)
    {
        return false;
    }

    public override bool get_staff_attack_input(GameObject gameObject)
    {
        return false;
    }

    public override Vector2 get_direction_input(GameObject gameObject)
    {
        var dir = new Vector2(get_x_move_input(gameObject),get_y_move_input(gameObject));
        return dir;
    }
}
