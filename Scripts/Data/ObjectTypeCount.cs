namespace ObjectType
{
    [System.Serializable]
    public class ObjectTypeCount
    {
        public ObjectTypeEnum ObjectType { get; set; }
        public int Count { get; set; }

        public ObjectTypeStringCount GetObjectTypeStringCount()
        {
            return new ObjectTypeStringCount { TypeName = ObjectType.typeName, Count = Count };
        }

        public ObjectTypeIndexCount GetObjectTypeIndexCount()
        {
            return new ObjectTypeIndexCount { Index = ObjectTypeLibrary.Find().GetIndexOfType(ObjectType.typeName), Count = Count };
        }
    }
}