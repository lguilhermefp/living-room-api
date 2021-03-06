using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace living_room_api.Data
{
    public class Television
    {
		[Key]
		[MaxLength(10)]
		[MinLength(10)]
        public string ID { get; set; }

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

        [CustomValidation(typeof(Television), "NonNegativeValue")]
        [Column(TypeName = "decimal(16, 2)")]
        public decimal Value { get; set; }

        public static ValidationResult NonNegativeValue(decimal decimalNumber)
        {
            if (decimalNumber < 0)
                return new ValidationResult("product value should not be negative");

            return ValidationResult.Success;
        }

		public Boolean is3D { get; set; }

        public Boolean isBeingSold { get; set; }
    }
}