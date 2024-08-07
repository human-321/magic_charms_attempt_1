using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class progression_manager : MonoBehaviour
{
    public List<string> collected_keys = new List<string>();
    
    // Start is called before the first frame update
    void Start()
    {
        collected_keys[0] = "smeagma";
    }

    // Update is called once per frame
    void Update()
    {
    }


    #region key funcs

    public void add_key(string key)
    {
        collected_keys.Add(key);
    }

    public bool have_key(string key)
    {
        return collected_keys.Contains(key);

    }

    #endregion
}
