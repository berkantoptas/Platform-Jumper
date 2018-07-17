using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public AudioClip starSound;
    public AudioClip waterSound;

    [SerializeField]
    private GameObject starPrefab;

    [SerializeField]
    private Text starText;

    private int collectedStars;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public GameObject StarPrefab
    {
        get
        {
            return starPrefab;
        }
    }

    public int CollectedStars
    {
        get
        {
            return collectedStars;
        }

        set
        {
            starText.text = value.ToString();
            this.collectedStars = value;
        }
    }

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        if (collectedStars == 17)
        {
            SceneManager.LoadScene(2);
        }
    }
}
