using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class locked_door_logic : MonoBehaviour
{
    [SerializeField] private string key_name = "you are not sigma\nfor not renaming this key"; // why did i do this

    // private GameObject player_instance;
    private progression_manager my_progression;
    private BoxCollider2D my_collider;
    // Start is called before the first frame update
    void Start()
    {
        my_collider = GetComponent<BoxCollider2D>();
        // player_instance = GameObject.FindWithTag("Player");
        my_progression = GameObject.FindWithTag("game_manager").GetComponent<progression_manager>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if(touch_player && my_progression.have_key(key_name))
        {
            Destroy(this.gameObject);
        }
    }
}
