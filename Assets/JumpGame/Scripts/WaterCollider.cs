using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterCollider : MonoBehaviour
{


    [SerializeField]
    private BoxCollider2D waterCollider;

    private bool flag = false;

    private GUIStyle guiStyle = new GUIStyle();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(GameManager.Instance.waterSound, other.gameObject.transform.position);
            waterCollider.enabled = !waterCollider.enabled;
            flag = true;
            // // //
        }
    }

    void OnGUI()
    {
        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.black;

        if (flag)
        {
            GUI.Label(new Rect(Camera.main.pixelWidth / 2 - 165, Camera.main.pixelHeight / 2 - 90, 180, 30), "You can't swim!",guiStyle);
            if (GUI.Button(new Rect(Camera.main.pixelWidth / 2 - 40, Camera.main.pixelHeight / 2 - 15, 80, 30), "Try again"))
            {
                int scene = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
        }
    }
}