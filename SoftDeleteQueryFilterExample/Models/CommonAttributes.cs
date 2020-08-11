using SoftDeleteQueryFilterExample.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftDeleteQueryFilterExample.Models
{
    public abstract class CommonAttributes : IDeletableEntity
    {
        public CommonAttributes()
        {
            CreatedDate = DateTime.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
