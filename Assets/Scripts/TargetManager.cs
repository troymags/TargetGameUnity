using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Texture2D cursorTexture;

    [SerializeField]
    private Text getReadyText;

    [SerializeField]
    private GameObject resultsPanel;

    [SerializeField]
    private Text scoreText, targetsHitText, shotsFiredText, accuracyText, reactionTimeText;

    [Header("Target Sizes")]
    [SerializeField] private float smallSize = 0.5f;
    [SerializeField] private float mediumSize = 1.0f;
    [SerializeField] private float largeSize = 1.5f;

    public static int score;
    public static int targetsHit;

    private float shotsFired;
    private float accuracy;
    private int targetAmount;

    private Vector2 targetRandomPos;
    private Vector2 cursorHotspot;

    private static List<TargetData> targetDataLog = new List<TargetData>();
    private static Vector2 lastHitPos;

    private struct TargetData
    {
        public float size;
        public float reactionTimeMs;
        public Vector2 position;
        public float distanceFromPrevious;
        public bool hit;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

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
            getReadyText.text = "Get Ready!\n " + i.ToString();
            yield return new WaitForSeconds(1f);
        }

        getReadyText.text = "Go!";
        yield return new WaitForSeconds(1f);

         StartCoroutine("SpawnTargets");
    }

    private IEnumerator SpawnTargets()
    {
        getReadyText.gameObject.SetActive(false);
        score = 0;
        shotsFired = 0;
        targetsHit = 0;
        accuracy = 0;
        targetDataLog.Clear();
        lastHitPos = Vector2.zero;

        float[] sizes = new float[] { smallSize, mediumSize, largeSize };

        for (int i = targetAmount; i >= 0; i--)
        {
            targetRandomPos = new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f));
            float chosenSize = sizes[i % sizes.Length];
            
            GameObject newTarget = Instantiate(target, targetRandomPos, Quaternion.identity);
            newTarget.GetComponent<Target>().SetSize(chosenSize);

            yield return new WaitForSeconds(1f);
        }

       ShowResults();
    }

    private void ShowResults()
    {
        resultsPanel.SetActive(true);
        scoreText.text = "Score: " + score;
        targetsHitText.text = "Targets Hit: " + targetsHit + "/" + targetAmount;
        shotsFiredText.text = "Shots Fired: " + shotsFired;

        accuracy = (shotsFired > 0) ? (targetsHit / shotsFired * 100f) : 0f;
        accuracyText.text = "Accuracy: " + accuracy.ToString("N2") + " %";

        float avgRT = CalculateAverageReactionTime();
        reactionTimeText.text = "Avg Reaction Time: " + avgRT.ToString("N0") + " ms";

        SaveDataToCSV();
    }

    private float CalculateAverageReactionTime()
    {
        float total = 0f;
        int count = 0;
        foreach (var d in targetDataLog)
        {
            if (d.hit) { total += d.reactionTimeMs; count++; }
        }
        return count > 0 ? total / count : 0f;

    }

    private void SaveDataToCSV()
    {
        string fileName = $"AimData_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv";
        string path = Path.Combine(Application.persistentDataPath, fileName);

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("TrialNumber,TargetSize,ReactionTime_ms,PosX,PosY,DistanceFromPrevious,Hit");
            for (int i = 0; i < targetDataLog.Count; i++)
            {
                var d = targetDataLog[i];
                writer.WriteLine($"{i + 1},{d.size},{d.reactionTimeMs:F2},{d.position.x:F3},{d.position.y:F3},{d.distanceFromPrevious:F3},{d.hit}");
            }
        }
        Debug.Log("CSV saved to: " + path);
    }

    public void StartGetReadyCoroutine()
    {
        resultsPanel.SetActive(false);
        getReadyText.gameObject.SetActive(true);
        StartCoroutine("GetReady");

    }   
}
