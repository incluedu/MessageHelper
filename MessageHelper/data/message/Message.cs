namespace MessageHelper.data.message
{
    public class Message
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public bool Alarm { get; set; }
        public string AlarmLastOccurence { get; set; }
        public bool TestDone { get; set; }
        public bool TestDoneHideOk { get; set; }
        public string TestDoneTime { get; set; }
        public string Position { get; set; }
        public string MessageText { get; set; }
        public string Comment { get; set; }
        public string MessageNumber { get; set; }
    }
}