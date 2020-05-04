using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs
{
	public class Delivery
	{

		public int Id { get; set; }

		public int CustomerId { get; set; } // TODO: use Customer object instead

		public DateTime Date { get; set; }

		public string Street { get; set; }

		public string Zip { get; set; }

		public string City { get; set; }

		public string Country { get; set; }

		public string Comment { get; set; }

		public List<Position> Positions { get; set; }


		public Delivery()
		{
			this.Positions = new List<Position>();
		}


		public override string ToString()
		{
			return this.Date.ToString("yyyyMMdd") + "-" + this.CustomerId.ToString("000") + this.Id.ToString("000");
		}

	}
}


