﻿using System.Reflection;

namespace Light.Data
{
	internal delegate void SetValueHandler(object source, object value);

	internal delegate object GetValueHandler(object source);

	internal delegate object ObjectInstanceHandler();

	internal delegate object FastMethodHandler(object target, object[] parameters);

	internal class PropertyHandler
	{
		public PropertyHandler(PropertyInfo property) {
			if (property.CanWrite) {
				Set = ReflectionHandlerFactory.PropertySetHandler(property);
			}
			if (property.CanRead) {
				Get = ReflectionHandlerFactory.PropertyGetHandler(property);
			}
			Property = property;
			IndexProperty = Property.GetMethod.GetParameters().Length > 0;
		}

		public GetValueHandler Get { get; }


		public bool IndexProperty { get; }

		public PropertyInfo Property { get; }

		public SetValueHandler Set { get; }
	}
}
