using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

class Range
{
    static System.Random random = new System.Random();
    public double min;
    public double max;

    public Range(double min, double max)
    {
        this.min = min;
        this.max = max;
    }

    public float getRandomNumberInRange()
    {
        return (float)(random.NextDouble() * (max - min) + min);
    }
}

class ThreeDimensionalRange 
{
    public Range rangeX;
    public Range rangeY;
    public Range rangeZ;

    public ThreeDimensionalRange(double minX, double maxX, double minY, double maxY, double minZ, double maxZ)
    {
        this.rangeX = new Range(minX, maxX);
        this.rangeY = new Range(minY, maxY);
        this.rangeZ = new Range(minZ, maxZ);
    }

    public Vector3 getRandomPointInRange()
    {
        return new Vector3(
            this.rangeX.getRandomNumberInRange(),
            this.rangeY.getRandomNumberInRange(),
            this.rangeZ.getRandomNumberInRange()
        );
    }
}

[RequireComponent(typeof(InputData))]
public class BallScript : MonoBehaviour
{
    Rigidbody rb;

    public InputActionProperty resetSetterAction;
    private InputData _inputData;

    private bool primaryAlreadyPressed = false;
    private bool secondaryAlreadyPressed = false;

    // Volleyball net is 2.43m
    Range heightRange = new Range(4.8d, 6.3d);
    ThreeDimensionalRange setterPosRange = new ThreeDimensionalRange(0, 4.5, 2.5, 2.5, -3.4, 0);
    ThreeDimensionalRange ballPosRange = new ThreeDimensionalRange(-5, -3, 2.8, 2.8, -1.2, -0.4);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _inputData = GetComponent<InputData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputData._leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool primaryIsPressed) && primaryIsPressed && !primaryAlreadyPressed)
        {
            ExecuteSettingBall();
            primaryAlreadyPressed = true;
        } else
        {
            primaryAlreadyPressed = false;
        }
        if (_inputData._leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool secondaryIsPressed) && secondaryIsPressed && !secondaryAlreadyPressed)
        {
            ResetBallToSetter();
            secondaryAlreadyPressed = true;
        } else 
        {
            secondaryAlreadyPressed = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteSettingBall();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ResetBallToSetter();
        }
    }

    void ExecuteSettingBall() {
        rb.useGravity = true;
        double height = heightRange.getRandomNumberInRange();
        Vector3 endPos = ballPosRange.getRandomPointInRange();
        SetBallVelocity(height, endPos.x, endPos.y, endPos.z);
    }

    void ResetBallToSetter()
    {
        rb.useGravity = false;
        rb.position = setterPosRange.getRandomPointInRange();
        rb.velocity = new Vector3(0f, 0f, 0f);
    }

    void SetBallVelocity(double h, double x, double y, double z) {
        double g = 9.81d;
        double vy = Math.Sqrt(2 * g * (h - rb.position.y));
        double t1 = vy / g;
        double t2 = Math.Sqrt(2 * (h - y) / g);
        double vx = (x - rb.position.x) / (t1 + t2);
        double vz = (z - rb.position.z) / (t1 + t2);
        rb.velocity = new Vector3((float)vx, (float)vy, (float)vz);
    }
}
