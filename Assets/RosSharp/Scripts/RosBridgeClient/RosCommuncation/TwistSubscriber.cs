/*
© CentraleSupelec, 2017
Author: Dr. Jeremy Fix (jeremy.fix@centralesupelec.fr)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

// Adjustments to new Publication Timing and Execution Framework
// © Siemens AG, 2018, Dr. Martin Bischoff (martin.bischoff@siemens.com)

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TwistSubscriber : UnitySubscriber<MessageTypes.Geometry.Twist>
    {
        public Transform SubscribedTransform;
        public bool reliesOnRateOfPublish = false;

        private float previousRealTime;
        private Vector3 linearVelocity;
        private Vector3 angularVelocity;
        private bool isMessageReceived;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Geometry.Twist message)
        {
            linearVelocity = ToVector3(message.linear).Ros2Unity();
            angularVelocity = -ToVector3(message.angular).Ros2Unity();
            isMessageReceived = true;
        }

        private static Vector3 ToVector3(MessageTypes.Geometry.Vector3 geometryVector3)
        {
            return new Vector3((float)geometryVector3.x, (float)geometryVector3.y, (float)geometryVector3.z);
        }

        private void Update()
        {
            if (reliesOnRateOfPublish)
            {
                if (isMessageReceived)
                {
                    ProcessMessage();
                }
            }
            else
            {
                ProcessMessage();
            }
        }
        private void ProcessMessage()
        {
            float deltaTime = Time.realtimeSinceStartup - previousRealTime;

            SubscribedTransform.Translate(linearVelocity * deltaTime);

            float angVelX = (angularVelocity.x / (Mathf.PI / 180)) * deltaTime;
            float angVelY = (angularVelocity.y / (Mathf.PI / 180)) * deltaTime;
            float angVelZ = (angularVelocity.z / (Mathf.PI / 180)) * deltaTime;
            SubscribedTransform.Rotate(Vector3.forward, angVelX);
            SubscribedTransform.Rotate(Vector3.up, angVelY);
            SubscribedTransform.Rotate(Vector3.left, angVelZ);

            previousRealTime = Time.realtimeSinceStartup;

            isMessageReceived = false;
        }
    }
}