using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class collision_data_retriever : MonoBehaviour
{
    [Header("no touch")]
    public Vector2 contact_normal;
    private PhysicsMaterial2D material;
    public bool first_touching_bounce_object = false;
    public float wall_x_dir;
    public bool on_ground;
    public bool on_wall;// {get; private set;}
    public float friction;
    public bool first_touching_jumper_switcher;
    public GameObject touch_jumper_switcher;
    public GameObject last_touch;
    [Header("tags")]
    public string bounce_game_object_tag = "bounce";
    public string jumper_switcher_tag = "jumper_switcher";



    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        evaluate_collision(collision);
        retrieve_friction(collision);


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        evaluate_collision(collision);
        retrieve_friction(collision);
        first_touching_jumper_switcher = false;
        // first_touching_bounce_object = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        on_ground = false;
        friction = 0;
        on_wall = false;
        first_touching_jumper_switcher = false;
        contact_normal = Vector2.zero;
        

        // first_touching_bounce_object = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(bounce_game_object_tag))
        {
            first_touching_bounce_object = true;
        }
        // else first_touching_bounce_object = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        first_touching_bounce_object = false;
    }

    private void OnTriggerExit2D(Collider2D cother)
    {
        first_touching_bounce_object = false;
    }


    public void evaluate_collision(Collision2D collision)
    {
        
        last_touch = collision.gameObject;
        for(int i =0; i < collision.contactCount; i++)
        {
            Vector2 contact_normal = collision.GetContact(i).normal;

            
            on_ground |= contact_normal.y >= 0.9f;
            wall_x_dir = contact_normal.x;
            on_wall = Mathf.Abs(contact_normal.x) >= 0.9f;
            if(touch_jumper_switcher != collision.gameObject && collision.gameObject)
            {
                first_touching_jumper_switcher |= collision.gameObject.CompareTag(jumper_switcher_tag);
                
            }

        }
        
        if(collision.gameObject)touch_jumper_switcher = collision.gameObject;
        if(on_ground) on_wall = false;

    }

    private void retrieve_friction(Collision2D collision)
    {
        material = collision.rigidbody.sharedMaterial;

        friction = 0;

        if(material != null)
        {
            friction = material.friction;
        }
    }

    public bool get_on_ground()
    {
        return on_ground;
    }

    public bool get_on_wall()
    {
        if(get_on_ground()) on_wall = false;
        return on_wall;
    }

    public float get_friction()
    {
        return friction;
    }

    public Vector2 get_surface_normal()
    {
        return contact_normal;
    }

    public float get_wall_x_dir()
    {
        return wall_x_dir;
    }

    public bool get_first_touch_bounce_object()
    {
        return first_touching_bounce_object;
    }

    public bool get_first_touch_jumper_switcher()
    {
        return first_touching_jumper_switcher;
    }

    public GameObject get_last_touch()
    {
        return last_touch;
    }
}
