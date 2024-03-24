namespace RewardEat.Adapter.Http.Contexts;

public interface IHttpHeaderContext
{
    IDictionary<string, string> Headers { get; }
    string GetValue(string key);
}
