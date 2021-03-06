﻿using DeanAndSons.Models;
using DeanAndSons.Models.Global.ViewModels;
using DeanAndSons.Models.IMS.ViewModels;
using DeanAndSons.Models.JSONClasses;
using DeanAndSons.Models.WAP;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DeanAndSons.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db = new ApplicationDbContext();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // *********** Added customer fields to applicationuser ***********
                var user = new Customer
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Forename = model.Forename,
                    Surname = model.Surname,
                    UserNameDisp = model.Email,
                    Deleted = false
                };
                // *********** END Added customer fields to applicationuser ***********

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Adds this user to the customer role on account creation
                    await UserManager.AddToRoleAsync(user.Id, "Customer");
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    // Find newly created user ID and pass to Profile Details method
                    return RedirectToAction("ProfileDetails", "Account", new { userID = user.Id });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult StaffManagerCreate()
        {
            var vm = new RegisterStaffViewModel();
            var subordinateList = new MultiSelectList(db.Users.OfType<Staff>(), "Id", "Forename");
            vm.Subordinates = subordinateList;
            ViewBag.SuperiorID = new SelectList(db.Users.OfType<Staff>(), "Id", "Forename");

            return View(vm);
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous] //TODO Remove anonymous
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> StaffManagerCreate([Bind(Exclude = "Subordinates")] RegisterStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var _subordinates = new Collection<Staff>();

                // Add all users seleced in listBox to newly created Staff member's subordinate list
                // This changes the subordinate's superior ID via EF framework magic
                foreach (var item in model.SubordinateIds)
                {
                    var _tmpUsr = (Staff)UserManager.FindById(item);
                    _subordinates.Add(_tmpUsr);
                }

                var user = new Staff
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Forename = model.Forename,
                    Surname = model.Surname,
                    Rank = model.Rank,
                    SuperiorID = model.SuperiorID,
                    Subordinates = _subordinates,
                    UserNameDisp = model.Email,
                    Deleted = false
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Adds this user to the Staff role on account creation
                    await UserManager.AddToRoleAsync(user.Id, "Staff");
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    // Find newly created user ID and pass to Profile Details method
                    return RedirectToAction("StaffManagerIndex", "IMS", null);
                }
                AddErrors(result);
            }

            var subordinateList = new MultiSelectList(db.Users.OfType<Staff>(), "Id", "Forename", model.SubordinateIds);
            model.Subordinates = subordinateList;
            ViewBag.SuperiorID = new SelectList(db.Users.OfType<Staff>(), "Id", "Forename", model.SuperiorID);
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        // ********** CUSTOM ACCOUNT CONTROLLER METHODS **********

        [Authorize(Roles = "Admin, Staff, Customer")]
        public ActionResult ProfileDetails(string userID)
        {
            if (String.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Include(i => i.Image).Include(c => c.Contact)
                    .Single(s => s.Id == userID);

            if (user is Customer)
            {
                var _tmp = db.Users.OfType<Customer>().Include(i => i.Image)
                    .Include(c => c.Contact)
                    .Include(e => e.SavedSearches)
                    .Include(p=>p.SavedPropertys)
                    .Single(s => s.Id == userID);
                var vm = new ProfileCustDetailsViewModel(_tmp);
                vm.CurrentUserID = User.Identity.GetUserId();
                return View("ProfileCustDetails", vm);
            }
            else
            {
                var _tmp = db.Users.OfType<Staff>().Include(i => i.Image)
                    .Include(c => c.Contact)
                    .Include(e => e.EventsOwned)
                    .Include(p => p.PropertysOwned)
                    .Include(s => s.ServicesOwned)
                    .Single(s => s.Id == userID);
                var vm = new ProfileStaffDetailsViewModel(_tmp);
                vm.CurrentUserID = User.Identity.GetUserId();
                return View("ProfileStaffDetails", vm);
            }
        }

        public ActionResult ProfileEdit(string userID)
        {
            if (String.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Include(c => c.Contact)
                .Where(u => u.Id == userID).Single();

            if (user == null)
            {
                return HttpNotFound();
            }

            if (user is Customer)
            {
                var vm = new ProfileCustEditViewModel((Customer)user);
                ViewBag.SiteTheme = new SelectList(populateSiteTheme(), "Value", "Text", user.SiteTheme);
                return View("ProfileCustEdit", vm);
            }
            else
            {
                var vm = new ProfileStaffEditViewModel((Staff)user);
                ViewBag.SiteTheme = new SelectList(populateSiteTheme(), "Value", "Text", user.SiteTheme);
                return View("ProfileStaffEdit", vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult ProfileCustEdit(ProfileCustEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var _cust = db.Users.OfType<Customer>()
                    .Include(i => i.Image)
                    .Include(c => c.Contact)
                    .Where(s => s.Id == vm.ID).Single();


                _cust.Forename = vm.Forename;
                _cust.Surname = vm.Surname;
                _cust.About = vm.About;
                _cust.UserNameDisp = vm.UserNameDisp;
                _cust.BudgetLower = vm.BudgetLower;
                _cust.BudgetHigher = vm.BudgetHigher;
                _cust.PrefPropertyType = vm.PrefPropertyType;
                _cust.PrefPropertyStyle = vm.PrefPropertyStyle;
                _cust.PrefPropertyAge = vm.PrefPropertyAge;
                _cust.PrefNoBedRms = vm.PrefNoBedRms;
                _cust.PrefNoBathRms = vm.PrefNoBathRms;
                _cust.PrefNoSittingRms = vm.PrefNoSittingRms;

                _cust.addContact(vm.PropertyNo, vm.Street, vm.Town, vm.PostCode, vm.TelephoneNo, null, _cust);
                _cust.addImage(vm.Image);
                _cust.SiteTheme = vm.SiteTheme;

                db.Entry(_cust).State = EntityState.Modified;
                db.SaveChanges();
                
                return RedirectToAction("ProfileDetails", new { userID = _cust.Id });
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult ProfileStaffEdit(ProfileStaffEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var _staff = db.Users.OfType<Staff>().Include(i => i.Image).Include(c => c.Contact)
                    .Where(s => s.Id == vm.ID).Single();

                _staff.Forename = vm.Forename;
                _staff.Surname = vm.Surname;
                _staff.About = vm.About;
                _staff.UserNameDisp = vm.UserNameDisp;

                _staff.addContact(vm.PropertyNo, vm.Street, vm.Town, vm.PostCode, vm.TelephoneNo, null, _staff);
                _staff.addImage(vm.Image);
                _staff.SiteTheme = vm.SiteTheme;

                db.Entry(_staff).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ProfileDetails", new { userID = _staff.Id });
            }

            return View(vm);
        }

        public string SaveSearch(string obj)
        {
            JavaScriptSerializer s = new JavaScriptSerializer();
            var _temp = s.Deserialize<SaveSearchJSON>(obj);

            var savedSearch = new SavedSearch(_temp, User.Identity.GetUserId());
            db.SavedSearches.Add(savedSearch);
            db.SaveChanges();

            return "{\"response\":\"Search parameters have been saved to your favourites\"}";
        }

        public string SaveProperty(string obj)
        {
            JavaScriptSerializer s = new JavaScriptSerializer();
            var _temp = s.Deserialize<SavePropertyJSON>(obj);
            var _userID = User.Identity.GetUserId();

            var user = db.Users.OfType<Customer>()
                .Include(sp => sp.SavedPropertys)
                .Single(u => u.Id == _userID);

            var property = db.Propertys.Find(_temp.PropertyID);

            user.SavedPropertys.Add(property);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return "{\"response\":\"Property has been saved to your favourites\"}";
        }

        [HttpPost, ActionName("DeletePropFav")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePropFav(int id)
        {
            var _userID = User.Identity.GetUserId();

            var user = db.Users.OfType<Customer>()
                .Include(sp => sp.SavedPropertys)
                .Single(u => u.Id == _userID);

            var property = db.Propertys.Find(id);

            user.SavedPropertys.Remove(property);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ProfileDetails", new { userID = _userID });
        }

        [HttpPost, ActionName("DeleteSearch")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSearch(int id)
        {
            var _userID = User.Identity.GetUserId();

            var user = db.Users.OfType<Customer>()
                .Include(s => s.SavedSearches)
                .Single(u => u.Id == _userID);

            var search = db.SavedSearches.Find(id);

            user.SavedSearches.Remove(search);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ProfileDetails", new { userID = _userID });
        }

        [AllowAnonymous]
        public ActionResult AnonProfileDetails()
        {
            var _sessionPropertys = Session["Propertys"] as List<Property>;

            if (_sessionPropertys == null)
            {
                _sessionPropertys = new List<Property>();
            }

            return View("ProfileAnonDetails", _sessionPropertys);
        }

        [AllowAnonymous]
        public string SavePropertyAnon(string obj)
        {
            JavaScriptSerializer s = new JavaScriptSerializer();
            var _temp = s.Deserialize<SavePropertyJSON>(obj);

            var property = db.Propertys.Find(_temp.PropertyID);
            var _sessionPropertys = Session["Propertys"] as List<Property>;

            if (_sessionPropertys != null)
            {
                // Flag variable
                var _alreadyExists = false;

                // Iterate through all properties in list
                foreach (var item in _sessionPropertys)
                {
                    // check if property ID fed in from view equals any of the properties in the list
                    if (item.PropertyID == _temp.PropertyID)
                    {
                        // If so change flag to true
                        _alreadyExists = true;
                    }
                }

                // If the property is not in the list, then add it
                if (!_alreadyExists)
                {
                    _sessionPropertys.Add(property);
                    Session["Propertys"] = _sessionPropertys;

                    return "{\"response\":\"Property has been saved to your temporary favourites\"}";
                }
            }
            else
            {
                _sessionPropertys = new List<Property>();
                _sessionPropertys.Add(property);
                Session["Propertys"] = _sessionPropertys;

                return "{\"response\":\"Property has been saved to your temporary favourites\"}";
            }

            return "{\"response\":\"An unexpected error has occurred.  Please try again later.\"}";
        }

        private List<SelectListItem> populateSiteTheme()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Grey", Value = "site.css" });
            items.Add(new SelectListItem { Text = "Gold", Value = "site2.css" });

            return items;
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}