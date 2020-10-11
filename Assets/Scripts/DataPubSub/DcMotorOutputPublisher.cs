using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class DcMotorOutputPublisher : UnityPublisher<MessageTypes.Ftc.DcMotorOutput >
    {

        private MessageTypes.Ftc.DcMotorOutput message;
        private float encoder_data = 0.0f;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void FixedUpdate()
        {
            UpdateMessage();
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Ftc.DcMotorOutput
            {
                
            };
        }

        private void UpdateMessage()
        {
            message.encoder_data = encoder_data;
            Publish(message);
        }

        public void updateEncoders(float data)
        {
            encoder_data = data;
        }
    }
}
