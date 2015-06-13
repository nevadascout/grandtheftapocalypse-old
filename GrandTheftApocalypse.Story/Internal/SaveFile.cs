// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveFile.cs" company="nevada_scout">
//   Copyright (c) nevada_scout 2015. All Rights Reserved.
//   This code is part of the GrandTheftApocalypse mod for GTA V.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GrandTheftApocalypse.Story.Internal
{
    using System.Collections.Generic;

    using GrandTheftApocalypse.Story.MissionEngine;

    using GTA;
    using GTA.Math;

    public class SaveFile
    {
        public static void Save(PlayerProgress playerProgress)
        {
            // Create file if not exists
        }

        public static PlayerProgress Load()
        {
            var progress = new PlayerProgress();

            progress.Missions = new List<AiMission>();

            // Mock out for testing...
            progress.Missions.Add(new AiMission
                                      {
                                          Mission = Mission.STORY_Intro,
                                          StoryOrder = 1,
                                          Complete = false
                                      });

            // TODO -- Set this to island
            //progress.PlayerPosition = new Vector3();

            // TODO -- Place campfire + tent on island

            // TODO -- Place boat

            // TODO -- Place Michael's Asea near lighthouse

            return progress;
        }
    }
}
