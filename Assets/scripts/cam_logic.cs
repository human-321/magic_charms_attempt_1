using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;


public class cam_logic : MonoBehaviour
{
    

    private BoxCollider2D my_collider;
    private GameObject player_instance = null;
    private Camera my_cam = null;

    private Rigidbody2D my_rigidbody = null;
    private float cam_width = 0;
    private float cam_height = 0;

    private Vector3 player_displacement_vector;

    

    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        my_cam = GetComponent<Camera>();
        my_collider = GetComponent<BoxCollider2D>();
        my_rigidbody = GetComponent<Rigidbody2D>();

        //get width and height
        cam_height = 2f * my_cam.orthographicSize;
        cam_width = cam_height * my_cam.aspect;

        my_collider.size = new Vector2(cam_width,cam_height);
   

        
    }

    // Update is called once per frame
    void Update()
    {
        player_instance = GameObject.FindWithTag("Player");
        if(player_instance != null)
        {
            var my_pos = gameObject.transform.position;
            var player_pos = player_instance.transform.position;
            

            Vector2 player_displacement_vector = new Vector2(player_pos.x - my_pos.x,player_pos.y - my_pos.y);

            Vector2 move_vector = get_adjust_vector(new Vector2(player_displacement_vector.x,0)) + get_adjust_vector(new Vector2(0,player_displacement_vector.y));


            transform.position += new Vector3(move_vector.x,move_vector.y);
        }
        
    }

    Vector2 get_adjust_vector(Vector2 vec)
    {
        var adjust_vector = vec;
        var size = new Vector2(cam_width,cam_height);
        var layer = LayerMask.GetMask("cam stop");
        var check_ray = Physics2D.BoxCast(transform.position,size,0,adjust_vector.normalized,adjust_vector.magnitude,layer);

        for(var move_to_player_multiplier = 1f;check_ray.collider == true && move_to_player_multiplier > 0;move_to_player_multiplier -= .05f)
        {
            adjust_vector = player_displacement_vector;
            player_displacement_vector *= move_to_player_multiplier;
            Physics2D.BoxCast(transform.position,size,0,adjust_vector.normalized,adjust_vector.magnitude,layer);
        }

        return adjust_vector;
    }
    
    void level_loaded()
    {
        player_instance = GameObject.FindWithTag("Player");
        if(player_instance != null)
        {
            var player_pos = player_instance.transform.position;
            transform.position = new Vector3(player_pos.x , player_pos.y,transform.position.z);
        }
    }

}
