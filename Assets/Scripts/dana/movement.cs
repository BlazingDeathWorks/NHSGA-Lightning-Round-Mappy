using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class movement : MonoBehaviour
{
    //player movement ignore this
    [SerializeField] private float speed = 2;
    [SerializeField] private float x;


    //>>>items and score<<<
    public string[] itemTags;
    public static int combo = 1;
    public bool isCombo = false;
    [SerializeField] private GameObject pointsDisplay;
    [SerializeField] private Transform canvas;
    [SerializeField] private float size = 1;

    
    //don't import ignore this
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(x, 0, 0);
        transform.Translate(movement * speed * Time.deltaTime);
    }



    private void Start()
    {
        itemTags = new string[2];
    }


    //>>>score + items<<<
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("tvItem"))
        {
            addScore(collision, 100);
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



