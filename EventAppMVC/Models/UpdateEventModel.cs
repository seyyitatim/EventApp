﻿using System.ComponentModel.DataAnnotations;

namespace EventAppMVC.Models
{
    public class UpdateEventModel
    {
        public int Id { get; set; }
        [Display(Name = "Başlık")]
        public string Title { get; set; }
        [Display(Name = "Yer")]
        public string Place { get; set; }
        [Display(Name = "Tarih")]
        public DateTime Time { get; set; }
        [Display(Name = "Ücretsiz mi?")]
        public bool IsFree { get; set; }
        [Display(Name = "Fiyat")]
        public float Price { get; set; }
        [Display(Name = "Açıklaması")]
        public string Description { get; set; }
    }
}
