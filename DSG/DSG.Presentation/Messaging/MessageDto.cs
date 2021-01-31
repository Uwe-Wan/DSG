namespace DSG.Presentation.Messaging
{
    public class MessageDto
    {
        public string Name { get; set; }

        public object Data { get; set; }

        public MessageDto(string name)
        {
            Name = Name;
        }

        public MessageDto(string name, object data)
        {
            Name = name;
            Data = data;
        }
    }
}
