using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusicMVC
{
    public class MusicViewModel
    {
        public string MusicId { get; set; }

        public Music Music { get; set; }

        public SelectList ArtistList { get; set; }

        [Required(ErrorMessage = "Please enter the Artist")]
        [Display(Name = "Artist")]
        public string ArtistId { get; set; }
    }
}
