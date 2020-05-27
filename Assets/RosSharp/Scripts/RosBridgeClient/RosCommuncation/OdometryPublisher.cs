using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class OdometryPublisher : UnityPublisher<MessageTypes.Nav.Odometry>
    {
        public Transform PublishedTransform;
        public string FrameId = "base_link";

        private MessageTypes.Nav.Odometry message;
        private float previousRealTime;

        private float previousEuler;

        private Vector3 previousPosition;

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
            message = new MessageTypes.Nav.Odometry
            {
                header = new MessageTypes.Std.Header()
                {
                    frame_id = FrameId
                }
            };
        }

        private void UpdateMessage()
        {
            message.header.Update();

            float deltaTime = Time.realtimeSinceStartup - previousRealTime;

            double [] cov = {0.01, 0.0, 0.0, 0.0, 0.0, 0.0,
                         0.0, 0.01, 0.0, 0.0, 0.0, 0.0,
                         0.0, 0.0, 0.01, 0.0, 0.0, 0.0,
                         0.0, 0.0, 0.0, 0.0001, 0.0, 0.0,
                         0.0, 0.0, 0.0, 0.0, 0.0001, 0.0,
                         0.0, 0.0, 0.0, 0.0, 0.0, 0.0001};

            GetGeometryPoint(PublishedTransform.localPosition.Unity2Ros(), message.pose.pose.position);
            GetGeometryQuaternion(PublishedTransform.localRotation.Unity2Ros(), message.pose.pose.orientation);
            message.pose.covariance = cov;

            GetVelocityLinear(PublishedTransform.localPosition.Unity2Ros(), message.twist.twist.linear, deltaTime);
            GetVelocityAngular(PublishedTransform.localRotation.Unity2Ros(), message.twist.twist.angular, deltaTime);
            message.twist.covariance = cov;

            Publish(message);

            previousRealTime = Time.realtimeSinceStartup;
        }

        private static void GetGeometryPoint(Vector3 position, MessageTypes.Geometry.Point geometryPoint)
        {
            geometryPoint.x = position.x;
            geometryPoint.y = position.y;
            geometryPoint.z = position.z;
        }

        private static void GetGeometryQuaternion(Quaternion quaternion, MessageTypes.Geometry.Quaternion geometryQuaternion)
        {
            geometryQuaternion.x = quaternion.x;
            geometryQuaternion.y = quaternion.y;
            geometryQuaternion.z = quaternion.z;
            geometryQuaternion.w = quaternion.w;
        }

        private void GetVelocityLinear(Vector3 position, MessageTypes.Geometry.Vector3 geometryPoint, float dt)
        {
            geometryPoint.x = (position.x - previousPosition.x)/dt;
            geometryPoint.y = (position.y - previousPosition.y)/dt;
            geometryPoint.z = (position.z - previousPosition.z)/dt;

            previousPosition = position;
        }

        private void GetVelocityAngular(Quaternion quaternion, MessageTypes.Geometry.Vector3 geometryPoint, float dt)
        {
            float euler = quaternion.eulerAngles[2] * (Mathf.PI / 180);

            float th = (euler - previousEuler)/dt;

            geometryPoint.x = 0;
            geometryPoint.y = 0;
            geometryPoint.z = th;

            previousEuler = euler;
        }

    }
}

