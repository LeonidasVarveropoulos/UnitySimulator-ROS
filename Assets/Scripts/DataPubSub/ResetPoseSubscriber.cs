using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class ResetPoseSubscriber : UnitySubscriber<MessageTypes.Std.Float32MultiArray>
    {
        public Transform GlobalTransform;
        public Transform LocalTransform;
        private float[] messageData;
        private bool gotData = false;

        protected override void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (gotData)
            {
                GlobalTransform.localPosition = new Vector3((float)-messageData[1], (float)GlobalTransform.localPosition.y, (float)messageData[0]);
                GlobalTransform.localEulerAngles = new Vector3(0, messageData[2] * 180f/Mathf.PI, 0);

                LocalTransform.localPosition = new Vector3(0.0f, (float)LocalTransform.localPosition.y, 0.0f);
                LocalTransform.localEulerAngles = new Vector3(0f, 0f, 0f);
                gotData = false;
            }
        }

        protected override void ReceiveMessage(MessageTypes.Std.Float32MultiArray message)
        {
            messageData = message.data;
            gotData = true;
        }
    }
}
