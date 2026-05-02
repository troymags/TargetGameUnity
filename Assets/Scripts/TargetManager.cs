using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Texture cursorTexture;

    private Vector2 cursorHotspot;
    private Vector2 mousePos;

    [SerializeField]
    private Text getReadyText;

    [SerializeField]
    private GameObject resultsPanel;

    [SerializeField]
    private Text scoreText, targetsHitText, shotsFiredText, accuracyText;

    public static int score;
    public static int targetsHit;

    private float shotsFired;
    private float accuracy;

    private int targetAmount;

    private Vector2 targetRandomPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor((Texture2D)cursorTexture, cursorHotspot, CursorMode.Auto);

        getReadyText.gameObject.SetActive(false);

        targetAmount = 50;
        score = 0;
        shotsFired = 0;
        targetsHit = 0;
        accuracy = 0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shotsFired += 1f;
        }
    }

    private IEnumerator GetReady()
    {
        for (int i = 3; i >= 1; i--)
        {
            getReadyText.text = "Get Ready! " + i.ToString();
            yield return new WaitForSeconds(1f);
        }

        getReadyText.text = "Go!";
        yield return new WaitForSeconds(1f);

         StartCoroutine(SpawnTargets());
    }

    private IEnumerator SpawnTargets()
    {
        getReadyText.gameObject.SetActive(false);
        score = 0;
        shotsFired = 0;
        targetsHit = 0;
        accuracy = 0f;

        for (int i = targetAmount; i >= 0; i--)
        {
            targetRandomPos = new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f));
            Instantiate(target, targetRandomPos, Quaternion.identity);

            yield return new WaitForSeconds(1f);
        }

        resultsPanel.SetActive(true);
        scoreText.text = "Score: " + score;
        targetsHitText.text = "Targets Hit: " + targetsHit + " / " + targetAmount;
        shotsFiredText.text = "Shots Fired: " + shotsFired;

        accuracy = targetsHit / shotsFired * 100f;
        accuracyText.text = "Accuracy: " + accuracy.ToString("N2") + " %";
    }

    public void StartGame()
    {
        resultsPanel.SetActive(false);
        getReadyText.gameObject.SetActive(true);
        StartCoroutine(GetReady());
    }   
}
