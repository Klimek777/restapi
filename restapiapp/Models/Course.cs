using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restapiapp.Models
{
	public class Course
	{
        [Key]
        public int CourseId { get; set; }
        [ForeignKey("User")]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

    }
}

