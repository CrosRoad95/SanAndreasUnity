using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SanAndreasUnity.Behaviours;
using SanAndreasUnity.Behaviours.Vehicles;

namespace SanAndreasUnity.Stats
{
    public class MiscStats : MonoBehaviour
    {
        
        void Start()
        {
            Utilities.Stats.RegisterStat(new Utilities.Stats.Entry(){category = "MISC", onGUI = OnStatGUI});
        }

        void OnStatGUI()
        {

            GUILayout.Label("Num peds: " + Ped.NumPeds);
            GUILayout.Label("Num vehicles: " + Vehicle.NumVehicles);
            GUILayout.Label("Num ped state changes received: " + Ped.NumStateChangesReceived);

            var sb = new System.Text.StringBuilder();

            // info about local ped

            var ped = Ped.Instance;
            if (ped != null)
            {
                sb.AppendFormat("Local ped:\n");
                sb.AppendFormat("position: {0}\n", ped.transform.position);
                sb.AppendFormat("net id: {0}\n", ped.netId);
                sb.AppendFormat("sync interval: {0}\n", ped.NetTransform.syncInterval);
                sb.AppendFormat("state: {0}\n", ped.CurrentState != null ? ped.CurrentState.GetType().Name : "");
                sb.AppendFormat("velocity: {0}\n", ped.Velocity);
                sb.AppendFormat("is grounded: {0}\n", ped.IsGrounded);
                sb.AppendFormat("model id: {0}\n", ped.PedDef != null ? ped.PedDef.Id.ToString() : "");
                sb.AppendFormat("model name: {0}\n", ped.PedDef != null ? ped.PedDef.ModelName : "");
                sb.AppendFormat("\n");

                // info about current vehicle

                var vehicle = ped.CurrentVehicle;
                if (vehicle != null)
                {
                    List<System.Object> objects = new List<System.Object>(){
                        vehicle.Velocity,
                        vehicle.Accelerator,
                        vehicle.Braking,
                        vehicle.Steering,
                        vehicle.AverageWheelHeight,
                        vehicle.NetTransform.netId,
                        vehicle.NetTransform.syncInterval,
                        vehicle.NetTransform.ComponentIndex,
                    };

                    var texts = new List<string>() {"velocity", "accelerator", "braking", "steering", "average wheel height", 
                        "net id", "sync interval", "component index"};


                    texts.Add("wheels");
                    objects.Add("");
                    foreach (var w in vehicle.Wheels)
                    {
                        texts.Add("\t" + w.Alignment);
                        objects.Add( string.Format("travel {0} rpm {1} radius {2} motor torque {3} mass {4} is grounded {5}", 
                            w.Travel, w.Collider.rpm, w.Collider.radius, w.Collider.motorTorque, w.Collider.mass, w.Collider.isGrounded) );

                    }

                    if (vehicle.Definition != null)
                    {
                        var def = vehicle.Definition;
                        texts.Add("game name");
                        texts.Add("type");
                        objects.Add(def.GameName);
                        objects.Add(def.VehicleType);
                    }

                    texts.Add("rigid body");
                    objects.Add("");
                    if (vehicle.RigidBody != null)
                    {
                        var rb = vehicle.RigidBody;
                        texts.AddRange(new string[]{"\tmass", "\tvelocity", "\tangular velocity"});
                        objects.AddRange(new object[]{rb.mass, rb.velocity, rb.angularVelocity});
                    }

                    texts.Add("seats");
                    objects.Add("");
                    foreach(var seat in vehicle.Seats)
                    {
                        texts.Add("\t" + seat.Alignment);
                        var p = seat.OccupyingPed;
                        objects.Add(p != null ? ("ped: net id " + p.netId) : "empty");
                    }

                    var closestSeat = vehicle.FindClosestSeatTransform(ped.transform.position);
                    if (closestSeat != null)
                    {
                        texts.Add("distance to closest seat");
                        objects.Add(Vector3.Distance(closestSeat.position, ped.transform.position));
                    }


                    sb.AppendFormat("Current vehicle:\n");
                    for (int i = 0; i < objects.Count; i++)
                    {
                        sb.AppendFormat("{0}: {1}\n", texts[i], objects[i]);
                    }

                }

            }


        }

    }
}
