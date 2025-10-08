using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TestCase.Components.Dialogs;

namespace TestCase.Services
{
    public interface IImagePreviewService
    {
        Task ShowImagePreview(string imageUrl, string title = "图片预览");
        Task ShowImagePreview(int imageId, string title = "图片预览");
    }

    public class ImagePreviewService : IImagePreviewService
    {
        private readonly IDialogService _dialogService;
        private readonly NavigationManager _navigationManager;

        public ImagePreviewService(IDialogService dialogService, NavigationManager navigationManager)
        {
            _dialogService = dialogService;
            _navigationManager = navigationManager;
        }

        public async Task ShowImagePreview(string imageUrl, string title = "图片预览")
        {
            var parameters = new DialogParameters();
            parameters.Add("ImageUrl", imageUrl);
            parameters.Add("Title", title);

            var dialog = await _dialogService.ShowAsync<ImagePreviewDialog>(title, parameters, new DialogOptions()
            {
                CloseButton = true,
                BackdropClick = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = true
            });
        }

        public async Task ShowImagePreview(int imageId, string title = "图片预览")
        {
            var imageUrl = $"{_navigationManager.BaseUri}api/image/{imageId}";
            await ShowImagePreview(imageUrl, title);
        }
    }
}