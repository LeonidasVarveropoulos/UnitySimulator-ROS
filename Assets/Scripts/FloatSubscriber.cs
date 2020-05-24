using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class FloatSubscriber : UnitySubscriber<MessageTypes.Std.Float32>
    {
        private float messageData = 0.0f;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Std.Float32 message)
        {
            messageData = message.data;
        }

        public float getData()
        {
            return messageData;
        }
    }
}
