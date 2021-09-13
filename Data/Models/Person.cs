using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace living_room_api.Data
{
    public class Person
    {
		[Key]
		[MinLength(10)]
        [MaxLength(10)]
        public string ID { get; set; }
		
        [Required]
        [MaxLength(60)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
		[MaxLength(60)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

		[Required]
		[MaxLength(60)]
        [Display(Name = "Country Birth Location")]
		public string CountryBirthLocation { get; set; }

		[Required]
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

		[Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}