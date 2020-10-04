using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class DcMotorInputSubscriber : UnitySubscriber<MessageTypes.Ftc.DcMotorInput>
    {
        private float messageCmd = 0.0f;
        public string messageMode = "";

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Ftc.DcMotorInput message)
        {
            messageCmd = message.cmd;
            messageMode = message.mode;
        }

        public float getCmd()
        {
            return messageCmd;
        }

        public string getMode()
        {
            return messageMode;
        }
    }
}
