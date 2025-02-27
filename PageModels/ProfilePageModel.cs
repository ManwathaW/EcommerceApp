using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EcommerceApp.Utilities;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using SQLite;
using EcommerceApp.Models;
using EcommerceApp.Services;
using EcommerceApp.Interfaces;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using System.Text.RegularExpressions;
using EcommerceApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.PageModels
{
    public partial class ProfilePageModel : BasePageModel
    {
        private readonly IMediaPicker _mediaPicker;
        private readonly IFileSystem _fileSystem;

        [ObservableProperty]
        private Profile profile = new();

        [ObservableProperty]
        private bool isEditMode;

        [ObservableProperty]
        private ImageSource profileImage;

        public ProfilePageModel(
       DatabaseContext database,
       IMediaPicker mediaPicker,
       IFileSystem fileSystem) : base(database)
        {
            _mediaPicker = mediaPicker;
            _fileSystem = fileSystem;
            Title = "My Profile";
        }

        public override async Task InitializeAsync()
        {
            await ExecuteBusyActionAsync(async () =>
            {
                var profiles = await Database.GetProfilesAsync();
                Profile = profiles.FirstOrDefault() ?? new Profile();
                await LoadProfileImageAsync();
            });
        }

        private async Task LoadProfileImageAsync()
        {
            if (!string.IsNullOrEmpty(Profile?.ProfileImagePath) &&
                File.Exists(Profile.ProfileImagePath))
            {
                ProfileImage = ImageSource.FromFile(Profile.ProfileImagePath);
            }
            else
            {
                ProfileImage = ImageSource.FromFile("default_profile.png");
            }
        }

        [RelayCommand]
        private async Task SaveProfile()
        {
            await ExecuteBusyActionAsync(async () =>
            {
                if (ValidateProfile())
                {
                    await Database.SaveProfileAsync(Profile);
                    IsEditMode = false;
                    await Shell.Current.DisplayAlert("Success", "Profile saved successfully!", "OK");
                }
            });
        }

        private bool ValidateProfile()
        {
            if (string.IsNullOrWhiteSpace(Profile.Name))
            {
                Shell.Current.DisplayAlert("Error", "Name is required", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Profile.EmailAddress))
            {
                Shell.Current.DisplayAlert("Error", "Email address is required", "OK");
                return false;
            }

            return true;
        }

        [RelayCommand]
        private void ToggleEditMode()
        {
            IsEditMode = !IsEditMode;
        }

        [RelayCommand]
        private async Task ChangePhoto()
        {
            await ExecuteBusyActionAsync(async () =>
            {
                try
                {
                    var photo = await _mediaPicker.PickPhotoAsync();
                    if (photo != null)
                    {
                        var newPath = await ProcessNewProfilePhoto(photo);
                        Profile.ProfileImagePath = newPath;
                        await LoadProfileImageAsync();
                    }
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error",
                        $"Failed to change photo: {ex.Message}", "OK");
                }
            });
        }

        private async Task<string> ProcessNewProfilePhoto(FileResult photo)
        {
            var imagesFolder = Path.Combine(FileSystem.AppDataDirectory, "ProfileImages");
            Directory.CreateDirectory(imagesFolder);

            if (!string.IsNullOrEmpty(Profile.ProfileImagePath) &&
                File.Exists(Profile.ProfileImagePath))
            {
                File.Delete(Profile.ProfileImagePath);
            }

            var fileName = $"profile_image_{DateTime.Now.Ticks}{Path.GetExtension(photo.FileName)}";
            var newFile = Path.Combine(imagesFolder, fileName);

            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
            {
                await stream.CopyToAsync(newStream);
            }

            return newFile;
        }

        [RelayCommand]
        private async Task Refresh()
        {
            IsRefreshing = true;
            await InitializeAsync();
            IsRefreshing = false;
        }
    }
}
