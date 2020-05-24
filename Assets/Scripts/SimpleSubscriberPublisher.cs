using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;

public class SimpleSubscriberPublisher : MonoBehaviour
{
    [Header("ROS IP")]
    public String rosIP = "ws://10.0.1.24:9090";

    [Header("Subscriber Topics- Float")]
    public String turretVelocity = "/turret/cmd_vel";
    public String turretVerticalOffset = "/turret/y_offset";
    public String flywheelWantedRpm = "/auto/flywheel/wanted/rpm";
    public String hoodWantedAngle = "/auto/hood/wanted/angle";
    public String turretWantedAngle = "/auto/turret/wanted/angle";

    [Header("Subscriber Topics- String")]
    public String shooterState = "/auto/shooter/state";
    public String flywheelState = "/auto/flywheel/state";
    public String turretState = "/auto/turret/state";
    public String hoodState = "/auto/hood/state";
    public String intakeState = "/auto/intake/state";

    [Header("Publisher Topics- Boolean")]
    public String autoState = "/auto/state";
    public String turretPrimed = "/turret/primed";

    [Header("Publisher Topics- Float")]
    public String autoSelect = "/auto/select";
    public String flywheelCurrentRpm = "/auto/flywheel/current/rpm";
    public String hoodCurrentAngle = "/auto/hood/current/angle";
    public String turretCurrentAngle = "/auto/turret/current/angle";

    public static void Main(string[] args)
    {
        // WebSocket Connection:
        RosSocket rosSocket = new RosSocket(new RosBridgeClient.Protocols.WebSocketNetProtocol(rosIp));

        // Publication:
        string publication_id = rosSocket.Advertise<std_msgs.String>("publication_test");
        std_msgs.String message = new std_msgs.String("publication test message data");
        rosSocket.Publish(publication_id, message);

        // Subscription:
        string subscription_id = rosSocket.Subscribe<std_msgs.String>("/subscription_test", SubscriptionHandler);
        subscription_id = rosSocket.Subscribe<std_msgs.String>("/subscription_test", SubscriptionHandler);

        Console.WriteLine("Press any key to close...");
        Console.ReadKey(true);
        rosSocket.Close();
    }

    private static void SubscriptionHandler(std_msgs.String message)
    {
        Console.WriteLine((message).data);
    }
}
