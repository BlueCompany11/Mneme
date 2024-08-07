﻿using System;

namespace Mneme.Model;

public abstract class Source
{
    public int Id { get; set; }
    /// <summary>
    /// Used to recognize if 2 sources are the same
    /// </summary>
    public virtual string IntegrationId { get; protected set; }
    public required string Title { get; set; }
    public DateTime CreationTime { get; init; }
    public required bool Active { get; set; }
    public abstract string GetDetails();
    public abstract string TextType { get; }

    public bool IsSame(Source other)
    {
        return IntegrationId == other.IntegrationId;
    }
}
