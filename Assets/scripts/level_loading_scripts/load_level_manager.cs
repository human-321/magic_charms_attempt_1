using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_level_manager : MonoBehaviour
{
    
    private GameObject player_instance = null;

    private GameObject player_new_pos_object;

    private PrefabAssetType player_prefab;
    
    public bool loading_level = false;

    string scene_target_name, player_pos_target_tag;

    public int call_count = 0;

    void Start()
    {
        player_instance = GameObject.FindWithTag("Player");

        

        DontDestroyOnLoad(gameObject);
        
    }

    void Update()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void load_level(string scene_target_name,string pos_target_tag)
    {
        if(!loading_level)
        {
            player_pos_target_tag = pos_target_tag;
            DontDestroyOnLoad(player_instance);
            player_instance.SetActive(false);
            SceneManager.LoadScene(scene_target_name,LoadSceneMode.Single);

            loading_level = true;
        }
    }


    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        if(loading_level)
        {
            player_instance.SetActive(true);

            var my_player = Instantiate(player_instance);

            Destroy(player_instance);
            player_instance = my_player;

            player_new_pos_object = GameObject.FindGameObjectWithTag(player_pos_target_tag);
            // Debug.Log(player_pos_target_tag);
    
            player_instance.transform.position = new Vector3(player_new_pos_object.transform.position.x, player_new_pos_object.transform.position.y, player_instance.transform.position.z);

            // Debug.Log(player_new_pos_object.transform.position);
            // Debug.Log(player_instance.transform.position);
            GameObject.FindWithTag("cam").SendMessage("level_loaded");
            loading_level = false;

        }
    }

}
