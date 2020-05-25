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
using System;
using System.Collections;

namespace RosSharp.RosBridgeClient
{
    public class TwistSubscriber : UnitySubscriber<MessageTypes.Geometry.Twist>
    {
        public Transform SubscribedTransform;
        public bool reliesOnRateOfPublish = false;
        public bool usesRollingAverage = true;

        public int rollingLinearLength = 20;
        public int rollingAngularLength = 20;

        private float previousRealTime;
        private Vector3 linearVelocity;
        private Vector3 angularVelocity;
        private bool isMessageReceived;

        private ArrayList rollingAverageLinear = new ArrayList();
        private ArrayList rollingAverageAngular = new ArrayList();

        protected override void Start()
        {
            base.Start();
            if (usesRollingAverage)
            {
                for (int x = 0; x < rollingLinearLength; x++)
                {
                    rollingAverageLinear.Add(new Vector3());
                }

                for (int x = 0; x < rollingAngularLength; x++)
                {
                    rollingAverageAngular.Add(0.0f);
                }
            }
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

            if (usesRollingAverage)
            {
                rollingAverageLinear.Insert(0, linearVelocity);
                rollingAverageLinear.RemoveAt(rollingAverageLinear.Count - 1);

                Vector3 linearAverage = Vector3.zero;
                for (int i = 0; i < rollingAverageLinear.Count; i++)
                {
                    linearAverage = linearAverage + (Vector3)rollingAverageLinear[i];
                }
                SubscribedTransform.Translate((linearAverage/ rollingAverageLinear.Count) * deltaTime);

                float angVelX = (angularVelocity.x / (Mathf.PI / 180)) * deltaTime;
                float angVelY = (angularVelocity.y / (Mathf.PI / 180)) * deltaTime;
                float angVelZ = (angularVelocity.z / (Mathf.PI / 180)) * deltaTime;

                rollingAverageAngular.Insert(0, angVelY);
                rollingAverageAngular.RemoveAt(rollingAverageAngular.Count - 1);

                float angularAverage = 0.0f;
                for (int i = 0; i < rollingAverageAngular.Count; i++)
                {
                    angularAverage += (float)rollingAverageAngular[i];
                }
                SubscribedTransform.Rotate(Vector3.forward, angVelX);
                SubscribedTransform.Rotate(Vector3.up, angularAverage / (float)rollingAverageAngular.Count);
                SubscribedTransform.Rotate(Vector3.left, angVelZ);
            }
            else
            {
                SubscribedTransform.Translate(linearVelocity * deltaTime);

                float angVelX = (angularVelocity.x / (Mathf.PI / 180)) * deltaTime;
                float angVelY = (angularVelocity.y / (Mathf.PI / 180)) * deltaTime;
                float angVelZ = (angularVelocity.z / (Mathf.PI / 180)) * deltaTime;
                SubscribedTransform.Rotate(Vector3.forward, angVelX);
                SubscribedTransform.Rotate(Vector3.up, angVelY);
                SubscribedTransform.Rotate(Vector3.left, angVelZ);
            }

            previousRealTime = Time.realtimeSinceStartup;

            isMessageReceived = false;
        }
    }
}