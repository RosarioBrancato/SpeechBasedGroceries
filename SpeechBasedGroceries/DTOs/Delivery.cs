using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs
{
	public class Delivery
	{

		public int Id { get; set; }

		public int CustomerNo { get; set; } // TODO: use Customer object instead

		public DateTime Date { get; set; }

		public string Street { get; set; }

		public string Zip { get; set; }

		public string City { get; set; }

		public string Country { get; set; }

		public string Comment { get; set; }

        public List<Position> Positions { get; set; }

		public string toString()
        {
			return
				Date.ToString("yyyyMMdd")
                + "-"
                + CustomerNo.ToString()
                + Id.ToString("0000"); 
        }

	}
}


