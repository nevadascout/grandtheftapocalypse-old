// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerProgress.cs" company="nevada_scout">
//   Copyright (c) nevada_scout 2015. All Rights Reserved.
//   This code is part of the GrandTheftApocalypse mod for GTA V.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GrandTheftApocalypse.Story.Internal
{
    using System.Collections.Generic;

    using GrandTheftApocalypse.Story.MissionEngine;

    using GTA.Math;

    public class PlayerProgress
    {
        #region Player

        public Vector3 PlayerPosition { get; set; }

        public int PlayerHealth { get; set; }

        // Player Weapons

        // Player inventory (snacks, health, armour, etc)
        
        #endregion


        #region World

        // Missions completed
        public List<AiMission> Missions { get; set; }

        // Safe zones + their progress

        // Vehicles

        #endregion
    }
}
