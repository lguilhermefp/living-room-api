using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace living_room_api.Data
{
    public class Computer
    {
		[Key]
		[MaxLength(10)]
		[MinLength(10)]
        public string ID { get; set; }

		// [MaxLength(10)]
		// [MinLength(10)]
		// [ForeignKey("Person")]
		// public string PersonID { get; set; }

        [Required]
		[MaxLength(60)]
        public string Brand { get; set; }

        [Required]
		[MaxLength(100)]
        public string Model { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation date")]
        public DateTime? CreationDate { get; set; }

		[Required]
        public Boolean isActive { get; set; }

        // public virtual ICollection<Person> People { get; set; }
    }
}