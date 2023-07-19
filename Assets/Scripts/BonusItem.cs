using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BonusItem : MonoBehaviour
{
    private Paul paul;
    [SerializeField] private GameObject pointsDisplay;
    [SerializeField] private Transform canvas;
    [SerializeField] private float size = 1;
    public bool canDoStuff = false;

    private void Awake()
    {
        enabled = false;
        canDoStuff = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canDoStuff) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            ScoreManager.Instance.IncreaseScoreItem(1000, false);
            //GameObject current = collision.gameObject;
            //Vector3 pos = new Vector3(current.transform.position.x + size, current.transform.position.y + size, current.transform.position.z);
            //GameObject pointDisplayText = Instantiate(pointsDisplay, pos, Quaternion.identity);
            //pointDisplayText.GetComponent<RectTransform>().SetParent(canvas);
            //TMP_Text smallScoreText = pointDisplayText.GetComponent<TMP_Text>();

            if (paul == null) return;
            paul.UnHide();
        }
    }

    public void SetPaul(Paul paul)
    {
        this.paul = paul;
    }
}
