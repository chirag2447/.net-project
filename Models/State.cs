using System;
using System.Collections.Generic;

namespace communityWeb.Models;

public partial class State
{
    public int Id { get; set; }

    public string? Name { get; set; } = null!;

    public int? CountryId { get; set; }

    public virtual ICollection<City> Cities { get; } = new List<City>();

    public virtual Country? Country { get; set; }
}
