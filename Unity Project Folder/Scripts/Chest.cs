using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Animator anim;
    GameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        anim= GetComponent<Animator>();
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.roundEnded)
        {
            anim.SetBool("openChest", true);
        }
        else
        {
            anim.SetBool("openChest", false);
        }
    }
}
