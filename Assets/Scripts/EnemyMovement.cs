using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour {

    public GameObject player;
    private Vector3 playerPosition;
    public float speed;
    private LineRenderer laserShotLine;                 // Reference to the laser shot line renderer.
    public Text textbox;
    private Vector3 headPosition;
    void Start() {
        laserShotLine = GetComponentInChildren<LineRenderer>();
        StartCoroutine(CoroutineAction());
    }

    public static IEnumerator Frames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }

    public IEnumerator CoroutineAction()
    {
        if (UIStateManager.startRotation)
        {
            // do some actions here  
            yield return StartCoroutine(Frames(2)); // wait for 5 frames
            textbox.text = "laser disabled";
            laserShotLine.enabled = false;
        }// do some actions after 5 frames
    }


    // Update is called once per frame
    void Update() {
        if (UIStateManager.startRotation)
        {
            //laserShotLine.enabled = false;
            playerPosition = player.transform.position;
            Vector3 targetDir = playerPosition - transform.position;
            float step = speed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            //Debug.DrawRay(transform.position, newDir, Color.red);
             transform.rotation = Quaternion.LookRotation(newDir);
         }
    }
}
