using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class level_change_collision : MonoBehaviour
{
    public string scene_target;
    public string new_player_pos_target_tag;


    private bool started_load = false;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.name == "player" && !started_load)
        {
            
            GameObject.Find("game_manager").GetComponent<load_level_manager>().load_level(scene_target,new_player_pos_target_tag);
            gameObject.SetActive(false);

            started_load = true;
        }
    }



}
