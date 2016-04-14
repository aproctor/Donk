/**
 * Script to move camera to middle point between 2 objects.
 * TODO abstract this to follow an arbitrary number of objects
 * 
 * Copied from http://stackoverflow.com/questions/22015697/how-to-keep-2-objects-in-view-at-all-time-by-scaling-the-field-of-view-or-zy
 */
using UnityEngine;
using System.Collections;

public class FollowerCamera : MonoBehaviour {

    public Transform player1;
    public Transform player2;
    private Camera followCam;

    public Game.CameraMode cameraMode = Game.CameraMode.SideBySide;

    public float distanceMargin = 1.0f;

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
        
        followCam = this.GetComponent<Camera>();
        tanFov = Mathf.Tan(Mathf.Deg2Rad * followCam.fieldOfView / 2.0f);        
    }

    void Update() {
        // Position the camera in the center.
        Vector3 newCameraPos = followCam.transform.position;
        newCameraPos.x = middlePoint.x;
        followCam.transform.position = newCameraPos;

        // Find the middle point between players.
        Vector3 vectorBetweenPlayers = player2.position - player1.position;
        middlePoint = player1.position + 0.5f * vectorBetweenPlayers;

        // Calculate the new distance.
        distanceBetweenPlayers = vectorBetweenPlayers.magnitude;
        cameraDistance = (distanceBetweenPlayers / 2.0f / aspectRatio) / tanFov;

        // Set camera to new position.
        Vector3 dir = (followCam.transform.position - middlePoint).normalized;
        followCam.transform.position = middlePoint + dir * (cameraDistance + distanceMargin);
    }
}
