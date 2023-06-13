using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restapiapp.Models
{
	public class Module
	{
        [Key]
        public int ModuleId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}

