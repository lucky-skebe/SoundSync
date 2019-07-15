// -----------------------------------------------------------------------
// <copyright file="GraphicalPipeLineDefinition.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace SharPipes.UI.GraphicalDecorators
{
    using System.Collections.Generic;
    using System.Windows;
    using SharPipes.Pipes.Base.PipeLineDefinitions;

    internal class GraphicalPipeLineDefinition
    {
        public GraphicalPipeLineDefinition(PipeLineDefinition definition, Dictionary<string, Point> positions)
        {
            this.Definition = definition;
            this.Positions = positions;
        }

        public PipeLineDefinition Definition { get; }

        public Dictionary<string, Point> Positions { get; }

        public static implicit operator PipeLineDefinition(GraphicalPipeLineDefinition d)
        {
            return d.Definition;
        }
    }
}