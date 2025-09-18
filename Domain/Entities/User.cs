using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string UserCode { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public virtual ICollection<UserAction> UserActions { get; set; } = new List<UserAction>();

    public virtual ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
}
