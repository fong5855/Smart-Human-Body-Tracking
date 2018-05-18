using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanTransform : MonoBehaviour {
    Transform root;
    Transform[] joints;
    enum JOINT
    {
        HEAD, NECK, TORSO, LEFT_SHOULDER, LEFT_ELBOW, LEFT_HAND, RIGHT_SHOULDER, RIGHT_ELBOW, RIGHT_HAND,
        LEFT_HIP, LEFT_KNEE, LEFT_FOOT, RIGHT_HIP, RIGHT_KNEE, RIGHT_FOOT
    }
    // Use this for initialization
    void Start()
    {
        root = GameObject.Find("unitychan").transform;
        joints = new Transform[15];
        joints[(int)JOINT.HEAD] = GameObject.Find("Character1_Head").transform;
        joints[(int)JOINT.NECK] = GameObject.Find("Character1_Neck").transform;
        joints[(int)JOINT.TORSO] = GameObject.Find("Character1_Hips").transform;
        joints[(int)JOINT.LEFT_SHOULDER] = GameObject.Find("Character1_LeftArm").transform;
        joints[(int)JOINT.LEFT_ELBOW] = GameObject.Find("Character1_LeftForeArm").transform;
        joints[(int)JOINT.LEFT_HAND] = GameObject.Find("Character1_LeftHand").transform;
        joints[(int)JOINT.RIGHT_SHOULDER] = GameObject.Find("Character1_RightArm").transform;
        joints[(int)JOINT.RIGHT_ELBOW] = GameObject.Find("Character1_RightForeArm").transform;
        joints[(int)JOINT.RIGHT_HAND] = GameObject.Find("Character1_RightHand").transform;
        joints[(int)JOINT.LEFT_HIP] = GameObject.Find("Character1_LeftUpLeg").transform;
        joints[(int)JOINT.LEFT_KNEE] = GameObject.Find("Character1_LeftLeg").transform;
        joints[(int)JOINT.LEFT_FOOT] = GameObject.Find("Character1_LeftFoot").transform;
        joints[(int)JOINT.RIGHT_HIP] = GameObject.Find("Character1_RightUpLeg").transform;
        joints[(int)JOINT.RIGHT_KNEE] = GameObject.Find("Character1_RightLeg").transform;
        joints[(int)JOINT.RIGHT_FOOT] = GameObject.Find("Character1_RightFoot").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateTorsoPosition(object obj, byte[] recv)
    {
        float tx = System.BitConverter.ToSingle(recv, 0);
        float ty = System.BitConverter.ToSingle(recv, 4);
        float tz = System.BitConverter.ToSingle(recv, 8);
        //trans.position = t;
        joints[(int)JOINT.TORSO].position = root.position + new Vector3(tx,ty,tz);
    }
    private void UpdateRotation(Transform trans, Vector3 euler)
    {
        //trans.position = t;
        
        trans.rotation = Quaternion.Euler(euler);
    }
    public void UpdatePoses(object obj, byte[] recv)
    {
        for (int i = 0; i < 15; i++)
        {
            Vector3 tmpq;
            //if (i == (int)JOINT.RIGHT_HAND || i == (int)JOINT.LEFT_HAND || i == (int)JOINT.LEFT_FOOT || i == (int)JOINT.RIGHT_FOOT) continue;
            if ((int)JOINT.RIGHT_HAND == i || (int)JOINT.LEFT_HAND == i)
            {
                float qx = System.BitConverter.ToSingle(recv, (i-1) * 16 + 4);
                float qy = System.BitConverter.ToSingle(recv, (i - 1) * 16 + 8);
                float qz = System.BitConverter.ToSingle(recv, (i - 1) * 16 + 12);
                float qw = System.BitConverter.ToSingle(recv, (i - 1) * 16 + 0);
                tmpq = new Quaternion(qx, -qy, qz, -qw).eulerAngles;
            }
            else
            {
                float qx = System.BitConverter.ToSingle(recv, i * 16 + 4);
                float qy = System.BitConverter.ToSingle(recv, i * 16 + 8);
                float qz = System.BitConverter.ToSingle(recv, i * 16 + 12);
                float qw = System.BitConverter.ToSingle(recv, i * 16 + 0);
                tmpq = new Quaternion(qx, -qy, qz, -qw).eulerAngles;
            }
 
            if (i == 1)
            {
                Debug.Log(tmpq.ToString("0.0000"));
            }
            if ((int)JOINT.TORSO == i) UpdateRotation(joints[i], new Vector3(-tmpq.z, -tmpq.y-90, -tmpq.x - 90));
            else if ((int)JOINT.NECK == i) UpdateRotation(joints[i], new Vector3(-tmpq.z ,-tmpq.y - 90, -tmpq.x - 90));
            else if ((int)JOINT.HEAD == i) UpdateRotation(joints[i], new Vector3(-tmpq.z, -tmpq.y - 90, -tmpq.x - 80));
            else if ((int)JOINT.LEFT_SHOULDER == i) UpdateRotation(joints[i], new Vector3(tmpq.x +150, -tmpq.y, tmpq.z));
            else if ((int)JOINT.LEFT_ELBOW == i) UpdateRotation(joints[i],  new Vector3(tmpq.x + 150, -tmpq.y, tmpq.z - 0));
            else if ((int)JOINT.LEFT_HAND == i) UpdateRotation(joints[i], new Vector3(tmpq.x + 150, -tmpq.y, tmpq.z));
            else if ((int)JOINT.RIGHT_SHOULDER == i) UpdateRotation(joints[i],  new Vector3(-tmpq.x, -tmpq.y + 180, tmpq.z ));
            else if ((int)JOINT.RIGHT_ELBOW == i) UpdateRotation(joints[i], new Vector3(-tmpq.x , -tmpq.y + 180, tmpq.z ));
            else if ((int)JOINT.RIGHT_HAND == i) UpdateRotation(joints[i], new Vector3(-tmpq.x, -tmpq.y + 180, tmpq.z));
            else if ((int)JOINT.LEFT_HIP == i) UpdateRotation(joints[i], new Vector3(tmpq.z, -tmpq.y + 90, tmpq.x  +90));
            else if ((int)JOINT.LEFT_KNEE == i) UpdateRotation(joints[i],  new Vector3(tmpq.z, -tmpq.y + 90, tmpq.x+ 90));
            else if ((int)JOINT.LEFT_FOOT == i) UpdateRotation(joints[i],  new Vector3(tmpq.z+180, -tmpq.y + 90, tmpq.x+ 60));
            else if ((int)JOINT.RIGHT_HIP == i) UpdateRotation(joints[i],  new Vector3(tmpq.z, -tmpq.y + 90, tmpq.x  +90));
            else if ((int)JOINT.RIGHT_KNEE == i) UpdateRotation(joints[i], new Vector3(tmpq.z, -tmpq.y + 90, tmpq.x+ 90));
            else if ((int)JOINT.RIGHT_FOOT == i) UpdateRotation(joints[i], new Vector3(tmpq.z + 180, -tmpq.y + 90, tmpq.x+ 60));
            else UpdateRotation(joints[i], new Vector3(tmpq.x, tmpq.y, tmpq.z));
            
        }
        float tx = System.BitConverter.ToSingle(recv, 240);
        float ty = System.BitConverter.ToSingle(recv, 244);
        float tz = System.BitConverter.ToSingle(recv, 248);
        //trans.position = t;
        joints[(int)JOINT.TORSO].position = root.position + new Vector3(tx, ty, -tz);

    }
}
