using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatToyWebApp.Pages
{
    public class VideosModel : PageModel
    {
        public async void OnGet()
        {
            await LoadVideos();
        }

        public List<string> videosList = new List<string>();

        string VideoDirectory = $"{Directory.GetCurrentDirectory()}{@"/wwwroot/CatVideos/"}";

        public async Task LoadVideos()
        {
            var files = Directory.GetFiles(VideoDirectory);
            foreach (var file in files)
            {
                videosList.Add("CatVideos/" + Path.GetFileName(file));
            }
        }
    }
}
