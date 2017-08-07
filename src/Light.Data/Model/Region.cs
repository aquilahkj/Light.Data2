using System;

namespace Light.Data
{
	/// <summary>
	/// 取值范围对象
	/// </summary>
	class Region
	{
		static Region region = new Region (0, 1);

		public static Region OneTimes {
			get {
				return region;
			}
		}


		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="start">开始索引,从0开始</param>
		/// <param name="size">取值数量</param>
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
		/// 获取开始索引
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
		/// 获取取值数量
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
