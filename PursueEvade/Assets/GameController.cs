using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    public GameObject plane;
    public GameObject[] pickups;
    const int NumObj = 8;
    private int count = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI infoText;
    private int closetObject;
    private LineRenderer line;

    public bool IsPlaying = false;

    int GetClosetObject()
    {
        int idx = -1;
        float min = -1;
        for (int i = 0; i < pickups.Length; i++)
            if (pickups[i].activeSelf)
        {
            float temp = Vector3.Distance(player.transform.position, pickups[i].transform.position);
            if (min == -1 || min > temp)
            {
                idx = i;
                min = temp;
            }
        }
        return idx;
    }

    void Start()
    {
        winText.text = "";
        infoText.text = "0.0 m/s";
        if (line == null)
        {
            line = gameObject.AddComponent<LineRenderer>();
            line.SetWidth(0.1f, 0.1f);
        }
    }

    void StarNewGame()
    {
        Start();
        IsPlaying = true;
        count = 0;
        ShowScore();
        player.transform.position = new Vector3(0, 0.5f, 0);
        player.GetRigidbody().velocity = Vector3.zero;
        player.GetRigidbody().angularVelocity= Vector3.zero;
        const int wallWidth = 1;
        var mesgInfo = plane.GetComponent<MeshFilter>().mesh;
        var bounds = mesgInfo.bounds;
        for(int i = 0; i<pickups.Length; i++)
        {
            pickups[i].SetActive(true);
            float x = Random.Range(plane.transform.position.x - plane.transform.localScale.x * bounds.size.x/2 + wallWidth,
                                   plane.transform.position.x + plane.transform.localScale.x * bounds.size.x / 2 + wallWidth);
            float z = Random.Range(plane.transform.position.z - plane.transform.localScale.z * bounds.size.z / 2 + wallWidth,
                                   plane.transform.position.z + plane.transform.localScale.z * bounds.size.z / 2 + wallWidth);

            pickups[i].transform.position = new Vector3(x, 0.5f, z);
        }
    }
    void Update()
    {
        infoText.text = player.GetRigidbody().velocity.magnitude.ToString("0.00") + "m/s";
        line.SetPosition(0, player.transform.position);
        line.SetPosition(1, player.transform.position + player.GetRigidbody().velocity);
        
        int newidx = GetClosetObject();
        if(closetObject >= 0 && closetObject != newidx ) 
        {
            pickups[closetObject].GetComponent<Renderer>().material.color = Color.white;  
        }
        if(newidx >= 0 && closetObject != newidx)
        {
            pickups[newidx].GetComponent<Renderer>().material.color = Color.red;
            closetObject = newidx;
        }
    }

    private void OnGUI()
    {
        if(!IsPlaying)
        {
            GUIStyle style = GUI.skin.GetStyle("Button");
            style.fontSize = 50;
            if (GUI.Button (new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 300, 50),"New game", style))
            {
                StarNewGame();
            }
        }
    }

    void ShowScore()
    {
        scoreText.text = "Score: " + count;
        if (count >= NumObj)
        {
            winText.text = "You Win!";
            IsPlaying = false;
        }
    }

    public void Increase()
    {
        count++;
        ShowScore();
    }
}
