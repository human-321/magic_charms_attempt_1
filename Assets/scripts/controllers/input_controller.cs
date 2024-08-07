using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class input_controller : ScriptableObject
{
    public abstract float get_x_move_input(GameObject gameObject);

    public abstract float get_y_move_input(GameObject gameObject);

    public abstract bool get_jump_input(GameObject gameObject);

    public abstract bool get_jump_hold_input(GameObject gameObject);

    public abstract bool get_dash_input(GameObject gameObject);

    public abstract bool get_staff_attack_input(GameObject gameObject);

    public abstract Vector2 get_direction_input(GameObject gameObject);
}
