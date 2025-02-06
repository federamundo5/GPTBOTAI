namespace GPTLearning.Services.Interfaces
{
    public interface IGPTService
    {
          Task<string> GetResponseFromOpenAI(string prompt);

    }
}
