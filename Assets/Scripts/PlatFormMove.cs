using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatFormMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.transform.tag == "Player")
        //{
        //    collision.gameObject.transform.parent = this.transform;
        //    collision.gameObject.GetComponent<InputManager>().Platform = this.gameObject;
        //}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.transform.tag == "Player")
        //{
        //    collision.gameObject.transform.parent = null;
        //    collision.gameObject.GetComponent<InputManager>().Platform = null;
        //}
    }
}
