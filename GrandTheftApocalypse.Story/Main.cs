// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.cs" company="nevada_scout">
//   Copyright (c) nevada_scout 2015. All Rights Reserved.
//   This code is part of the GrandTheftApocalypse mod for GTA V.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GrandTheftApocalypse.Story
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using GrandTheftApocalypse.Story.Internal;
    using GrandTheftApocalypse.Story.Peds;

    using GTA;
    using GTA.Native;

    public class Main : Script
    {
        private PlayerProgress playerProgress;

        private bool DEBUG = true;

        private bool isNight;
        private float zedSpeed = 1f; // 1f = Walk, 2f = jog, 3f = sprint
        private int zedDamage = 10;

        private int maxAmbientZeds = 5;
        private int maxHoardZeds = 25;

        private List<Ped> zombies = new List<Ped>();
        private List<Ped> friendlies = new List<Ped>();
        private List<Ped> enemies = new List<Ped>();

        private DateTime lastZedUpdate = DateTime.Now;

        public Main()
        {
            this.Tick += this.onTick;
            this.KeyDown += this.onKeyDown;

            // Load player save
            this.playerProgress = SaveFile.Load();

            this.ApplyLoadedSave();

            var firstStoryMission = this.playerProgress.Missions.FirstOrDefault(p => p.StoryOrder == 1 && p.Complete == false);
            if (firstStoryMission != null)
            {
                // Start first mission
            }
        }

        private void onTick(object sender, EventArgs eventArgs)
        {
            this.DisableWantedLevel();
            
            // Change zombie behaviour during the night
            this.CheckNightStatus();

            this.ManageZeds();

            this.MakeZombiesDrunk();
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            // Save player progress to file when F8 is pressed.
            if (e.KeyCode == Keys.F8)
            {
                SaveFile.Save(this.playerProgress);
            }

            if (this.DEBUG)
            {
                if (e.KeyCode == Keys.F6)
                {
                    UI.Notify(this.isNight + " :: " + this.zedSpeed);
                }

                if (e.KeyCode == Keys.F5)
                {
                    if (Game.Player.Character.IsInVehicle())
                    {
                        UI.Notify("Saved vehicle position to log");

                        var vehName = Game.Player.Character.CurrentVehicle.DisplayName;
                        var position = Game.Player.Character.CurrentVehicle.Position;
                        var heading = Game.Player.Character.CurrentVehicle.Heading;
                        Logger.Log(string.Format("VEHICLE {0} POS: + {1} ## HEADING: {2}", vehName, position, heading));
                    }
                    else
                    {
                        UI.Notify("Saved player position to log");

                        var position = Game.Player.Character.Position;
                        var heading = Game.Player.Character.Heading;
                        Logger.Log(string.Format("PLAYER POS: + {0} ## HEADING: {1}", position, heading));
                    }
                }
            }
        }


        #region Helpers

        private void DisableWantedLevel()
        {
            // Disable wanted levels
            if (Game.Player.WantedLevel != 0)
            {
                Game.Player.WantedLevel = 0;
            }
        }

        private void CheckNightStatus()
        {
            // Set "night" bool between 7pm and 5am
            if (World.CurrentDayTime <= TimeSpan.FromHours(4) || World.CurrentDayTime >= TimeSpan.FromHours(19))
            {
                this.isNight = true;

                // Increase zombie speed + damage at night
                this.zedSpeed = 3f; // Sprint
                this.zedDamage = 25;
                this.maxAmbientZeds = 15;
            }
            else
            {
                this.isNight = false;

                // Decrease zombie speed + damage during daylight hours
                this.zedSpeed = 1f; // Walk
                this.zedDamage = 10;
                this.maxAmbientZeds = 5;
            }
        }
        
        private void MakeZombiesDrunk()
        {
            foreach (var zombie in this.zombies)
            {
                if (!Function.Call<bool>(Hash.HAS_ANIM_SET_LOADED, "move_m@drunk@verydrunk"))
                {
                    Function.Call(Hash.REQUEST_ANIM_SET, "move_m@drunk@verydrunk");
                }
                
                Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, zombie.Handle, "move_m@drunk@verydrunk", 0x3E800000); // 0x3E800000 = 0.25f);
            }
        }

        private void ApplyLoadedSave()
        {
            // Set player position
            // Set vehicles
        }

        private void ManageZeds()
        {
            var player = Game.Player.Character;

            // Spawn one new zed per 2 seconds
            if (DateTime.Now > this.lastZedUpdate)
            {
                if (this.zombies.Count() < this.maxAmbientZeds)
                {
                    // Check player is on main land
                    var zed = Zombie.Spawn();
                    this.zombies.Add(zed);
                }

                //this.lastZedUpdate = DateTime.Now + TimeSpan.FromMilliseconds(2000);
                this.lastZedUpdate = DateTime.Now + TimeSpan.FromMilliseconds(500);
                // TODO -- Check if this value is too low (changing from 2000 to 500 caused a zlib error almost immediately. Could be coincidence, not sure.
            }

            for (int i = this.zombies.Count - 1; i >= 0; i--)
            {
                var distanceToPlayer = this.zombies[i].Position.DistanceTo(player.Position);

                if (distanceToPlayer >= 150f || this.zombies[i].IsDead)
                {
                    this.zombies.RemoveAt(i);

                    // if (this.DEBUG) UI.Notify("CLEANUP ZED");
                }
                else
                {
                    // Pathfind towards player
                    var zed = this.zombies[i];
                    var playerPos = player.Position;

                    Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, zed.Handle, playerPos.X, playerPos.Y, playerPos.Z, this.zedSpeed, -1, 0f, 0f);
                }
            }
        }

        #endregion
    }
}
