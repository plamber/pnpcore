﻿using System;

namespace PnP.Core.Model.Teams
{
    /// <summary>
    /// Represents a reaction to a chatMessage entity.
    /// </summary>
    [ConcreteType(typeof(TeamChatMessageReaction))]
    public interface ITeamChatMessageReaction : IDataModel<ITeamChatMessageReaction>
    {
        /// <summary>
        /// The Timestamp type represents date and time information using ISO 8601 format and is always in UTC time. 
        /// For example, midnight UTC on Jan 1, 2014 would look like this: '2014-01-01T00:00:00Z'
        /// </summary>
        public DateTimeOffset CreatedDateTime { get; set; }

        /// <summary>
        /// Type of reaction
        /// </summary>
        public ChatMessageReactionType ReactionType { get; set; }

        /// <summary>
        /// The user who reacted to the message.
        /// </summary>
        public ITeamIdentitySet User { get; set; }

    }
}
