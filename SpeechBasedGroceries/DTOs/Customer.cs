using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs
{
	public class Customer
	{

		public int Id { get; set; }

		public int ClientNo { get; set; }

		public string Firstname { get; set; }

		public string Surname { get; set; }


        public string toString()
        {
			return Firstname + " " + Surname + " (" + ClientNo + ")"; 
        }

	}
}


