using System.Text;

namespace Light.Data
{
	/// <summary>
	/// Date time formater.
	/// </summary>
	public class DateTimeFormater
	{
		string yearFormat = "yyyy";

		string monthFormat = "MM";

		string dayFormat = "dd";

		string hourFormat = "HH";

		string minuteFormat = "mm";

		string secondFormat = "ss";

		/// <summary>
		/// Gets or sets the year format.
		/// </summary>
		/// <value>The year format.</value>
		public string YearFormat {
			get {
				return yearFormat;
			}

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
			get {
				return monthFormat;
			}

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
			get {
				return dayFormat;
			}

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
			get {
				return hourFormat;
			}

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
			get {
				return minuteFormat;
			}

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
			get {
				return secondFormat;
			}

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
			StringBuilder sb = new StringBuilder();
			char[] chars = format.ToCharArray();
			int len = chars.Length;
			int i = 0;
			while (i < len) {
				char c = chars[i];
				switch (c) {
					case 'y': {
							int ret = ParseValue(chars, i, 4, 'y');
							if (ret > 0) {
								i += ret;
								sb.Append(yearFormat);
								continue;
							}
						}
						break;
					case 'M': {
							int ret = ParseValue(chars, i, 2, 'M');
							if (ret > 0) {
								i += ret;
								sb.Append(monthFormat);
								continue;
							}
						}
						break;
					case 'd': {
							int ret = ParseValue(chars, i, 2, 'd');
							if (ret > 0) {
								i += ret;
								sb.Append(dayFormat);
								continue;
							}
						}
						break;
					case 'H': {
							int ret = ParseValue(chars, i, 2, 'H');
							if (ret > 0) {
								i += ret;
								sb.Append(hourFormat);
								continue;
							}
						}
						break;
					case 'm': {
							int ret = ParseValue(chars, i, 2, 'm');
							if (ret > 0) {
								i += ret;
								sb.Append(minuteFormat);
								continue;
							}
						}
						break;
					case 's': {
							int ret = ParseValue(chars, i, 2, 's');
							if (ret > 0) {
								i += ret;
								sb.Append(secondFormat);
								continue;
							}
						}
						break;
				}

				sb.Append(c);
				i++;
			}
			return sb.ToString();
		}

		int ParseValue(char[] chars, int i, int max, char c) {
			if (chars.Length < i + max) {
				return 0;
			}
			for (int j = i; j < i + max; j++) {
				if (chars[j] != c) {
					return 0;
				}
			}
			return max;
		}
	}
}

