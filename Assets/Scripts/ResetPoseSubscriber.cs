using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class ResetPoseSubscriber : UnitySubscriber<MessageTypes.Std.Float32MultiArray>
    {
        public Transform SubscribedTransform;
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
                Debug.Log(SubscribedTransform.localPosition);
                SubscribedTransform.localPosition = new Vector3((float)-messageData[1], (float)SubscribedTransform.localPosition.y, (float)messageData[0]);
                SubscribedTransform.localEulerAngles = new Vector3(0, messageData[2] * 180f/Mathf.PI, 0);
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
