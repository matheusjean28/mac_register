namespace ResponseMacListModel
{
    public class ResponseMacList
    {
        public string Mac { get; set; } = string.Empty;
        public bool MacExists { get; set; }
        public bool CreatedSuccessfully { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}