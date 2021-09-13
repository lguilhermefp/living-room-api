using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace living_room_api.Data
{
	public class PersonHomeTheater
	{
		[Key]
		[ForeignKey("Person")]
		public string PersonId { get; set; }

		[ForeignKey("HomeTheater")]
		public string HomeTheaterId { get; set; }

		// public Person Person { get; set; }
		
		// public HomeTheater HomeTheater { get; set; }
	}
}
