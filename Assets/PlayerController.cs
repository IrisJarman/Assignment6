using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;
    public Text countText;
    public Text livesText;
    public static int count = 0;
    private static int maxLives = 3;
    private static int currentLives = maxLives;
    public Text ScoreCount;
    public InputField playerName;
    public string player;
    
    public int num_scores = 10;


    private void Start()
    {
        agent.updateRotation = false;

        SetCountText();
        SetLivesText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pickup"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            currentLives--;
            if (currentLives <= 0)
            {
                AddNewScore();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
            }
        }
    }

    void SetCountText()
    {
        countText.text = count.ToString();
    }
    void SetLivesText()
    {
        livesText.text = "Lives:" + currentLives.ToString();
    }
    public void SetPlayerName()
    {
        player = playerName.text;
    }
    public void AddNewScore()
    {
        string path = "Assets/scores.txt";
        string line;
        string[] fields;
        int scores_written = 0;
        string newName = "don't forget to input";
        string newScore = "999";
        bool newScoreWritten = false;
        string[] writeNames = new string[10];
        string[] writeScores = new string[10];

        newName = player;
        newScore = ScoreCount.text;

        // you need to have at least one record (name, score) for this to work.
        // if there is no record at all,  the code  below will report an index out of bounds.
        // you can fix this with the code at https://www.techiedelight.com/check-if-file-is-empty-csharp/

        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();
            fields = line.Split(',');
            if (!newScoreWritten && scores_written < num_scores) // if new score has not been written yet
            {
                //check if we need to write new higher score first
                if (Convert.ToInt32(newScore) > Convert.ToInt32(fields[1]))
                {
                    writeNames[scores_written] = newName;
                    writeScores[scores_written] = newScore;
                    newScoreWritten = true;
                    scores_written += 1;
                }

            }
            if (scores_written < num_scores) // we have not written enough lines yet
            {
                writeNames[scores_written] = fields[0];
                writeScores[scores_written] = fields[1];
                scores_written += 1;
            }
        }
        reader.Close();

        // now we have parallel arrays with names and scores to write
        StreamWriter writer = new StreamWriter(path);

        for (int x = 0; x < scores_written; x++)
        {
            writer.WriteLine(writeNames[x] + ',' + writeScores[x]);
        }
        writer.Close();

        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("scores");

    }
}
