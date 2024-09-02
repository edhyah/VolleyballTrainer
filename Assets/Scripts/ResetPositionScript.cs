using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputData))]
public class ResetPositionScript : MonoBehaviour
{
    private InputData _inputData;
    private bool primaryAlreadyPressed = false;
    private Vector3 originInGlobalCoordinates = new Vector3(-5f, 0f, -3.3f);

    // Start is called before the first frame update
    void Start()
    {
        _inputData = GetComponent<InputData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputData._rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool primaryIsPressed) && primaryIsPressed && !primaryAlreadyPressed)
        {
            GameObject XRRig = GameObject.Find("XR Origin (XR Rig)");

            Vector3 diff = originInGlobalCoordinates - transform.position;
            diff.y = 0;
            XRRig.transform.position += diff;

            // float rotationY = transform.rotation.y * 180f / 3.1415f;
            // Debug.Log("current rotation Y = " + rotationY);
            // // XRRig.transform.Rotate
            // // XRRig.transform.rotation = Quaternion.Euler(new Vector3(0, -1f * rotationY, 0));
            // Debug.Log("current eulerAngles" + XRRig.transform.eulerAngles);
            // XRRig.transform.eulerAngles = new Vector3(0, rotationY, 0);
            // Debug.Log("after eulerAngles" + XRRig.transform.eulerAngles);
            XRRig.transform.Rotate(0f, 0.5f, 0f);
            primaryAlreadyPressed = true;
        } else
        {
            primaryAlreadyPressed = false;
        }
    }
}
