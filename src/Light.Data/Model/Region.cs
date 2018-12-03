using System;

namespace Light.Data
{
	/// <summary>
	/// Data region
	/// </summary>
	class Region
	{
		static readonly Region region = new Region (0, 1);

		public static Region OneTimes {
			get {
				return region;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="start">Start index,start from 0</param>
		/// <param name="size">Size</param>
		public Region (int start, int size)
		{
			if (start < 0) {
				throw new ArgumentOutOfRangeException (nameof (start));
			}
			_start = start;
			if (size < 0) {
				throw new ArgumentOutOfRangeException (nameof (size));
			}
			_size = size;
		}

		int _start;

        /// <summary>
        /// Start index,start from 0
        /// </summary>
        public int Start {
			get {
				return _start;
			}
			//set {
			//	if (value < 0) {
			//		throw new ArgumentOutOfRangeException (nameof (value));
			//	}
			//	else {
			//		_start = value;
			//	}
			//}
		}

		int _size = 1;

		/// <summary>
		/// Size
		/// </summary>
		public int Size {
			get {
				return _size;
			}
			//set {
			//	if (value <= 0) {
			//		throw new ArgumentOutOfRangeException (nameof (value));
			//	}
			//	else {
			//		_size = value;
			//	}
			//}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString ()
		{
			return string.Format ("Region:{0} to {1}", Start, Start + Size);
		}
	}
}
