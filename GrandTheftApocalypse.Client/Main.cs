// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.cs" company="nevada_scout">
//   Copyright (c) nevada_scout 2015. All Rights Reserved.
//   This code is part of the GrandTheftApocalypse mod for GTA V.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GrandTheftApocalypse.Client
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using GTA;
    using GTA.Math;
    using GTA.Native;

    public class Main : Script
    {
        private bool playerInvincible;

        private bool zedsAreAggroed;

        private bool isNight;

        private List<Ped> zombies = new List<Ped>();

        private int zedLimit = 50;


        public Main()
        {
            this.Tick += this.onTick;
            this.KeyUp += this.onKeyUp;

            // Set up world
            GTA.World.Weather = Weather.Foggy;
            GTA.World.CurrentDayTime = TimeSpan.FromHours(5.5);

            // Change player model to "Hunter"
            World.Helpers.SetPlayerModel(PedHash.Hunter);

            // Teleport to remote location
            Game.Player.Character.Position = new Vector3(402.807f, 3175.139f, 53.14062f);
            Game.Player.Character.Heading = 90f;

            // Give the player some weapons
            Game.Player.Character.Weapons.RemoveAll();
            Game.Player.Character.Weapons.Give(WeaponHash.SniperRifle, 150, true, true);
            Game.Player.Character.Weapons.Give(WeaponHash.GolfClub, 1, false, true);
            Game.Player.Character.Weapons.Give(WeaponHash.CarbineRifle, 2500, false, true);
            Game.Player.Character.Weapons.Give(WeaponHash.PumpShotgun, 1500, false, true);

            // Spawn vehicle
            World.Helpers.CreateDrivableVehicle(VehicleHash.Dubsta, new Vector3(382.2398f, 3181.735f, 52.69014f), Color.Black);
        }

        private void onTick(object sender, EventArgs e)
        {
            var playerPos = Game.Player.Character.Position;

            // Disable wanted levels
            if (Game.Player.WantedLevel != 0)
            {
                Game.Player.WantedLevel = 0;
            }

            // Set "night" bool between 7pm and 5am
            if (GTA.World.CurrentDayTime <= TimeSpan.FromHours(4) || GTA.World.CurrentDayTime >= TimeSpan.FromHours(19))
            {
                this.isNight = true;
            }

            // Values for during day
            float zedSpeed = 1f; // Walk
            int zedDamage = 10;
            
            // Increase zombie speed + damage at night
            if (this.isNight)
            {
                zedSpeed = 3f; // Sprint
                zedDamage = 25;
            }

            // Despawn zeds if they're too far away from the player
            for (int i = this.zombies.Count - 1; i >= 0; i--)
            {
                if (this.zombies[i].Position.DistanceTo(Game.Player.Character.Position) >= 250f)
                {
                    this.zombies.RemoveAt(i);
                    this.zombies[i].Delete();
                }
            }

            // TODO -- Clean up deleted zeds

            // Spawn zeds if we're below the limit
            if (this.zombies.Count < this.zedLimit)
            {
                this.SpawnZombie();
            }

            foreach (var zombie in this.zombies)
            {
                if (this.zedsAreAggroed)
                {
                    // Make zeds move towards player
                    // TODO -- replace this with pathfinding so that they go around obstacles
                    Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, zombie.Handle, playerPos.X, playerPos.Y, playerPos.Z, zedSpeed, -1, 0f, 0f);
                }

                // Damage player if the zeds are too close
                // TODO -- Add check if zombie is facing the player
                if (!Game.Player.Character.IsInVehicle() && zombie.IsTouching(Game.Player.Character) && !zombie.IsDead)
                {
                    // Make the player actually take damage, instead of just removing health
                    //Game.Player.Character.Health -= zedDamage;
                    UI.Notify("Taken Damage: " + zedDamage);
                }
                
                // If the player is in a car, drag them out
                if (Game.Player.Character.IsInVehicle() && zombie.IsTouching(Game.Player.Character))
                {
                    // TODO
                    //var seat = Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, Game.Player.Character.CurrentVehicle, (int)VehicleSeat.Driver) == Game.Player.Character;
                    //zombie.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, Game.Player.Character.CurrentVehicle.Seat);
                }
                
                // zombie.Position.DistanceTo(Game.Player.Character.Position) <= 0.2f)
            }
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                UI.Notify("GameTime:" + Game.GameTime);
                UI.Notify("IsNight: " + this.isNight);
            }

            if (e.KeyCode == Keys.F5)
            {
                if (this.playerInvincible)
                {
                    Game.Player.Character.IsInvincible = false;
                    this.playerInvincible = false;
                }
                else
                {
                    Game.Player.Character.IsInvincible = true;
                    this.playerInvincible = true;
                }
            }

            if (e.KeyCode == Keys.F6)
            {
                World.Helpers.CreatePropInFrontOfPlayer("prop_beach_fire");
            }

            if (e.KeyCode == Keys.F7)
            {
                var truck = World.Helpers.CreateDrivableVehicle(
                    VehicleHash.Bison,
                    Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 5, 0)),
                    Color.Green,
                    Game.Player.Character.Heading + 90);
                Blip blip = truck.AddBlip();
                blip.Color = BlipColor.Blue;
                blip.Scale = 0.8f;

                var trailer = World.Helpers.CreateDrivableVehicle(
                    VehicleHash.TrailerSmall,
                    truck.GetOffsetInWorldCoords(new Vector3(0, -5, 0)),
                    Color.Green,
                    Game.Player.Character.Heading + 90);
                Blip blip2 = trailer.AddBlip();
                blip2.Color = BlipColor.Yellow;
                blip2.Scale = 0.6f;
            }

            if (e.KeyCode == Keys.F8)
            {
                this.zedsAreAggroed = !this.zedsAreAggroed;
            }
        }

        private void SpawnZombie()
        {
            var ped = GTA.World.CreatePed(PedHash.DeadHooker, Game.Player.Character.Position.Around(75));
            Function.Call(Hash.SET_PED_IS_DRUNK, ped, 1);

            // TODO -- NOT WORKING! :(
            Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped.Handle, "move_m@drunk@verydrunk", 0x3E800000); // 0x3E800000 = 0.25f);

            this.zombies.Add(ped);
        }
    }
}
