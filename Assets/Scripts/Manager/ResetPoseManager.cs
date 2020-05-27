using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class ResetPoseManager : UnitySubscriber<MessageTypes.Std.Float32MultiArray>
    {
        public Transform baseLink;
        private float[] messageData;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Std.Float32MultiArray message)
        {
            messageData = message.data;
            setPose();
        }

        private void setPose()
        {
            Debug.Log(messageData[0] + " " + messageData[1] + " " + messageData[2]);
            baseLink.localPosition = new Vector3((float)-messageData[1], (float)baseLink.transform.localPosition.z, (float)messageData[0]);
            baseLink.localEulerAngles = new Vector3(0, 0, messageData[2]);
        }
    }
}
