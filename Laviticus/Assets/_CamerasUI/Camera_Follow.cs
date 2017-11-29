using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.CameraUI
{ 
    public class Camera_Follow : MonoBehaviour {

    public GameObject player;

    private Vector3 offset;

    // Use this for initialization
    void Start() {

        offset = transform.position - player.transform.position;

    }

    // Update is called once per frame
    void Update() {

    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
}