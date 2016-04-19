/**
 * Script to move camera to middle point between 2 objects.
 * TODO abstract this to follow an arbitrary number of objects
 * 
 * Adapted from http://stackoverflow.com/questions/22015697/how-to-keep-2-objects-in-view-at-all-time-by-scaling-the-field-of-view-or-zy
 */
using UnityEngine;
using System.Collections;

public class FollowerCameraRig : MonoBehaviour {

    public Transform player1;
    public Transform player2;
    [SerializeField]
    private Camera followCam;

    public Game.CameraMode cameraMode = Game.CameraMode.SideBySide;

    public float distanceMargin = 1.0f;
    public float minimumDistance = 20f;
    public float maximumDistance = 200f;

    private Vector3 middlePoint;
    private float distanceFromMiddlePoint;
    private float distanceBetweenPlayers;
    private float cameraDistance;
    private float aspectRatio;
    private float fov;
    private float tanFov;

   
    void Start() {

        if (this.cameraMode == Game.CameraMode.SideBySide) {
            aspectRatio = ((float)Screen.width / 2) / Screen.height;
        } else {
            aspectRatio = Screen.width / Screen.height;
        }
        
        tanFov = Mathf.Tan(Mathf.Deg2Rad * followCam.fieldOfView / 2.0f);        
    }

    void Update() {
        // Find the middle point between players.
        Vector3 vectorBetweenPlayers = player2.position - player1.position;
        middlePoint = player1.position + 0.5f * vectorBetweenPlayers;
        
        // Calculate the new distance.
        distanceBetweenPlayers = vectorBetweenPlayers.magnitude;
        cameraDistance = ((distanceBetweenPlayers / 2.0f + distanceMargin)/ aspectRatio) / tanFov;
        cameraDistance = Mathf.Clamp(cameraDistance, this.minimumDistance, this.maximumDistance);

        //Set the Rig to the middlepoint
        this.transform.position = middlePoint;

        //Move the camera backwards away from the middlepoint
        followCam.transform.localPosition = new Vector3(0f, 0f, -cameraDistance);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(middlePoint, 0.2f);
    }
}
