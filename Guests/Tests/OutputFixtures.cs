public class RegisterOutput
{
    public string id { get; set; }
    public string token { get; set; }
    public string fail { get; set; }
}

public class CustomBadRequest
{
    public string type { get; set; }
    public string title { get; set; }
    public int status { get; set; }
    public string traceId { get; set; }
    public object errors { get; set; }

}

public class Errors
{
    public string[] DuplicateEmail { get; set; }
    public string[] DuplicateUserName { get; set; }
}


