using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatToyWebApp.Pages
{
    public class PhotosModel : PageModel
    {
        public async void OnGet()
        {
            await LoadImages();
        }

        public List<string> imagesList = new List<string>();
        public string imgPath { get; set; }

        string PhotoDirectory = $"{Directory.GetCurrentDirectory()}{@"/wwwroot/CatPhotos/"}";
        public async Task LoadImages()
        {
            var files = Directory.GetFiles(PhotoDirectory);
            foreach (var file in files)
            {
                imagesList.Add("CatPhotos/" + Path.GetFileName(file));
            }
        }
    }
}
