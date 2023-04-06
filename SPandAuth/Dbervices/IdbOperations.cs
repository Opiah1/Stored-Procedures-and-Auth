using SPandAuth.Data;
using SPandAuth.RequestObj;
using SPandAuth.ResponseObj;

namespace SPandAuth.Dbervices
{
    public interface IdbOperations
    {
        CreateAcctData RegisterAccount(UserRegisterRequest request);
        BalanceResponse2 BalanceEnquiry(string accountnumber);
    }
}
