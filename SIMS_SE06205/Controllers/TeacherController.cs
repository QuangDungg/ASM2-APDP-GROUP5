using Microsoft.AspNetCore.Mvc;
using SIMS_SE06205.Models;
using Newtonsoft.Json;

namespace SIMS_SE06205.Controllers
{
    public class TeacherController : Controller
    {
        private string filePathTeacher = @"D:\apdp\APDP-BTec-main\data-sims\data-teacher.json";

        [HttpGet]
        public IActionResult Index()
        {
            string dataJson = System.IO.File.ReadAllText(filePathTeacher);
            TeacherModel teacherModel = new TeacherModel();
            teacherModel.TeachersList = new List<TeacherViewModel>();

            var teachers = JsonConvert.DeserializeObject<List<TeacherViewModel>>(dataJson);
            var dataTeacher = (from t in teachers select t).ToList();
            foreach (var item in dataTeacher)
            {
                teacherModel.TeachersList.Add(new TeacherViewModel
                {
                    Id = item.Id,
                    TeacherCode = item.TeacherCode,
                    TeacherName = item.TeacherName,
                    Birthday = item.Birthday,
                    Address = item.Address,
                    Gender = item.Gender,
                    Phone = item.Phone,
                    Email = item.Email,
                });
            }
            return View(teacherModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            TeacherViewModel teacher = new TeacherViewModel();
            return View(teacher);
        }

        [HttpPost]
        public IActionResult Add(TeacherViewModel teacherViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string dataJson = System.IO.File.ReadAllText(filePathTeacher);
                    var teachers = JsonConvert.DeserializeObject<List<TeacherViewModel>>(dataJson);
                    int maxId = 0;
                    if (teachers != null)
                    {
                        maxId = int.Parse((from t in teachers
                                           select t.Id).Max()) + 1;
                    }
                    string idIncrement = maxId.ToString();

                    teachers.Add(new TeacherViewModel
                    {
                        Id = idIncrement,
                        TeacherCode = teacherViewModel.TeacherCode,
                        TeacherName = teacherViewModel.TeacherName,
                        Birthday = teacherViewModel.Birthday,
                        Address = teacherViewModel.Address,
                        Gender = teacherViewModel.Gender,
                        Phone = teacherViewModel.Phone,
                    });
                    var dtJson = JsonConvert.SerializeObject(teachers, Formatting.Indented);
                    System.IO.File.WriteAllText(filePathTeacher, dtJson);
                    TempData["saveStatus"] = true;
                }
                catch
                {
                    TempData["saveStatus"] = false;
                }
return RedirectToAction(nameof(TeacherController.Index), "Teacher");
            }
            return View(teacherViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathTeacher);
                var teachers = JsonConvert.DeserializeObject<List<TeacherViewModel>>(dataJson);
                var itemToDelete = teachers.Find(item => item.Id == id.ToString());
                if (itemToDelete != null)
                {
                    teachers.Remove(itemToDelete);
                    string deletedJson = JsonConvert.SerializeObject(teachers, Formatting.Indented);
                    System.IO.File.WriteAllText(filePathTeacher, deletedJson);
                    TempData["DeleteStatus"] = true;
                }
                else
                {
                    TempData["DeleteStatus"] = false;
                }
            }
            catch
            {
                TempData["DeleteStatus"] = false;
            }
            return RedirectToAction(nameof(TeacherController.Index), "Teacher");
        }

        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            string dataJson = System.IO.File.ReadAllText(filePathTeacher);
            var teachers = JsonConvert.DeserializeObject<List<TeacherViewModel>>(dataJson);
            var itemTeacher = teachers.Find(item => item.Id == id.ToString());

            TeacherViewModel teacherModel = new TeacherViewModel();

            if (itemTeacher != null)
            {
                teacherModel.Id = itemTeacher.Id;
                teacherModel.TeacherCode = itemTeacher.TeacherCode;
                teacherModel.TeacherName = itemTeacher.TeacherName;
                teacherModel.Birthday = itemTeacher.Birthday;
                teacherModel.Address = itemTeacher.Address;
                teacherModel.Gender = itemTeacher.Gender;
                teacherModel.Phone = itemTeacher.Phone;
            }

            return View(teacherModel);
        }

        [HttpPost]
        public IActionResult Update(TeacherViewModel teacherModel)
        {
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathTeacher);
                var teachers = JsonConvert.DeserializeObject<List<TeacherViewModel>>(dataJson);
                var itemTeacher = teachers.Find(item => item.Id == teacherModel.Id.ToString());

                if (itemTeacher != null)
                {
                    itemTeacher.TeacherCode = teacherModel.TeacherCode;
                    itemTeacher.TeacherName = teacherModel.TeacherName;
                    itemTeacher.Birthday = teacherModel.Birthday;
                    itemTeacher.Address = teacherModel.Address;
                    itemTeacher.Gender = teacherModel.Gender;
                    itemTeacher.Phone = teacherModel.Phone;
string updateJson = JsonConvert.SerializeObject(teachers, Formatting.Indented);
                    System.IO.File.WriteAllText(filePathTeacher, updateJson);
                    TempData["UpdateStatus"] = true;
                }
                else
                {
                    TempData["UpdateStatus"] = false;
                }
            }
            catch (Exception ex)
            {
                TempData["UpdateStatus"] = false;
            }
            return RedirectToAction(nameof(TeacherController.Index), "Teacher");
        }
    }
}
