using Microsoft.EntityFrameworkCore;

namespace SPandAuth.ResponseObj
{
    [Keyless]
    public class BalanceResponse
    {
        public string FullName { get; set; } = string.Empty;    
        public int? UsableBalance { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
    }
}
