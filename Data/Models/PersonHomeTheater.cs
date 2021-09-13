using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace living_room_api.Data
{
	public class PersonHomeTheater
	{
		[Key]
		[MaxLength(10)]
		[MinLength(10)]
		public string ID { get; set; }

		[ForeignKey("Person")]
		[MaxLength(10)]
		[MinLength(10)]
		public string PersonId { get; set; }

		[ForeignKey("HomeTheater")]
		[MaxLength(10)]
		[MinLength(10)]
		public string HomeTheaterId { get; set; }
	}
}
