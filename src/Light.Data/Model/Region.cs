using System;

namespace Light.Data
{
	/// <summary>
	/// Data region
	/// </summary>
	internal class Region
	{
		public static Region OneTimes { get; } = new Region (0, 1);


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
			Start = start;
			if (size < 0) {
				throw new ArgumentOutOfRangeException (nameof (size));
			}
			Size = size;
		}

		/// <summary>
        /// Start index,start from 0
        /// </summary>
        public int Start
        {
	        get;
	        //set {
	        //	if (value < 0) {
	        //		throw new ArgumentOutOfRangeException (nameof (value));
	        //	}
	        //	else {
	        //		_start = value;
	        //	}
	        //}
        }

		/// <summary>
		/// Size
		/// </summary>
		public int Size
		{
			get;
			//set {
			//	if (value <= 0) {
			//		throw new ArgumentOutOfRangeException (nameof (value));
			//	}
			//	else {
			//		_size = value;
			//	}
			//}
		} = 1;

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
