using Luxa.Interfaces;
using Luxa.Models;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Luxa.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly IUserService _userService;

        public SettingsService(IUserService userService, SignInManager<UserModel> signInManager,
            UserManager<UserModel> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userService = userService;
        }

        async Task<bool> SetNewPassword(UserModel user, string oldPassword, string newPassword)
        {
            var result = await _signInManager.UserManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        async Task<bool> ChangeEmail(UserModel userModel, string newEmail)
        {
            var token = await _userManager.GenerateChangeEmailTokenAsync(userModel, newEmail);
            var result = await _userManager.ChangeEmailAsync(userModel, newEmail, token);
            return result.Succeeded;
        }

        public async Task<string> ChangePassword(UserModel? user, string oldPassword, string newPassword)
        {
            if (user != null)
            {
                if (await SetNewPassword(user, oldPassword, newPassword))
                {
                    return "Hasło zostało pomyślnie zmienione";
                }
            }
            return "Nie można zmienić hasła. Sprawdź poprawność wprowadzonych danych";
        }

        public DataChangeVM? GetDataChangeVMFromUser(UserModel? user)
        {
            if (user != null)
            {
                var dataChangeVM = new DataChangeVM
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Country = user.Country,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                };
                return dataChangeVM;
            }
            return null;
        }

        public async Task<string> ChangeData(UserModel? user, bool isValid, DataChangeVM dataChangeVM)
        {
            if (isValid && user != null && dataChangeVM.Email != null)
            {
                string emailNotification;
                user.FirstName = dataChangeVM.FirstName;
                user.LastName = dataChangeVM.LastName;
                user.Country = dataChangeVM.Country;
                user.PhoneNumber = dataChangeVM.PhoneNumber;
                emailNotification = user.Email != null && !user.Email.Equals(dataChangeVM.Email, StringComparison.OrdinalIgnoreCase) && await ChangeEmail(user, dataChangeVM.Email)
                    ? " Zmieniono adres E-mail\n"
                    : "";
                if (await _userService.SaveUser(user))
                {
                    return "Zmiana danych powiodła się." + emailNotification;
                }
            }
            return "Coś poszło nie tak.";
        }

        public async Task<string> ChangePrivacy(UserModel? user, bool isPrivate)
        {
            if (user != null)
            {
                user.IsPrivate = isPrivate;
                if (await _userService.SaveUser(user))
                {
                    return "Zmiana ustawień prywatności powiodła się.";
                }
            }
            return "Coś poszło nie tak.";
        }

        public async Task<string> ChangeProfile(UserModel? user, string Description)
        {
            if (user != null)
            {
                user.Description = Description;
                if (await _userService.SaveUser(user))
                {
                    return "Zmiana opisu powiodła się.";
                }
            }
            return "Coś poszło nie tak.";
        }
    }
}
