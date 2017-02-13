using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalWebService.Models
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Description { get; set; }

        [Required, StringLength(255)]
        [Display(Name = "Thread")]
        public string Thread { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Level")]
        public string Level { get; set; }

        [Required, StringLength(255)]
        [Display(Name = "Logger")]
        public string Logger { get; set; }

        [Required, StringLength(4000)]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Required, StringLength(2000)]
        [Display(Name = "Exception")]
        public string Exception { get; set; }
    }
}