using System.Text;

namespace Light.Data
{
	/// <summary>
	/// Date time formater.
	/// </summary>
	public class DateTimeFormater
	{
		private string yearFormat = "yyyy";

		private string monthFormat = "MM";

		private string dayFormat = "dd";

		private string hourFormat = "HH";

		private string minuteFormat = "mm";

		private string secondFormat = "ss";

		/// <summary>
		/// Gets or sets the year format.
		/// </summary>
		/// <value>The year format.</value>
		public string YearFormat {
			get => yearFormat;

			set {
				if (!string.IsNullOrEmpty(value))
					yearFormat = value;
			}
		}

		/// <summary>
		/// Gets or sets the month format.
		/// </summary>
		/// <value>The month format.</value>
		public string MonthFormat {
			get => monthFormat;

			set {
				if (!string.IsNullOrEmpty(value))
					monthFormat = value;
			}
		}

		/// <summary>
		/// Gets or sets the day format.
		/// </summary>
		/// <value>The day format.</value>
		public string DayFormat {
			get => dayFormat;

			set {
				if (!string.IsNullOrEmpty(value))
					dayFormat = value;
			}
		}

		/// <summary>
		/// Gets or sets the hour format.
		/// </summary>
		/// <value>The hour format.</value>
		public string HourFormat {
			get => hourFormat;

			set {
				if (!string.IsNullOrEmpty(value))
					hourFormat = value;
			}
		}

		/// <summary>
		/// Gets or sets the minute format.
		/// </summary>
		/// <value>The minute format.</value>
		public string MinuteFormat {
			get => minuteFormat;

			set {
				if (!string.IsNullOrEmpty(value))
					minuteFormat = value;
			}
		}

		/// <summary>
		/// Gets or sets the second format.
		/// </summary>
		/// <value>The second format.</value>
		public string SecondFormat {
			get => secondFormat;

			set {
				if (!string.IsNullOrEmpty(value))
					secondFormat = value;
			}
		}

		/// <summary>
		/// Formats the data.
		/// </summary>
		/// <returns>The data.</returns>
		/// <param name="format">Format.</param>
		public string FormatData(string format) {
			var sb = new StringBuilder();
			var chars = format.ToCharArray();
			var len = chars.Length;
			var i = 0;
			while (i < len) {
				var c = chars[i];
				switch (c) {
					case 'y': {
							var ret = ParseValue(chars, i, 4, 'y');
							if (ret > 0) {
								i += ret;
								sb.Append(yearFormat);
								continue;
							}
							break;
						}
					case 'M': {
							var ret = ParseValue(chars, i, 2, 'M');
							if (ret > 0) {
								i += ret;
								sb.Append(monthFormat);
								continue;
							}
							break;
						}
					case 'd': {
							var ret = ParseValue(chars, i, 2, 'd');
							if (ret > 0) {
								i += ret;
								sb.Append(dayFormat);
								continue;
							}
							break;
						}
					case 'H': {
							var ret = ParseValue(chars, i, 2, 'H');
							if (ret > 0) {
								i += ret;
								sb.Append(hourFormat);
								continue;
							}
							break;
						}
					case 'm': {
							var ret = ParseValue(chars, i, 2, 'm');
							if (ret > 0) {
								i += ret;
								sb.Append(minuteFormat);
								continue;
							}
							break;
						}
					case 's': {
							var ret = ParseValue(chars, i, 2, 's');
							if (ret > 0) {
								i += ret;
								sb.Append(secondFormat);
								continue;
							}
							break;
						}
				}

				sb.Append(c);
				i++;
			}
			return sb.ToString();
		}

		private int ParseValue(char[] chars, int i, int max, char c) {
			if (chars.Length < i + max) {
				return 0;
			}
			for (var j = i; j < i + max; j++) {
				if (chars[j] != c) {
					return 0;
				}
			}
			return max;
		}
	}
}

