// -----------------------------------------------------------------------
// <copyright file="DesignerPipelineDefinition.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia.Helper
{
    using System.Collections.Generic;
    using CStreamer.PipeLineDefinitions;
    using global::Avalonia;

    internal class DesignerPipelineDefinition
    {
        public DesignerPipelineDefinition(PipeLineDefinition definition, Dictionary<string, Point> positions)
        {
            this.Definition = definition;
            this.Positions = positions;
        }

        public PipeLineDefinition Definition { get; }

        public Dictionary<string, Point> Positions { get; }

        public static implicit operator PipeLineDefinition(DesignerPipelineDefinition d)
        {
            return d.Definition;
        }
    }
}
