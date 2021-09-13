using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace living_room_api.Data
{
	public class PersonComputer
	{
		[Key]
		[ForeignKey("Person")]
		public string PersonId { get; set; }
		[ForeignKey("Computer")]
		public string ComputerId { get; set; }
		// public Person Person { get; set; }
		// public Computer Computer { get; set; }
	}
}
