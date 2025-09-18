using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class UserTask
{
    public int TaskId { get; set; }

    public string UserCode { get; set; } = null!;

    public string CategoryCode { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime Deadline { get; set; }

    public string State { get; set; } = null!;

    public virtual Category CategoryCodeNavigation { get; set; } = null!;

    public virtual User UserCodeNavigation { get; set; } = null!;
}
