using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class movement : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private TextMeshProUGUI scoreSmall;

    private bool isOnTram = false;
    [SerializeField] private float x;
    private Rigidbody2D rb;
    [SerializeField] private float floatSpeed = 20;
    public string[] itemTags;
    public static int combo = 1;
    public bool isCombo = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        itemTags = new string[2];
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        


        if (x != 0)
        {
            isOnTram = false;
        }
        if (isOnTram)
        {
            //rb.transform.position += Vector3.up * floatSpeed * Time.deltaTime;
            Vector3 moveTo = new Vector3(0, floatSpeed * Time.deltaTime, 0);
            if (rb.transform.position.y < 4.5)
            {
                transform.position += moveTo;
            }
            else
            {
                isOnTram = false;
            }
        }

        if (x == 0) return;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * x, transform.localScale.y, transform.localScale.z);


    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(x, rb.velocity.y) * speed;

        
    }

    //ignore this
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("tram"))
        {
            isOnTram = true;
        }
        if (collision.gameObject.CompareTag("floor"))
        {
            isOnTram = false;
        }
    }
    //ignore this
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("tram"))
        {
            isOnTram = true;
        }
    }

    //combo system
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("tvItem"))
        {
            addScore(collision, 100);

            Vector3 spawnPos = new Vector3(collision.transform.position.x, collision.transform.position.y, collision.transform.position.z);
            //scoreSmall.SetText("100");
            //Instantiate(scoreSmall, spawnPos, Quaternion.identity);
        }
        else if (collision.gameObject.CompareTag("ringItem"))
        {
            addScore(collision, 200);
        }
    }
    private void addScore(Collider2D collision, int score)
    {
        itemTags[1] = collision.gameObject.tag;
        if (itemTags[0] != null)
        {
            Debug.Log("first:" + itemTags[0]);
            if (itemTags[0].Equals(itemTags[1]))
            {
                combo++;
                isCombo = true;
            }
            else
            {
                isCombo = false;
            }

            itemTags[0] = itemTags[1];
        }
        else
        {
            Debug.Log("null");
            itemTags[0] = collision.gameObject.tag;
        }

        Destroy(collision.gameObject);
        ScoreManager.Instance.IncreaseScoreItem(score, isCombo);
    }
}



