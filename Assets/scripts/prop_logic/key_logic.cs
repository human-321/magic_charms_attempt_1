using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class key_logic : MonoBehaviour
{
    [SerializeField] private string key_name = "you are not sigma\nfor not renaming this key"; // why did i do this
    [Header("hover stats")]
    [SerializeField] private float hover_amplitude = .05f;
    [SerializeField] private float hover_time_seconds = .25f; // in seconds


    // private GameObject player_instance;
    private Rigidbody2D body;
    private progression_manager my_progression;
    private float hover_phase = 0f;
    private BoxCollider2D my_collider;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        my_collider = GetComponent<BoxCollider2D>();
        // player_instance = GameObject.FindWithTag("Player");
        my_progression = GameObject.FindWithTag("game_manager").GetComponent<progression_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        // player_instance = GameObject.FindWithTag("Player");

        hover_phase += Time.deltaTime;

        var base_height = transform.position.y;

        var offset = math.sin(hover_phase * (math.PI*2/hover_time_seconds)) * hover_amplitude;

        var new_y = base_height + offset;

        body.MovePosition(new Vector2(transform.position.x,new_y));
        


        RaycastHit2D[] results = new RaycastHit2D[25];
        var touch_player = false;

        for(var i = 0;i <= my_collider.Cast(Vector2.down,results,0f,true) - 1;i++)
        {
            GameObject check = results[i].collider.gameObject;
            if(check != null) 
            {
                touch_player |= check.CompareTag("Player");
            }
        }

        if(touch_player) on_touch_player();
    }

    void on_touch_player()
    {
        my_progression.add_key(key_name);
        Destroy(this.gameObject);   
    }
}
