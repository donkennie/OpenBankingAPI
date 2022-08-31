using AutoMapper;
using OpenBankingAPI.Models;

namespace OpenBankingAPI.Profiles
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<RegisterNewAccountModel, Account>();

            CreateMap<UpdateAccountModel, Account>();

            CreateMap<Account, GetAccountModel>();

           // CreateMap<TransactionRequestDto, Transaction>();
        }
    }
}
