using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Roulette.DataAccess.Models
{
    [Table("Colors", Schema = "Roulette")]

    public class Colors: BaseUpdatableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
