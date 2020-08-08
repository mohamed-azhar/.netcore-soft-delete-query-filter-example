using SoftDeleteQueryFilterExample.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftDeleteQueryFilterExample.Models
{
    public abstract class CommonAttributes : IDeletableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid Uid { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
