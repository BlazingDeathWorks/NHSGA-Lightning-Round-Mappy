using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class movement : MonoBehaviour
{
    //player movement ignore this
    [SerializeField] private float speed = 2;
    private bool isOnTram = false;
    [SerializeField] private float x;
    private Rigidbody2D rb;
    [SerializeField] private float floatSpeed = 20;


    //>>>items and score<<<
    public string[] itemTags;
    public static int combo = 1;
    public bool isCombo = false;
    [SerializeField] private GameObject pointsDisplay;
    [SerializeField] private Transform canvas;
    [SerializeField] private float size = 1;

    //don't import ignore this
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    //don't import ignore this
    private void Start()
    {
        itemTags = new string[2];
    }
    //don't import ignore this
    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        if (x != 0)
        {
            isOnTram = false;
        }
        if (isOnTram)
        {
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
    //don't import ignore this
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(x, rb.velocity.y) * speed;
    }
    //don't import ignore this
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



    //>>>score + items<<<
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
        else if(collision.gameObject.CompareTag("gemItem"))
        {
            addScore(collision, 300);
        }
        else if (collision.gameObject.CompareTag("necklaceItem"))
        {
            addScore(collision, 400);
        }
        else if (collision.gameObject.CompareTag("crownItem"))
        {
            addScore(collision, 500);
        }


        //add more items
    }
    //>>>score + items<<<
    private void addScore(Collider2D collision, int score)
    {
        itemTags[1] = collision.gameObject.tag;
        if (itemTags[0] != null)
        {
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
            itemTags[0] = collision.gameObject.tag;
        }

        Destroy(collision.gameObject);

        GameObject current = collision.gameObject;
        Vector3 pos = new Vector3(current.transform.position.x + size, current.transform.position.y + size, current.transform.position.z);

        
        GameObject pointDisplayText = Instantiate(pointsDisplay, pos, Quaternion.identity);
        pointDisplayText.GetComponent<RectTransform>().SetParent(canvas);
        //pointDisplayText.GetComponent<RectTransform>().parent = canvas;

        TMP_Text smallScoreText = pointDisplayText.GetComponent<TMP_Text>();

        if (isCombo)
        {
            smallScoreText.text = score.ToString() + "×" + combo;
        }
        else
        {
            smallScoreText.text = score.ToString();
        }

        ScoreManager.Instance.IncreaseScoreItem(score, isCombo);
        Destroy(smallScoreText, 2);
    }
}



