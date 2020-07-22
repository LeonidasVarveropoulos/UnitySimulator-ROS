using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class FlywheelDataPublisher : UnityPublisher<MessageTypes.Std.Float32>
    {
        public GameObject flywheel;
        private FlywheelControl flywheelData;

        private MessageTypes.Std.Float32 message;
        private float previousRealTime;

        void Awake()
        {
            flywheelData = flywheel.GetComponent<FlywheelControl>();
        }

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
            message = new MessageTypes.Std.Float32();
        }

        private void UpdateMessage()
        {
            GetData(flywheelData.getData(), message);

            Publish(message);

            previousRealTime = Time.realtimeSinceStartup;
        }

        private static void GetData(float msg_data, MessageTypes.Std.Float32 msg)
        {
            msg.data = msg_data;
        }

    }
}