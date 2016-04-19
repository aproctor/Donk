/**
 * Script to move camera to middle point between 2 objects.
 * TODO abstract this to follow an arbitrary number of objects
 * 
 * Adapted from http://stackoverflow.com/questions/22015697/how-to-keep-2-objects-in-view-at-all-time-by-scaling-the-field-of-view-or-zy
 */
using UnityEngine;
using System.Collections;

public class FollowerCameraRig : MonoBehaviour {

    public Transform[] players;
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
        if (players.Length > 0) {
            Bounds bounds = new Bounds(players[0].position, Vector3.zero);
            for(int i = 0; i < players.Length; i++) {
                bounds.Encapsulate(players[i].position);
            }            

            //Set the Rig to the middlepoint
            middlePoint = bounds.center;
            this.transform.position = middlePoint;

            // Calculate the new camera distance.
            distanceBetweenPlayers = Mathf.Max(bounds.size.x, (bounds.size.z * aspectRatio));
            cameraDistance = ((distanceBetweenPlayers / 2.0f + distanceMargin)) / tanFov;
            cameraDistance = Mathf.Clamp(cameraDistance, this.minimumDistance, this.maximumDistance);

            //Move the camera backwards away from the middlepoint
            followCam.transform.localPosition = new Vector3(0f, 0f, -cameraDistance);
        }        
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(middlePoint, 1f);
    }
}
