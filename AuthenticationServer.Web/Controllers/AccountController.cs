using AuthenticationServer.Common.Enums;
using AuthenticationServer.Common.Exceptions;
using AuthenticationServer.Common.Extentions;
using AuthenticationServer.Common.Interfaces.Services;
using AuthenticationServer.Common.Models.ContractModels;
using AuthenticationServer.Common.Models.ContractModels.Account;
using AuthenticationServer.Common.Models.ContractModels.Token;
using AuthenticationServer.Logic.Factories;
using AuthenticationServer.Web.Middleware.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthenticationServer.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServiceFactory _accountServiceFactory;
        private IAccountService _accountService;

        public AccountController(IAccountServiceFactory accountServiceFactory)
        {
            _accountServiceFactory = accountServiceFactory;
        }



        /// <summary>
        /// Login with your Tenant Account
        /// </summary>
        /// <remarks>
        /// **ApplicationId:** optional parameter: Used to identify the application a Tenant is using. If null the hostname of the request will be used to identify the application
        /// </remarks>
        /// <return>
        /// jwt token
        /// </return>
        /// <param name="credentials"></param>
        [HttpPost("Login")]
        public async Task<Ticket> Login([FromBody] Credentials credentials)
        {
            _accountService = await _accountServiceFactory.CreateAccountService(credentials.Email);

            if (_accountService == null)
                throw new AuthenticationApiException("login", "Invalid Credentials provided");

            string hostname = Request.GetDomainUrl();

            return await _accountService.LoginAsync(credentials, hostname);
        }



        /// <summary>
        /// Register an application to manage your users
        /// </summary>
        /// <remarks>
        /// **AuthenticationRole:** options = ["Admin", "Tenant"]<br/><br/>
        /// **ConfigData:** Custom tenant atrributes and values to store inside the database as json <br/><br/>
        /// **ApplicationId** Optional parameter to specify the application the user registered on <br/><br/>
        /// **AdminId** Only required for Tenant accounts <br/><br/>
        /// </remarks>
        /// <param name="accountData"></param>
        /// <returns>Registration message</returns>
        [HttpPost("Register")]
        [SuccessStatusCode(StatusCodes.Status201Created)]
        public async Task<string> Register([FromBody] AccountRegistration accountData)
        {
            _accountService = _accountServiceFactory.CreateAccountService(Enum.Parse<AccountRole>(accountData.AuthenticationRole));

            return await _accountService.RegisterAsync(accountData);
        }


        /// <summary>
        /// Change an account's password
        /// </summary>
        /// <param name="newCredentials"></param>
        [HttpPost("change-password")]
        public async Task ChangePassword([FromBody] NewCredentials newCredentials)
        {
            _accountService = await _accountServiceFactory.CreateAccountService(newCredentials.Email);

            await _accountService.ChangePassword(newCredentials);
        }

        /// <summary>
        /// Send a password reset email to your email to recover an account
        /// </summary>
        /// <param name="email"></param>
        [HttpPost("reset-password")]
        public async Task ResetPassword([FromBody] string email)
        {
            _accountService = await _accountServiceFactory.CreateAccountService(email);

            await _accountService.ResetPassword(email);
        }

        /// <summary>
        /// Send a password recover email to your email to recover an account
        /// </summary>
        /// <param name="resetPasswordModel"></param>
        [HttpPost]
        [Route("recover-password")]
        public async Task RecoverPassword([FromForm] ResetPasswordModel resetPasswordModel)
        {
            _accountService = await _accountServiceFactory.CreateAccountService(resetPasswordModel.Email);

            await _accountService.RecoverPassword(resetPasswordModel);
        }

        [HttpGet("{code}/VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(Guid code)
        {
            _accountService = await _accountServiceFactory.CreateAccountService(code);

            string email = await _accountService.VerifyEmail(code);
			//TODO configure landing page
            return new ContentResult
            {
                ContentType = "text/html",
                Content = @$" 
                    <!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title> Document </title>
                        <style>
                            html
							{{
								background:black;
	
							}}
							body{{
								max-height: 100vh;
								overflow: hidden;
							}}

							.label-activated{{
							  color: white;
							  text-align: center;
							}}

							#IfOnlyICouldBeSoGrosslyIncandescent
							{{
								width:1200px;
								height:1000px;
								margin:auto;
								position:relative;
								background:black;
								overflow:auto;	
							}}

							#head
							{{
								border-bottom: 130px solid white; 
								border-left: 20px solid transparent; 
								border-right: 20px solid transparent; 
								height: 0; 
								width: 100px;	
								position:relative;
								margin:auto;
								margin-top:15%;
								z-index:13;
							}}

							#head::before
							{{
								content:"""";
								width: 0; 
								height: 0; 
								border-left: 50px solid transparent; 
								border-right: 50px solid transparent; 
								border-bottom: 20px solid white;	
								position:absolute;
								top:-20px;
							}}

							#head::after
							{{
								content:"""";
								background:white;
								width:140px;
								height:30px;
								border-radius:100%;
								position:absolute;
								top:115px;
								left:-20px;
								z-index:13;
							}}

							#neck
							{{
								background:black;
								width:180px;
								height:30px;
								border-radius:100%;
								position:absolute;
								top:125px;
								left:-40px;	
								z-index:12;
							}}

							#feather
							{{
								width:15px;
								height:70px;
								background:white;
								border-top-left-radius:100%;
								border-top-right-radius:100%;
								top:-50px;
								left:-10px;
								position:absolute;
								transform:rotate(-10deg);	
							}}

							#feather::before
							{{
								content:"""";
								width:15px;
								height:70px;
								background:white;
								border-top-left-radius:100%;
								border-top-right-radius:100%;
								top:-40px;
								left:-15px;
								position:absolute;
								transform:rotate(-15deg);	
							}}

							#face
							{{
								width:10px;
								height:100px;
								background:black;
								position:relative;
								margin:auto;
								top:20px;
							}}

							#face::before
							{{
								content:"""";
								width:40px;
								height:20px;
								background:black;
								position:absolute;
								top:20px;
								transform:rotate(5deg);
								left:5px;
							}}

							#face::after
							{{
								content:"""";
								width:40px;
								height:20px;
								background:black;
								position:absolute;
								top:20px;
								transform:rotate(-5deg);
								right:5px;
							}}

							#body 
							{{
								width: 100px; 
								height: 22%; 
								background: white; 
								position: relative; 
								z-index:8;
								margin:auto;
							}} 

							#body:before 
							{{
								content: """"; 
								position: absolute; 
								top: 0; 
								left: -80px; 
								border-bottom: 70px solid white; 
								border-left:60px solid black;
								border-right: 60px solid black; 
								width: 140px; 
								height: 0; 
	
							}} 
 
							 #body:after 
							 {{
								 content: """"; 
								 position: absolute; 
								 bottom: 0; 
								 left: -95px; 
								 border-top: 170px solid white; 
								 border-left: 60px solid black; 
								 border-right: 60px solid black;
								 width: 170px; 
								 height: 0; 
							 }}
 
							 #body2
							 {{
								width: 220px; 
								height:100px;
								background:white;
								position:relative;
								margin:auto;
								top:-100px; 
								z-index:11; 
							 }}
 
							 #rArm1
							 {{
								border-radius:100%;
								height:50px;
								width:300px;
								background:black;
								border:15px solid white;
								position:absolute;
								left:10px;
								top:-10px;
								z-index:7;	
								transform:rotate(-50deg); 
							 }}
 
							 #rArm1::before
							 {{
								content:"""";
								height:60px;
								width:130px;
								background:white;
								position:absolute;
								left:0px;
								top:-8px;
								z-index:7;	
							 }}
 
							 #rArm2
							 {{
								border-radius:100%;
								height:40px;
								width:200px;
								background:black;
								border:15px solid white;
								position:absolute;
								left:70%;
								top:-10px;
								z-index:7;
								transform:rotate(-2deg);	
							 }}
 
							 #rArm2::before
							 {{
								content:"""";
								height:42px;
								width:100px;
								background:black;
								border-radius:100%;
								position:absolute;
								left:-30px;
								top:-3px;
								z-index:7;	
							 }}
 
							 #rArm2::after
							 {{
								content:"""";
								height:70px;
								width:100px;
								background:white;
								position:absolute;
								border-bottom-left-radius:20%;
								border-top-left-radius:20%;
								border-bottom-right-radius:80%;
								border-top-right-radius:20%;
								left:60%;
								top:-15px;
								z-index:7;	
							 }}
 
							 #rHand
							 {{
								border-radius:100%;
								height:40px;
								width:120px;
								background:white;
								left:180px;
								top:-10px;
								position:absolute;	 
								transform:rotate(-5deg);
							 }}
 
							 #rHand::after
							 {{
								content:"""";
								border-radius:100%;
								height:20px;
								width:70px;
								top:25px;
								background:white;
								border-right:3px solid #000;
								position:absolute;	 
							 }}
 
 
							 #lArm1
							 {{
								border-radius:100%;
								height:50px;
								width:300px;
								background:black;
								border:15px solid white;
								position:absolute;
								right:10px;
								top:-10px;
								z-index:7;	
								transform:rotate(50deg); 
							 }}
 
							 #lArm1::before
							 {{
								content:"""";
								height:60px;
								width:130px;
								background:white;
								position:absolute;
								left:170px;
								top:-8px;
								z-index:7;	
							 }}
 
							 #lArm2
							 {{
								border-radius:100%;
								height:40px;
								width:200px;
								background:black;
								border:15px solid white;
								position:absolute;
								right:70%;
								top:-10px;
								z-index:7;
								transform:rotate(2deg);	
							 }}
 
							 #lArm2::before
							 {{
								content:"""";
								height:45px;
								width:100px;
								background:black;
								border-radius:100%;
								position:absolute;
								right:-50px;
								top:-5px;
								z-index:7;	
							 }}
 
							 #lArm2::after
							 {{
								content:"""";
								height:70px;
								width:100px;
								background:white;
								position:absolute;
								border-bottom-left-radius:80%;
								border-top-left-radius:20%;
								border-bottom-right-radius:20%;
								border-top-right-radius:20%;
								right:60%;
								top:-15px;
								z-index:7;	
							 }}
 
							 #lHand
							 {{
								border-radius:100%;
								height:40px;
								width:120px;
								background:white;
								right:180px;
								top:-10px;
								position:absolute;	 
								transform:rotate(5deg);
							 }}
 
							 #lHand::after
							 {{
								content:"""";
								border-radius:100%;
								height:20px;
								width:70px;
								top:25px;
								left:45px;
								background:white;
								border-left:3px solid #000;
								position:absolute;	 
							 }}
 
							 #sun 
							 {{
								 background: black; 
								 width: 150px; 
								 height: 20px; 
								 margin:auto;
								 bottom:35px;
								 position: relative; 
								 text-align: center;
								 border-radius:100%;
								 transform:rotate(90deg);
	
							 }} 
							 #sun::before 
							 {{
								  content: """"; 
								  position: absolute; 
								  top: 0; 
								  left: 0; 
								  height: 20px; 
								  width: 150px; 
								  background: black; 
								  border-radius:100%;
								  transform:rotate(90deg);
							  }} 
  
							 #sun::after 
							 {{
								  content: """"; 
								  position: absolute; 
								  top: 0; 
								  left: 0; 
								  height: 20px; 
								  width: 150px; 
								  background: black; 
								  border-radius:100%;
								  transform:rotate(45deg);
							  }} 
 
							 #sunface
							 {{
								position: absolute; 
								top: 0; 
								left: 0; 
								height: 20px; 
								width: 150px; 
								background: black; 
								border-radius:100%;
								transform:rotate(-45deg);
								z-index:9;
							 }}
 
							 #sunface::before
							 {{
								 content:"""";
								height:100px;
								width:100px;
								background:white;
								border-radius:100%;	
								position:absolute;
								top:-40px;
								left:25px; 
								z-index:15;
							 }}
 
							#praise
							{{
								color:#fff;
								text-align:center;
								font-family:Baskerville, ""Palatino Linotype"", Palatino, ""Century Schoolbook L"", ""Times New Roman"", serif;
								font-size:100px;
								font-weight:bolder;
							}}
                        </style>
                    </head>
                    <body>
                        <div class=""label-activated"">
                      <h2>{email} is now active</h2>
                    </div>
                    <div id=""IfOnlyICouldBeSoGrosslyIncandescent"">
    	                    <div id=""head"">
        	                    <div id=""feather"">
                                </div>
        	                    <div id=""face"">
                                </div> 
                                 <div id=""neck"">
        	                    </div> 
                            </div>
       
                            <div id=""body"">
        	                    <div id=""rArm1"">
            	                    <div id=""rArm2"">
                	                    <div id=""rHand"">
                                        </div>
                                    </div>
                                </div>
                                <div id=""lArm1"">
            	                    <div id=""lArm2"">
                	                    <div id=""lHand"">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id=""body2"">
        	                     <div id=""sun"">
             	                    <div id=""sunface"">
                                    </div>
        	                    </div>
                            </div>
        
                            <div id=""praise"">
        	                    PRAISE THE SUN
                            </div>
       
                        </div>

                    </body>
                    </html>
                "
            };
        }
    }
}
