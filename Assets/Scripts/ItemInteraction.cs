using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    public float DistanceToInteract = 4f;
    public bool IsInteracted;
    public GameObject Icon;
    public bool DoDamage;
    public GameObject Clock;
    GameObject Player;
    GameObject ClockInstance;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        IsInteracted = false;
    }

    private void EnableDamage()
    {
        this.gameObject.tag = "DAMAGE";
    }

    private void DisableClock()
    {
        Destroy(ClockInstance);
    }

    // Update is called once per frame
    void Update()
    {
        Icon.SetActive(false);
        if (!IsInteracted)
        {
            if(Vector3.Distance(this.transform.position,Player.transform.position)<= DistanceToInteract)
            {
                Icon.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E))
                {
                    if(this.GetComponent<Blinking>()!=null)
                    {
                        this.GetComponent<Blinking>().StopBlinking();
                    }
                    else
                    {
                        Blinking[] blinkings = this.GetComponentsInChildren<Blinking>();
                        for(int i = 0;i<blinkings.Length;i++)
                        {
                            blinkings[i].StopBlinking();
                        }
                    }
                    if (this.GetComponent<Animator>() != null)
                    {
                        this.GetComponent<Animator>().SetBool("Play", true);
                        if (Clock != null)
                        {
                            ClockInstance = Instantiate(Clock,this.gameObject.transform);
                            ClockInstance.transform.position = Icon.transform.position;
                            ClockInstance.transform.parent = null;
                            ClockInstance.GetComponent<Animator>().SetTrigger("Play");
                            ClockInstance.GetComponent<AudioSource>().Play();
                            Invoke("DisableClock", 2f);
                        }
                    }
                    IsInteracted = true;
                    switch (this.gameObject.tag)
                    {
                        case "BIND":
                            this.GetComponent<BoxCollider2D>().enabled = true;
                            break;
                        case "STONE":
                            GameManager.Instance.HandleActiveWater();
                            break;
                        case "WATER":
                            GameManager.Instance.HandleActiveSunLight();
                            break;
                        case "GEM":
                            GameManager.Instance.HandleActiveGem();
                            break;
                    }
                    if(DoDamage)
                    {
                        Invoke("EnableDamage", 1f);
                    }
                }
            }
        }
    }
}
