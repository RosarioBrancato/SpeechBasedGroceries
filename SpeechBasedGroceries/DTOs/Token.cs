using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs
{
	public class Token
	{

		public int Id { get; set; }

        public int CustomerId { get; set; }

		public DateTime Creation { get; set; } // TODO: use Customer object instead

		public string Name { get; set; }

		public string Value { get; set; }

		public DateTime Expiration { get; set; }

	}
}


