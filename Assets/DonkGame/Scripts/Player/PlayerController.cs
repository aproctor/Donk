using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour {
 
    public float fireSensitivity = 0.1f;
    
    public float speed = 100f;
    public int playerNumber = 1;

    private Vector3 aimDirection = Vector3.zero;

    [Header("Object Links")]
    [SerializeField]
    private GameObject model;
    [SerializeField]
    private GameObject turret;

    [HideInInspector]
    public InputDevice device = null;

    void Start() {
        Renderer r = model.GetComponent<Renderer>();
        r.material = Instantiate<Material>(r.material);

        r.material.color = Game.Config.colors.playerColors[playerNumber - 1];
    }
	
	// Update is called once per frame
	void Update () {
        if (device == null && InputManager.Devices.Count >= playerNumber) {
            //FIXME pass in device properly when drop in exists
            Debug.LogError("Defaulted device for player " + playerNumber);
            device = InputManager.Devices[playerNumber - 1];
        }

        if (device) {
            UpdateDirection();
            UpdateActions();
        }
    }

    void UpdateDirection() {        
        //Convert Incontrol vectors to movement directions in 3d space
        Vector3 moveDirection = new Vector3(device.Direction.Vector.x, 0, device.Direction.Vector.y).normalized;
        this.aimDirection = new Vector3(device.RightStick.Vector.x, 0f, device.RightStick.Vector.y);

        this.transform.position += moveDirection * Time.deltaTime * this.speed;
        if (this.turret && aimDirection.sqrMagnitude > this.fireSensitivity) {                
            Quaternion quat = Quaternion.LookRotation(aimDirection, Vector3.up);
            this.turret.transform.rotation = quat;
        }
    }

    void UpdateActions() {
        //The following comments assume xbox 360 controls, but it should work cross platform

        if (device.Action1.WasPressed) {
            //A button
            Debug.LogError("Player "+ playerNumber + " A");
        }
        if (device.Action2.WasPressed) {
            //B button
            Debug.LogError("Player " + playerNumber + " B");
        }
        if (device.Action3.WasPressed) {
            //X button
            Debug.LogError("Player " + playerNumber + " X");
        }
        if (device.Action4.WasPressed) {
            //Y Button
            Debug.LogError("Player " + playerNumber + " Y");
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(this.transform.position + this.aimDirection, 0.3f);
    }
}
