using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// [CreateAssetMenu(fileName = "ai_controller", menuName = "input_controller/ai_controller")]
[CreateAssetMenu(fileName = "walker_controller", menuName = "input_controller/walker_controller")]

public class walker_controller : input_controller
{
    [Header("interaction")]
    [SerializeField] private LayerMask my_layer_mask = -1;
    [Header("ray")]
    [SerializeField] private float bottom_ray_distance = 1f;
    [SerializeField] private float top_ray_distance = 1f;
    [SerializeField] private float x_offset = 1f;

    private RaycastHit2D ground_info_bottom;
    private RaycastHit2D ground_info_top;

    public override bool get_jump_input(GameObject gameObject)
    {
        return false;
    }

    public override float get_x_move_input(GameObject gameObject)
    {
        var ray_org = new Vector2(gameObject.transform.position.x + (x_offset * gameObject.transform.localScale.x),gameObject.transform.position.y);

        // ground_info_bottom = Physics2D.Raycast(ray_org,Vector2.down,bottom_ray_distance,my_layer_mask);

        // Debug.DrawRay(ray_org,Vector2.down * bottom_ray_distance,Color.green);

        ground_info_top = Physics2D.Raycast(ray_org,Vector2.right * gameObject.transform.localScale.x,bottom_ray_distance,my_layer_mask);

        Debug.DrawRay(ray_org,Vector2.right * gameObject.transform.localScale.x * top_ray_distance,Color.green);


        if(ground_info_top.collider == true)
        {
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1,gameObject.transform.localScale.y);
        }

        return gameObject.transform.localScale.x;
        
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
