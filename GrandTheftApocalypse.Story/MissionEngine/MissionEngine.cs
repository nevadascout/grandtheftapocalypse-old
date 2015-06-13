// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissionEngine.cs" company="nevada_scout">
//   Copyright (c) nevada_scout 2015. All Rights Reserved.
//   This code is part of the GrandTheftApocalypse mod for GTA V.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GrandTheftApocalypse.Story.MissionEngine
{
    using GrandTheftApocalypse.Story.Internal;

    public class MissionEngine
    {
        public void StartMission(Mission mission)
        {
            switch (mission)
            {
                case Mission.STORY_Intro:
                    Logger.Log("Started story mission 'Intro'...");
                    break;
            }
        }
    }
}
