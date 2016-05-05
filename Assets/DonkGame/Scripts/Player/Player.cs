using UnityEngine;
using System.Collections;
using InControl;

public class Player : MonoBehaviour {
 
    public float fireSensitivity = 0.1f;
    
    public float speed = 100f;
    public int playerNumber = 1;

    private Vector3 aimDirection = Vector3.zero;

    [Header("Object Links")]
    [SerializeField]
    private GameObject model;
    [SerializeField]
    private GameObject turret;
    [SerializeField]
    int maxHealth = 100;
    [SerializeField]
    int currentHealth = 100;
    [SerializeField]
    Ability[] Abilities = new Ability[5];
    [SerializeField] LayerMask attackMask;

    [HideInInspector]
    public InputDevice device = null;

    void Start() {
        Renderer r = model.GetComponent<Renderer>();
        r.material = Instantiate<Material>(r.material);

        r.material.color = Game.Config.colors.playerColors[playerNumber - 1];

        this.currentHealth = this.maxHealth;
        this.Abilities[0] = new SwordSlash(this.transform, this.attackMask);
    }
	
	// Update is called once per frame
	void Update () {
        if (device == null && InputManager.Devices.Count >= playerNumber) {
            //FIXME pass in device properly when drop in exists
            Debug.LogError("Defaulted device for player " + playerNumber);
            device = InputManager.Devices[playerNumber - 1];
        }

        if (device) {
            UpdateActions();
        }
    }

    void FixedUpdate() {
        if (device) {
            UpdateDirection();
        }
    }

    void UpdateDirection() {        
        //Convert Incontrol vectors to movement directions in 3d space
        Vector3 moveDirection = new Vector3(device.Direction.Vector.x, 0, device.Direction.Vector.y).normalized;
        this.aimDirection = new Vector3(device.RightStick.Vector.x, 0f, device.RightStick.Vector.y);

        this.GetComponent<Rigidbody>().MovePosition(this.transform.position + moveDirection * Time.deltaTime * this.speed);
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
            if(this.Abilities[0] != null) {
                this.Abilities[0].Activate();
            }
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

        if (device.RightTrigger.WasPressed) {
            Attack();
            teampAttackSemaphore = true;
        } else {
            teampAttackSemaphore = false;
        }
    }

    private bool teampAttackSemaphore = false;
    public float tempAttackRadius = 10f;
    void Attack() {
        //TODO check equiped weapon, and run it's attack.  For now just doing some simple sphere intercepting
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, tempAttackRadius);
        for (int i = 0; i < hitColliders.Length; i++) {
            Damagable d = hitColliders[i].GetComponent<Damagable>();
            if (d != null) {
                d.TakeDamage(10f);
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(this.transform.position, 2f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(this.transform.position + this.aimDirection, 0.3f);

        if (this.teampAttackSemaphore) {
            Gizmos.color = new Color(1f,0f,0f,0.3f);
            Gizmos.DrawSphere(this.transform.position + this.aimDirection, this.tempAttackRadius);
        }
    }

    public void TakeDamage(int value) {
        this.currentHealth -= value;
        if(this.currentHealth <= 0) {
            Debug.LogError("Player " + this.playerNumber + " died");
        } else {
            Debug.LogError("Player " + this.playerNumber + " got hit");
        }
    }
}
