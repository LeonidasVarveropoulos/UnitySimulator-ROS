using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class HoodDataPublisher : UnityPublisher<MessageTypes.Std.Float32>
    {
        public GameObject hood;
        private HoodControl hoodData;

        private MessageTypes.Std.Float32 message;
        private float previousRealTime;

        void Awake()
        {
            hoodData = hood.GetComponent<HoodControl>();
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
            GetData(hoodData.getData(), message);

            Publish(message);

            previousRealTime = Time.realtimeSinceStartup;
        }

        private static void GetData(float msg_data, MessageTypes.Std.Float32 msg)
        {
            msg.data = msg_data;
        }

    }
}