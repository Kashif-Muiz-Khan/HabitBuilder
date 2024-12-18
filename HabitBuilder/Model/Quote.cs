﻿using System.ComponentModel.DataAnnotations;

namespace HabitBuilder.Model
{
    public class Quote // Quote model
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Quote")]
        [StringLength(250, ErrorMessage = "The Quote must not exceed 250 characters.")]
        public string QuoteText { get; set; }

        [Required(ErrorMessage = "Please enter the Author")]
        public string Author { get; set; }

    }
}
