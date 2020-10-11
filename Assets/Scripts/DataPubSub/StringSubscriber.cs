using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class StringSubscriber : UnitySubscriber<MessageTypes.Std.String>
    {
        private string messageData = "";

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Std.String message)
        {
            messageData = message.data;
        }

        public string getData()
        {
            return messageData;
        }
    }
}
