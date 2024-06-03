using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabboniKalman : MonoBehaviour
{/*======================
		Control Spine rotation;
	========================*/

    public const float default_Q_angle = 0.001f;
    public const float default_Q_bias = 0.1f;
    public const float default_R_measure = 0.0001f;

    private float _Q_angle;
    public float Q_angle
    {
        get
        {
            return _Q_angle;
        }
        set
        {
            _Q_angle = value;
        }
    }
    private float _Q_bias;
    public float Q_bias
    {
        get
        {
            return _Q_bias;
        }
        set
        {
            _Q_bias = value;
        }
    }
    private float _R_measure;
    public float R_measure
    {
        get
        {
            return _R_measure;
        }
        set
        {
            _R_measure = value;
        }
    }

    // private float gyroXangle, gyroYangle; // Angle calculate using the gyro only
    // private float compAngleX, compAngleY; // Calculated angle using a complementary filter
    private float kalAngleX, kalAngleY; // Calculated angle using a Kalman filter

    private float lastKalmanMoment;
    private KalmanFilter kalmanX = new KalmanFilter(); // Create the Kalman instances
    private KalmanFilter kalmanY = new KalmanFilter();

    private float roll = 0f;
    private float pitch = 0f;

    [Header("Rotation")]
    public Quaternion kalmanRotation;

    void Start()
    {
        SetQAngle(default_Q_angle);
        SetQBias(default_Q_bias);
        SetRMeasure(default_R_measure);
    }

    public void UpdateIMU(Vector3 rawAcc, Vector3 rawGyro)
    {
        GetKalmanRotation(rawAcc, rawGyro);
    }

    public void SetQAngle(float newQ_angle)
    {
        Q_angle = newQ_angle;

        kalmanX.SetQAngle(newQ_angle);
        kalmanY.SetQAngle(newQ_angle);
    }

    public void SetQBias(float newQ_bias)
    {
        Q_bias = newQ_bias;

        kalmanX.SetQBias(newQ_bias);
        kalmanY.SetQBias(newQ_bias);
    }

    public void SetRMeasure(float newR_measure)
    {
        R_measure = newR_measure;

        kalmanX.SetRMeasure(newR_measure);
        kalmanY.SetRMeasure(newR_measure);
    }

    public void ResetKalmanRotation()
    {
        kalmanX.SetAngle(0f);
        kalmanY.SetAngle(0f);

        lastKalmanMoment = Time.time;
    }

    public void ResetSpineRotation()
    {
        kalmanRotation = Quaternion.identity;
    }

    public void GetKalmanRotation(Vector3 newAcc, Vector3 newGyro)
    {
        //Unity is left-handed, IMU is right-handed
        float accX = -newAcc.x;
        float accY = newAcc.z;
        float accZ = newAcc.y;

        float gyroX = -newGyro.x;
        float gyroY = newGyro.z;
        float gyroZ = newGyro.y;

        // float dt = (float)(micros() - timer) / 1000000; // Calculate delta time
        float dt = Time.time - lastKalmanMoment; // Calculate delta time
        lastKalmanMoment = Time.time;

        // Source: http://www.freescale.com/files/sensors/doc/app_note/AN3461.pdf eq. 25 and eq. 26
        // Mathf.Atan2 outputs the value of -£k to £k (radians) - see http://en.wikipedia.org/wiki/Atan2
        // It is then converted from radians to degrees

        roll = Mathf.Atan2(accY, accZ) * Mathf.Rad2Deg;
        pitch = Mathf.Atan2(-accX, Mathf.Sqrt(accY * accY + accZ * accZ)) * Mathf.Rad2Deg;

        float gyroXrate = gyroX; // Convert to deg/s
        float gyroYrate = gyroY; // Convert to deg/s
        float gyroZrate = gyroZ; // Convert to deg/s

        if ((roll < -90f && kalAngleX > 90f) || (roll > 90f && kalAngleX < -90f))
        {
            kalmanX.SetAngle(roll);
            kalAngleX = roll;
        }
        else
            kalAngleX = kalmanX.GetAngle(roll, gyroXrate, dt); // Calculate the angle using a Kalman filter

        if (Mathf.Abs(kalAngleX) > 90f)
            gyroYrate = -gyroYrate; // Invert rate, so it fits the restriced accelerometer reading

        kalAngleY = kalmanY.GetAngle(pitch, gyroYrate, dt);

        kalmanRotation = Quaternion.AngleAxis(kalAngleX, Vector3.right) * Quaternion.AngleAxis(kalAngleY, Vector3.forward);
    }

    public float GetRoll()
    {
        return kalAngleX;
    }

    public float GetPitch()
    {
        return kalAngleY;
    }
}
