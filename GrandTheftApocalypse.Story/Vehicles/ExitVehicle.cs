// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExitVehicle.cs" company="nevada_scout">
//   Copyright (c) nevada_scout 2015. All Rights Reserved.
//   This code is part of the GrandTheftApocalypse mod for GTA V.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GrandTheftApocalypse.Story.Vehicles
{
    using System;

    using GTA;

    /// <summary>
    /// This script brings back functionality from GTA IV where holding down the "F" key while in a vehicle
    /// will turn off the engine when getting out and tapping "F" will leave the engine running.
    /// </summary>
    public class ExitVehicle : Script
    {
        private DateTime vehicleLastExit;

        public ExitVehicle()
        {
            this.Tick += this.onTick;
        }

        public void onTick(object sender, EventArgs e)
        {
            var player = Game.Player.Character;

            if (Game.IsControlPressed(2, Control.VehicleExit) && DateTime.Now > this.vehicleLastExit && player.IsInVehicle())
            {
                var vehicle = player.CurrentVehicle;
                var isDriver = vehicle.GetPedOnSeat(VehicleSeat.Driver) == player;

                Script.Wait(250);

                if (Game.IsControlPressed(2, Control.VehicleExit))
                {
                    player.Task.LeaveVehicle(vehicle, true);
                }
                else
                {
                    player.Task.LeaveVehicle(vehicle, false);

                    Script.Wait(0);

                    if (isDriver)
                    {
                        vehicle.EngineRunning = true;
                    }
                }

                this.vehicleLastExit = DateTime.Now + TimeSpan.FromMilliseconds(2000);
            }
        }
    }
}
