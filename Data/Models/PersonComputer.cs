using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace living_room_api.Data
{
	public class PersonComputer
	{
		[Key]
		[MaxLength(10)]
		[MinLength(10)]
		public string ID { get; set; }

		[ForeignKey("Person")]
		[MaxLength(10)]
		[MinLength(10)]
		public string PersonId { get; set; }
		
		[ForeignKey("Computer")]
		[MaxLength(10)]
		[MinLength(10)]
		public string ComputerId { get; set; }
		// public Person Person { get; set; }
		// public Computer Computer { get; set; }
	}
}
