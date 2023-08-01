using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] clouds;
    [SerializeField]
    private float delayValue;

    private float delay;

    private void Start()
    {
        delay = delayValue;
    }

    // Update is called once per frame
    void Update()
    {
        var randomSpawn = new Vector2(3.3f, Random.Range(0f, 1.1f));

        if (delay == delayValue )
        {
            if (GameObject.Find("Small Cloud 1(Clone)") == false)
            {
                Instantiate(clouds[0], randomSpawn, Quaternion.identity);
            }
            else if (GameObject.Find("Small Cloud 2(Clone)") == false)
            {
                Instantiate(clouds[1], randomSpawn, Quaternion.identity);
            }
            else if (GameObject.Find("Small Cloud 3(Clone)") == false)
            {
                Instantiate(clouds[2], randomSpawn, Quaternion.identity);
            }
        }

        delay -= Time.deltaTime;

        if (delay <= 0)
        {
            delay = delayValue;
        }
        

        var Clouds = GameObject.FindGameObjectsWithTag("Cloud");
        foreach (GameObject cloud in Clouds)
        {
            Vector3 movement = new Vector3(-0.5f, 0, 0);
            cloud.transform.Translate(movement * Time.deltaTime);
            if (cloud.transform.position.x <= -3.3f)
            {
                Destroy(cloud);
            }
        }
    }
}
