namespace Light.Data
{
	internal class RelationItem
	{
		public string CurrentFieldPath { get; set; }

		public string [] Keys { get; set; }

		public DataEntityMapping DataMapping { get; set; }

		public SingleRelationFieldMapping FieldMapping { get; set; }

		public string AliasName { get; set; }

		public string PrevFieldPath { get; set; }
	}
}

