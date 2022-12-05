namespace App.Service;

public class Error {
    public required string Message { get; set; } = "";
    public ICollection<string> Path { get; set; } = new List<string>();
}
