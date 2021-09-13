using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace living_room_api.Data
{
	public class PersonTelevision
	{
		[Key]
		[ForeignKey("Person")]
		public string PersonId { get; set; }
		[ForeignKey("Television")]
		public string TelevisionId { get; set; }
		public virtual Person Person { get; set; }
		public virtual Television Television { get; set; }
	}
}
