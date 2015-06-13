// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AiMission.cs" company="nevada_scout">
//   Copyright (c) nevada_scout 2015. All Rights Reserved.
//   This code is part of the GrandTheftApocalypse mod for GTA V.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GrandTheftApocalypse.Story.MissionEngine
{
    using GTA.Math;

    public class AiMission
    {
        public Mission Mission { get; set; }

        public int StoryOrder { get; set; }

        public Vector3 TriggerPosition { get; set; }

        public bool Complete { get; set; }
    }
}