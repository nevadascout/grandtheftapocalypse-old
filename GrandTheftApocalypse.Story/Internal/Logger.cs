// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="nevada_scout">
//   Copyright (c) nevada_scout 2015. All Rights Reserved.
//   This code is part of the GrandTheftApocalypse mod for GTA V.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GrandTheftApocalypse.Story.Internal
{
    using System;
    using System.IO;

    /// <summary>
    /// Static logger class that allows direct logging of anything to a text file
    /// </summary>
    public static class Logger
    {
        public static void Log(object message)
        {
            File.AppendAllText("GrandTheftApocalypse.log", DateTime.Now + " : " + message + Environment.NewLine);
        }
    }
}
