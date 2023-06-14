using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restapiapp.Data;
using restapiapp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace restapiapp.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/course/create")]
        public IActionResult CreateCourse([FromBody] Course courseData)
        {

            Course course = new Course
            {
                Id = courseData.Id,
                Title = courseData.Title,
                Description = courseData.Description,
            };
            _context.Courses.Add(course);
            _context.SaveChanges();

            return Ok(course);
        }

        [HttpGet]
        [Route("/courses/read")]
        public IActionResult ReadCourses()
        {
            var coursesList = _context.Courses.ToList();

            var coursesWithAuthors = coursesList.Select(course => new
            {
                CourseId = course.CourseId,
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                AuthorName = _context.Users.FirstOrDefault(user => user.Id == course.Id).Name
        }).ToList();

            return Ok(coursesWithAuthors);
        }

        [HttpPost]
        [Route("/course/{courseId}/join")]
        public IActionResult JoinCourse(string courseId)
        {
            var userId = Request.Headers["userId"];

            UserCourse userCourse = new UserCourse
            {
                CourseId = courseId,
                UserId = userId
            };
            _context.UserCourses.Add(userCourse);
            _context.SaveChanges();

            return Ok("Joining succeeded");
        }

        [HttpGet]
        [Route("/courses/recently-viewed/read")]
        public IActionResult ReadRecentlyViewedCourses()
        {
            var recentlyViewedCoursesList = new List<Course>();
            string recentlyViewedCourses = Request.Headers["courses"];

            if (!string.IsNullOrEmpty(recentlyViewedCourses))
            {
                string[] recentlyViewedCoursesSplit = recentlyViewedCourses.Split(",");

                foreach (var courseId in recentlyViewedCoursesSplit)
                {
                    Console.WriteLine(courseId);
                    var course = _context.Courses.FirstOrDefault(course => course.CourseId.ToString() == courseId);

                    if (course != null)
                    {
                        recentlyViewedCoursesList.Add(course);
                    }

                    Console.WriteLine("test start");
                    Console.WriteLine("test stop");
                }
            }

            return Ok(recentlyViewedCoursesList);
        }

        public class CourseWithModules
        {
            public int CourseId { get; set; }
            public string CourseTitle { get; set; }
            public string CourseDescription { get; set; }
            public List<Module> Modules { get; set; }
        }

        [HttpGet]
        [Route("/course-details/{courseId}/read")]
        public IActionResult ReadCourseDetails(int courseId)
        {
            var courseDetailsWithModules = _context.Courses
            .Where(course => course.CourseId == courseId)
            .Select(course => new CourseWithModules
            {
                CourseId = course.CourseId,
                CourseTitle = course.Title,
                CourseDescription = course.Description,
                Modules = _context.Modules
                    .Where(module => module.CourseId == course.CourseId)
                    .ToList()
            })
            .FirstOrDefault();

            return Ok(courseDetailsWithModules);
        }

        public class FileViewModel
        {
            public string FileName { get; set; }
            public byte[] FileContent { get; set; }
        }

        [HttpGet]
        [Route("/course-details/{courseId}/{moduleId}/files")]
        public IActionResult ReadFile(int courseId, int moduleId)
        {
            var files = new List<FileViewModel>();
            string uploadsFolderPath = Path.Combine("uploads/course_" + courseId, "module_" + moduleId);
            if (Directory.Exists(uploadsFolderPath))
            {
                string[] fileNames = Directory.GetFiles(uploadsFolderPath);

              
                foreach (string fileName in fileNames)
                {
                    byte[] fileContent = System.IO.File.ReadAllBytes(fileName);

                    var fileViewModel = new FileViewModel
                    {
                        FileName = Path.GetFileName(fileName),
                        FileContent = fileContent
                    };

                    files.Add(fileViewModel);
                }
            }
            return Ok(files);
        }

        [HttpPost]
        [Route("/module/create")]
        public IActionResult CreateModule([FromBody] Module moduleData)
        {

            Module module = new Module
            {
                ModuleId = moduleData.ModuleId,
                CourseId = moduleData.CourseId,
                Title = moduleData.Title,
                Content = moduleData.Content,
            };
            _context.Modules.Add(module);
            _context.SaveChanges();

            return Ok(module);
        }

        [HttpPost]
        [Route("/module/file")]
        public IActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0]; // Pobierz przesłany plik

                // Sprawdź, czy plik został przesłany
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Nie wybrano pliku do przesłania.");
                }

                // Przykładowa logika obsługi przesłanego pliku

                var folderPath = Path.Combine("uploads", "course_" + Request.Form["courseId"], "module_" + Request.Form["moduleId"]);

                Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    file.CopyTo(stream); // Zapisz plik na serwerze
                }

                return Ok("Plik dodany poprawnie!");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Wystąpił błąd podczas przesyłania pliku: " + ex.Message);
            }
        }
    }
}

