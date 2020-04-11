using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs
{
	public class Customer
	{

		public int Id { get; set; }

		public int No { get; set; }

		public string Firstname { get; set; }

		public string Surname { get; set; }

        public DateTime Birthdate { get; set; }

		public string Street { get; set; }

		public string Zip { get; set; }

		public string City { get; set; }

		public string Country { get; set; }

		public string Email { get; set; }

        // TODO: List<Token>

		public string toString()
        {
			return
                No.ToString() + ": "
                + Firstname + " "
                + Surname
                + " (" + Email + ")"; 
        }

	}
}


