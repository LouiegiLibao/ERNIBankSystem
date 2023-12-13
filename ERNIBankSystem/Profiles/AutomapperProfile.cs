using AutoMapper;
using ERNIBankSystem.Data.Models;

namespace ERNIBankSystem.Profiles
{
    public class AutomapperProfile:Profile
    {
        public AutomapperProfile()
        {
            CreateMap<RegisterNewAccountModel, Account>();
            CreateMap<Account, GetAccountModel>();
            CreateMap<Account, AccountCurrentBalance>();
        }
    }
}
