using System;
using System.Data;
using System.Reflection;

namespace Light.Data
{
	/// <summary>
	/// Data mapping.
	/// </summary>
	internal abstract class DataMapping : IDataDefine
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataMapping"/> class.
		/// </summary>
		/// <param name="type">Type.</param>
		protected DataMapping (Type type)
		{
			ObjectType = type;
            ObjectTypeInfo = type.GetTypeInfo();
		}

		/// <summary>
		/// Gets or sets the type of the object.
		/// </summary>
		/// <value>The type of the object.</value>
		public Type ObjectType
		{
			get;
		}

		public TypeInfo ObjectTypeInfo { get; }

		/// <summary>
		/// Gets or sets the extent parameters.
		/// </summary>
		/// <value>The extent parameters.</value>
		public ConfigParamSet ExtentParams { get; protected set; }

		#region IFieldCollection 成员

		public abstract object LoadData (DataContext context, IDataReader dataReader, object state);

		public abstract object InitialData ();

		#endregion

		public override string ToString ()
		{
			return string.Format ("[DataMapping: ObjectType={0}, ExtentParams={1}]", ObjectType, ExtentParams);
		}
	}
}
