using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string fireAxisHorizontal = "RightStickH1";
    public string fireAxisVertical = "RightStickV1";
    public float fireSensitivity = 0.1f;
    
    public float speed = 100f;
    public int playerNumber = 1;

    private Vector3 aimDirection = Vector3.zero;

    [Header("Object Links")]
    [SerializeField]
    private GameObject model;
    [SerializeField]
    private GameObject turret;

    void Start() {
        Renderer r = model.GetComponent<Renderer>();
        r.material = Instantiate<Material>(r.material);

        r.material.color = Game.Config.colors.playerColors[playerNumber - 1];
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 moveDirection = new Vector3(Input.GetAxis(horizontalAxis), 0, Input.GetAxis(verticalAxis)).normalized;

        this.transform.position += moveDirection * Time.deltaTime * this.speed;

        this.aimDirection = new Vector3(Input.GetAxis(fireAxisHorizontal), 0, Input.GetAxis(fireAxisVertical));
        if (this.turret && aimDirection.sqrMagnitude > this.fireSensitivity) {            
            Quaternion quat = Quaternion.LookRotation(aimDirection.normalized, Vector3.up);
            this.turret.transform.rotation = quat;
        }
	}
}
