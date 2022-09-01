using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenBankingAPI.Models;
using OpenBankingAPI.Services.Interfaces;
using System.Text.RegularExpressions;

namespace OpenBankingAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountsController : ControllerBase
    {
        private IMapper _mapper;
        private IAccountService _userService;

        public AccountsController(IMapper mapper, IAccountService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
        [HttpPost]
        [Route("register_new_account")]
        public IActionResult RegisterNewAccount([FromBody] RegisterNewAccountModel newAccount)
        {
            if (!ModelState.IsValid) return BadRequest(newAccount);
            //map
            var account = _mapper.Map<Account>(newAccount);
            return Ok(_userService.Create(account, newAccount.Pin, newAccount.ConfirmPin));
        }


        [HttpGet]
        [Route("get_account_by_id")]
        public IActionResult GetAccountById(int Id)
        {
            var account = _userService.GetById(Id);
            var getAccountModel = _mapper.Map<GetAccountModel>(account);
            return Ok(getAccountModel);
        }


        [HttpGet]
        [Route("get_all_accounts")]
        public IActionResult GetAllAccounts()
        {
            var allAccounts = _userService.GetAllAccounts();
            var getCleanedAccounts = _mapper.Map<IList<GetAccountModel>>(allAccounts);
            return Ok(getCleanedAccounts);
        }


        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            var authResult = _userService.Authenticate(model.AccountNumber, model.Pin);
            if (authResult == null) return Unauthorized("Invalid Credentials");
            return Ok(authResult);
        }


        [HttpGet]
        [Route("get_by_account_number")]
        public IActionResult GetByAccountNumber(string AccountNumber)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]/d{9}$|^[1-9]\d{9}$"))
            {
                return BadRequest("Account must be 10 digits");
            }

            var account = _userService.GetByAccountNumber(AccountNumber);

            var cleanedaccount = _mapper.Map<GetAccountModel>(account);

            return Ok(cleanedaccount);
        }


        [HttpPost]
        [Route("update_account")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountModel updateAccount)
        {
            if (!ModelState.IsValid) return BadRequest(updateAccount);
            var account = _mapper.Map<Account>(updateAccount);

            _userService.Update(account, updateAccount.Pin);

            return Ok();
        }

    }
}
