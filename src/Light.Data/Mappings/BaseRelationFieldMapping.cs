using System;

namespace Light.Data
{
	/// <summary>
	/// Base relation field mapping.
	/// </summary>
	internal abstract class BaseRelationFieldMapping
	{
		readonly protected PropertyHandler handler;

		readonly protected RelationKey [] keyPairs;

		readonly protected Type relateType;

		readonly protected string fieldName;

		readonly protected DataEntityMapping masterEntityMapping;

		readonly protected DataFieldMapping [] masterFieldMappings;

		readonly protected DataFieldInfo [] masterInfos;

		public PropertyHandler Handler => handler;

		public DataEntityMapping MasterMapping => masterEntityMapping;

		public string FieldName => fieldName;

		private readonly object locker = new object ();

		protected DataEntityMapping relateEntityMapping;

		public DataEntityMapping RelateMapping => relateEntityMapping;

		protected DataFieldMapping [] relateFieldMappings;

		protected DataFieldInfo [] relateInfos;

		public DataFieldInfoRelation [] CreateDataFieldInfoRelations (string masterAlias, string relateAlias)
		{
			InitialRelateMapping ();
			var array = new DataFieldInfoRelation [keyPairs.Length];
			for (var i = 0; i < keyPairs.Length; i++) {
				var masterField = new DataFieldInfo (masterFieldMappings [i], masterAlias);
				var relateField = new DataFieldInfo (relateFieldMappings [i], relateAlias);
				array [i] = new DataFieldInfoRelation (masterField, relateField);
			}
			return array;
		}

		public RelationKey [] GetKeyPairs ()
		{
			return keyPairs.Clone () as RelationKey [];
		}


		protected BaseRelationFieldMapping (string fieldName, DataEntityMapping mapping, Type relateType, RelationKey [] keyPairs, PropertyHandler handler)
		{
			if (fieldName == null)
				throw new ArgumentNullException (nameof (fieldName));
			if (mapping == null)
				throw new ArgumentNullException (nameof (mapping));
			if (relateType == null)
				throw new ArgumentNullException (nameof (relateType));
			if (keyPairs == null || keyPairs.Length == 0)
				throw new ArgumentNullException (nameof (keyPairs));
			if (handler == null)
				throw new ArgumentNullException (nameof (handler));
			this.fieldName = fieldName;
			masterEntityMapping = mapping;
			this.relateType = relateType;
			this.keyPairs = keyPairs;
			this.handler = handler;
			masterFieldMappings = new DataFieldMapping [keyPairs.Length];
			masterInfos = new DataFieldInfo [keyPairs.Length];
			for (var i = 0; i < keyPairs.Length; i++) {
				var field = mapping.FindDataEntityField (keyPairs [i].MasterKey);
				if (field == null) {
                    throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedField, mapping.ObjectType, keyPairs[i].MasterKey));
				}
				masterFieldMappings [i] = field;
				masterInfos [i] = new DataFieldInfo (field);
			}
		}

		protected void InitialRelateMapping ()
		{
			if (relateEntityMapping == null) {
				lock (locker) {
					if (relateEntityMapping == null) {
						InitialRelateMappingInc ();
					}
				}
			}
		}

		protected virtual void InitialRelateMappingInc ()
		{
			var mapping = DataEntityMapping.GetEntityMapping (relateType);
			var infos = new DataFieldInfo [keyPairs.Length];
			var fields = new DataFieldMapping [keyPairs.Length];
			for (var i = 0; i < keyPairs.Length; i++) {
				var field = mapping.FindDataEntityField (keyPairs [i].RelateKey);
				if (field == null) {
					throw new LightDataException(string.Format(SR.CanNotFindTheSpecifiedField, mapping.ObjectType, keyPairs[i].RelateKey));
				}
				fields [i] = field;
				infos [i] = new DataFieldInfo (field);
			}
			relateInfos = infos;
			relateFieldMappings = fields;
			relateEntityMapping = mapping;
		}

		public bool IsReverseMatch (BaseRelationFieldMapping mapping)
		{
			InitialRelateMapping ();
			mapping.InitialRelateMapping ();
			if (masterEntityMapping.TableName != mapping.relateEntityMapping.TableName) {
				return false;
			}
			if (relateEntityMapping.TableName != mapping.masterEntityMapping.TableName) {
				return false;
			}
			if (keyPairs.Length != mapping.keyPairs.Length) {
				return false;
			}
			for (var i = 0; i < keyPairs.Length; i++) {
				var ismatch = false;
				var master = keyPairs [i];
				for (var j = 0; j < mapping.keyPairs.Length; j++) {
					var relate = mapping.keyPairs [j];
					if (master.IsReverseMatch (relate)) {
						ismatch = true;
						break;
					}
				}
				if (!ismatch) {
					return false;
				}
			}
			return true;
		}

		public bool IsMatch (BaseRelationFieldMapping mapping)
		{
			InitialRelateMapping ();
			mapping.InitialRelateMapping ();
			if (masterEntityMapping.TableName != mapping.masterEntityMapping.TableName) {
				return false;
			}
			if (relateEntityMapping.TableName != mapping.relateEntityMapping.TableName) {
				return false;
			}
			if (keyPairs.Length != mapping.keyPairs.Length) {
				return false;
			}
			for (var i = 0; i < keyPairs.Length; i++) {
				var ismatch = false;
				var master = keyPairs [i];
				for (var j = 0; j < mapping.keyPairs.Length; j++) {
					var relate = mapping.keyPairs [j];
					if (master.IsMatch (relate)) {
						ismatch = true;
						break;
					}
				}
				if (!ismatch) {
					return false;
				}
			}
			return true;
		}
	}
}

