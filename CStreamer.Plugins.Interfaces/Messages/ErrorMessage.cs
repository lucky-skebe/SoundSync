﻿// -----------------------------------------------------------------------
// <copyright file="ErrorMessage.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Plugins.Interfaces.Messages
{
    public class ErrorMessage : Message
    {
        public ErrorMessage(string errorText)
        {
            this.ErrorText = errorText;
        }

        public string ErrorText { get; }
    }
}
