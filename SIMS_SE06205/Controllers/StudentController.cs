using Microsoft.AspNetCore.Mvc;
using SIMS_SE06205.Models;
using Newtonsoft.Json;  

namespace SIMS_SE06205.Controllers
{
    public class StudentController : Controller
    {
        private string filePathStudent = @"D:\apdp\APDP-BTec-main\data-sims\data-student.json";

        [HttpGet]
        public IActionResult Index()
        {
            string dataJson = System.IO.File.ReadAllText(filePathStudent);
            StudentModel studentModel = new StudentModel();
            studentModel.StudentsList = new List<StudentViewModel>();

            // kiem tra username va password co ton tai trong dataJson hay khong ?
            var student = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson);
            var dataStudent = (from s in student select s).ToList();
            foreach (var item in dataStudent)
            {
                studentModel.StudentsList.Add(new StudentViewModel
                {
                    Id = item.Id,
                    StudentCode = item.StudentCode,
                    StudentName = item.StudentName,
                    Birthday = item.Birthday,
                    Address = item.Address,
                    Gender = item.Gender,
                    Phone = item.Phone,
                    Email = item.Email,
                });
            }
            return View(studentModel);
        }

        [HttpGet]
        public IActionResult Add()
        {

            StudentViewModel student = new StudentViewModel();
            return View(student);
        }

        [HttpPost]
        public IActionResult Add(StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem tệp JSON có tồn tại không
                    if (!System.IO.File.Exists(filePathStudent))
                    {
                        // Nếu tệp không tồn tại, khởi tạo danh sách sinh viên rỗng
                        TempData["Error"] = "Data file not found.";
                        return View(studentViewModel);
                    }

                    // Đọc dữ liệu từ tệp JSON
                    string dataJson = System.IO.File.ReadAllText(filePathStudent);
                    var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson) ?? new List<StudentViewModel>();

                    // Tính toán ID mới cho sinh viên
                    int maxId = students.Any() ? students.Max(s => int.Parse(s.Id)) : 0;
                    string newId = (maxId + 1).ToString();

                    // Thêm sinh viên mới vào danh sách
                    students.Add(new StudentViewModel
                    {
                        Id = newId,
                        StudentCode = studentViewModel.StudentCode,
                        StudentName = studentViewModel.StudentName,
                        Birthday = studentViewModel.Birthday,
                        Address = studentViewModel.Address,
                        Gender = studentViewModel.Gender,
                        Phone = studentViewModel.Phone,
                        Email = studentViewModel.Email,
                     
                    });

                    // Lưu danh sách sinh viên đã cập nhật vào tệp JSON
                    var updatedJson = JsonConvert.SerializeObject(students, Formatting.Indented);
                    System.IO.File.WriteAllText(filePathStudent, updatedJson);

                    // Đặt trạng thái lưu thành công
                    TempData["saveStatus"] = true;
                }
                catch (Exception ex)
                {
                    // Đặt trạng thái lưu thất bại và ghi lại thông báo lỗi
                    TempData["saveStatus"] = false;
                    TempData["Error"] = $"Error saving student data: {ex.Message}";
                }

                // Chuyển hướng về trang danh sách sinh viên
                return RedirectToAction(nameof(Index));
            }

            // Nếu mô hình không hợp lệ, trả về view với mô hình hiện tại
            return View(studentViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathStudent);
                var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson);
                var itemToDelete = students.Find(item => item.Id == id.ToString());
                if (itemToDelete != null)
                {
                    students.Remove(itemToDelete);
                    string deletedJson = JsonConvert.SerializeObject(students, Formatting.Indented);
                    System.IO.File.WriteAllText(filePathStudent, deletedJson);
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
            return RedirectToAction(nameof(StudentController.Index), "Student");
        }

        [HttpGet]
        public IActionResult Update(int id = 0)
        {
            string dataJson = System.IO.File.ReadAllText(filePathStudent);
            var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson);
            var itemStudent = students.Find(item => item.Id == id.ToString());

            StudentViewModel studentModel = new StudentViewModel();

            if (itemStudent != null)
            {
                studentModel.Id = itemStudent.Id;
                studentModel.StudentCode = itemStudent.StudentCode;
                studentModel.StudentName = itemStudent.StudentName;
                studentModel.Birthday = itemStudent.Birthday;
                studentModel.Address = itemStudent.Address;
                studentModel.Gender = itemStudent.Gender;
                studentModel.Phone = itemStudent.Phone;
            }

            return View(studentModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(StudentViewModel studentModel)
        {
            // Kiểm tra tính hợp lệ của mô hình
            if (!ModelState.IsValid)
            {
                // Nếu mô hình không hợp lệ, trả về view với mô hình hiện tại
                return View(studentModel);
            }

            try
            {
                // Đọc dữ liệu từ file
                string dataJson = System.IO.File.ReadAllText(filePathStudent);
                var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson);

                // Tìm học sinh cần cập nhật
                var itemStudent = students.Find(item => item.Id == studentModel.Id);

                if (itemStudent != null)
                {
                    // Cập nhật thông tin học sinh
                    itemStudent.StudentCode = studentModel.StudentCode;
                    itemStudent.StudentName = studentModel.StudentName;
                    itemStudent.Birthday = studentModel.Birthday;
                    itemStudent.Address = studentModel.Address;
                    itemStudent.Gender = studentModel.Gender;
                    itemStudent.Phone = studentModel.Phone;
                    itemStudent.Email = studentModel.Email;

                    // Lưu dữ liệu cập nhật vào file
                    string updateJson = JsonConvert.SerializeObject(students, Formatting.Indented);
                    System.IO.File.WriteAllText(filePathStudent, updateJson);

                    // Đặt thông báo trạng thái thành công
                    TempData["UpdateStatus"] = true;
                }
                else
                {
                    // Nếu không tìm thấy học sinh, đặt thông báo trạng thái thành công
                    TempData["UpdateStatus"] = false;
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và đặt thông báo trạng thái thất bại
                TempData["UpdateStatus"] = false;
            }

            // Chuyển hướng về trang danh sách học sinh
            return RedirectToAction(nameof(Index));
        }
    }
}
//using Microsoft.AspNetCore.Mvc;
//using SIMS_SE06205.Models;
//using Newtonsoft.Json;

//namespace SIMS_SE06205.Controllers
//{
//    public class StudentController : Controller
//    {
//        private string filePathStudent = @"D:\apdp\APDP-BTec-main\data-sims\data-students.json";

//        [HttpGet]
//        public IActionResult Index()
//        {
//            string dataJson = System.IO.File.ReadAllText(filePathStudent);
//            StudentModel studentModel = new StudentModel();
//            studentModel.StudentLists = new List<StudentViewModel>();

//            var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson);
//            var dataStudent = (from s in students select s).ToList();
//            foreach (var item in dataStudent)
//            {
//                studentModel.StudentLists.Add(new StudentViewModel
//                {
//                    Id = item.Id,
//                    Name = item.Name,
//                    DateOfBirth = item.DateOfBirth,
//                    Course = item.Course,
//                    Address = item.Address,
//                    PhoneNumber = item.PhoneNumber,
//                    Email = item.Email
//                });
//            }
//            return View(studentModel);
//        }

//        [HttpGet]
//        public IActionResult Add()
//        {
//            StudentViewModel student = new StudentViewModel();
//            return View(student);
//        }

//        [HttpPost]
//        public IActionResult Add(StudentViewModel studentViewModel)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    string dataJson = System.IO.File.ReadAllText(filePathStudent);
//                    var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson);
//                    int maxId = 0;
//                    if (students != null)
//                    {
//                        maxId = int.Parse((from s in students
//                                           select s.Id).Max()) + 1;
//                    }
//                    string idIncrement = maxId.ToString();

//                    students.Add(new StudentViewModel
//                    {
//                        Id = idIncrement,
//                        Name = studentViewModel.Name,
//                        DateOfBirth = studentViewModel.DateOfBirth,
//                        Course = studentViewModel.Course,
//                        Address = studentViewModel.Address,
//                        PhoneNumber = studentViewModel.PhoneNumber,
//                        Email = studentViewModel.Email
//                    });
//                    var dtJson = JsonConvert.SerializeObject(students, Formatting.Indented);
//                    System.IO.File.WriteAllText(filePathStudent, dtJson);
//                    TempData["saveStatus"] = true;
//                }
//                catch
//                {
//                    TempData["saveStatus"] = false;
//                }
//                return RedirectToAction(nameof(StudentController.Index), "Student");
//            }
//            return View(studentViewModel);
//        }

//        [HttpGet]
//        public IActionResult Delete(int id = 0)
//        {
//            try
//            {
//                string dataJson = System.IO.File.ReadAllText(filePathStudent);
//                var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson);
//                var itemToDelete = students.Find(item => item.Id == id.ToString());
//                if (itemToDelete != null)
//                {
//                    students.Remove(itemToDelete);
//                    string deletedJson = JsonConvert.SerializeObject(students, Formatting.Indented);
//                    System.IO.File.WriteAllText(filePathStudent, deletedJson);
//                    TempData["DeleteStatus"] = true;
//                }
//                else
//                {
//                    TempData["DeleteStatus"] = false;
//                }
//            }
//            catch
//            {
//                TempData["DeleteStatus"] = false;
//            }
//            return RedirectToAction(nameof(StudentController.Index), "Student");
//        }

//        [HttpGet]
//        public IActionResult Update(int id = 0)
//        {
//            string dataJson = System.IO.File.ReadAllText(filePathStudent);
//            var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson);
//            var itemStudent = students.Find(item => item.Id == id.ToString());

//            StudentViewModel studentModel = new StudentViewModel();

//            if (itemStudent != null)
//            {
//                studentModel.Id = itemStudent.Id;
//                studentModel.Name = itemStudent.Name;
//                studentModel.DateOfBirth = itemStudent.DateOfBirth;
//                studentModel.Course = itemStudent.Course;
//                studentModel.Address = itemStudent.Address;
//                studentModel.PhoneNumber = itemStudent.PhoneNumber;
//                studentModel.Email = itemStudent.Email;
//            }

//            return View(studentModel);
//        }

//        [HttpPost]
//        public IActionResult Update(StudentViewModel studentModel)
//        {
//            try
//            {
//                string dataJson = System.IO.File.ReadAllText(filePathStudent);
//                var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson);
//                var itemStudent = students.Find(item => item.Id == studentModel.Id.ToString());

//                if (itemStudent != null)
//                {
//                    itemStudent.Name = studentModel.Name;
//                    itemStudent.DateOfBirth = studentModel.DateOfBirth;
//                    itemStudent.Course = studentModel.Course;
//                    itemStudent.Address = studentModel.Address;
//                    itemStudent.PhoneNumber = studentModel.PhoneNumber;
//                    itemStudent.Email = studentModel.Email;
//                    string updateJson = JsonConvert.SerializeObject(students, Formatting.Indented);
//                    System.IO.File.WriteAllText(filePathStudent, updateJson);
//                    TempData["UpdateStatus"] = true;
//                }
//                else
//                {
//                    TempData["UpdateStatus"] = false;
//                }
//            }
//            catch
//            {
//                TempData["UpdateStatus"] = false;
//            }
//            return RedirectToAction(nameof(StudentController.Index), "Student");
//        }
//    }
//}
