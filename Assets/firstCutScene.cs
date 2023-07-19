using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstCutScene : MonoBehaviour
{
    [SerializeField] private float speed = 8;
    private FallHandler fallHandler;
    private Animator animator;

    [SerializeField] private bool first = true;
    [SerializeField] private bool second = true;
    [SerializeField] private bool pos = true;
    [SerializeField] private float y;


    // Start is called before the first frame update
    void Start()
    {
        fallHandler = GetComponent<FallHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);

        if (first && transform.position.x <= 9.3)
        {
            transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
        }
        else if(transform.position.x >= 9.5 && transform.position.y < 5)
        {
            first = false;
        }

        if (!first && second && pos)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
            Debug.Log("omg");
        }
        pos = false;



        if (transform.position.y > 5.69 && transform.position.x > 8.39 && !first && second)
        {
            y = transform.position.y;
            fallHandler.StopFall();
            transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
        }
        else if(transform.position.x <= 8.39)
        {
            second = false;
            


        }


    }

    
}
