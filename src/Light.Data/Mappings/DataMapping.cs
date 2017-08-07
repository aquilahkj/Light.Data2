using System;
using System.Data;
using System.Reflection;

namespace Light.Data
{
	/// <summary>
	/// Data mapping.
	/// </summary>
	abstract class DataMapping : IDataDefine
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataMapping"/> class.
		/// </summary>
		/// <param name="type">Type.</param>
		protected DataMapping (Type type)
		{
			this.objectType = type;
            this.objectTypeInfo = type.GetTypeInfo();
		}

		Type objectType;

        TypeInfo objectTypeInfo;

		/// <summary>
		/// Gets or sets the type of the object.
		/// </summary>
		/// <value>The type of the object.</value>
		public Type ObjectType {
			get {
				return objectType;
			}
			//protected set {
			//	objectType = value;
			//}
		}

        public TypeInfo ObjectTypeInfo {
            get {
                return objectTypeInfo;
            }
        }

		ConfigParamSet extentParams;

		/// <summary>
		/// Gets or sets the extent parameters.
		/// </summary>
		/// <value>The extent parameters.</value>
		public ConfigParamSet ExtentParams {
			get {
				return extentParams;
			}
			protected set {
				extentParams = value;
			}
		}

		#region IFieldCollection 成员

		public abstract object LoadData (DataContext context, IDataReader datareader, object state);

		public abstract object InitialData ();

		#endregion

		public override string ToString ()
		{
			return string.Format ("[DataMapping: ObjectType={0}, ExtentParams={1}]", ObjectType, ExtentParams);
		}
	}
}
