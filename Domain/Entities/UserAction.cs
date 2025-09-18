using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class UserAction
{
    public int ActionId { get; set; }

    public string UserCode { get; set; } = null!;

    public string ActionDescr { get; set; } = null!;

    public DateTime ActionDate { get; set; }

    public virtual User UserCodeNavigation { get; set; } = null!;
}
