using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "player_controller", menuName = "input_controller/player_controller")]

public class player_script : input_controller
{
    public override bool get_jump_input(GameObject gameObject)
    {
        return Input.GetButtonDown("Jump");
    }

    public override float get_x_move_input(GameObject gameObject)
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override float get_y_move_input(GameObject gameObject)
    {
        return Input.GetAxisRaw("Vertical");
    }

    public override bool get_jump_hold_input(GameObject gameObject)
    {
        return Input.GetButton("Jump");
    }

    public override bool get_dash_input(GameObject gameObject)
    {
        return Input.GetButton("dash");
    }

    public override bool get_staff_attack_input(GameObject gameObject)
    {
        return Input.GetButton("attack");
    }

    public override Vector2 get_direction_input(GameObject gameObject)
    {
        var dir = new  Vector2(get_x_move_input(gameObject),get_y_move_input(gameObject));
        // dir = dir/dir.magnitude;
        return dir;
    }
}
