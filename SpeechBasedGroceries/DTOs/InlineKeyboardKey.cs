using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs
{
	public class InlineKeyboardKey
	{

		public string Text { get; set; }

		public string CallbackData { get; set; }


		public InlineKeyboardKey(string text, string callbackData)
		{
			this.Text = text;
			this.CallbackData = callbackData;
		}

	}
}
