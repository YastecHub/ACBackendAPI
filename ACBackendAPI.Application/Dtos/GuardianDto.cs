using System.Text.Json.Serialization;

public class GuardianDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("relationship")]
    public string Relationship { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }
}
