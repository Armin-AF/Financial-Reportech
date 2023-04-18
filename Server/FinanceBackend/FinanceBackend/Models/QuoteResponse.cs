using FinanceBackend.Services;

namespace FinanceBackend.Models;

public class QuoteResponse
{
    public List<QuoteResult> Result { get; set; }
}