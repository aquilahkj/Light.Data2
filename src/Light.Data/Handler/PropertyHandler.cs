using System.Reflection;

namespace Light.Data
{
	delegate void SetValueHandler(object source, object value);
	delegate object GetValueHandler(object source);
	delegate object ObjectInstanceHandler();
	delegate object FastMethodHandler(object target, object[] paramters);

	class PropertyHandler
	{
		private GetValueHandler mGetValue;
		private PropertyInfo mProperty;
		private SetValueHandler mSetValue;
		private bool mIndexProperty;
		public PropertyHandler(PropertyInfo property) {
			if (property.CanWrite) {
				this.mSetValue = ReflectionHandlerFactory.PropertySetHandler(property);
			}
			if (property.CanRead) {
				this.mGetValue = ReflectionHandlerFactory.PropertyGetHandler(property);
			}
			this.mProperty = property;
			this.IndexProperty = this.mProperty.GetMethod.GetParameters().Length > 0;
		}

		public GetValueHandler Get {
			get {
				return this.mGetValue;
			}
		}


		public bool IndexProperty {
			get {
				return this.mIndexProperty;
			}
			set {
				this.mIndexProperty = value;
			}
		}

		public PropertyInfo Property {
			get {
				return this.mProperty;
			}
			set {
				this.mProperty = value;
			}
		}

		public SetValueHandler Set {
			get {
				return this.mSetValue;
			}
		}
	}
}
