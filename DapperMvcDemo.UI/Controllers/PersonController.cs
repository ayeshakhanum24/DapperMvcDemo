using DapperMvcDemo.Data.Models.Domain;
using DapperMvcDemo.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DapperMvcDemo.UI.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonRepository _personRepo;
        private readonly IDepartmentRepository _departmentRepo;

        public PersonController(IPersonRepository personRepo, IDepartmentRepository departmentRepo)
        {
            _personRepo = personRepo;
            _departmentRepo = departmentRepo;
        }

        public async Task<IActionResult> Add()
        {
            var departments = await _departmentRepo.GetAllAsync();
            ViewBag.Departments = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Person person)
        {
            try
            {
                if (!ModelState.IsValid) 
                    return View(person);
                
                var addPersonResult = await _personRepo.AddAsync(person);
                if (addPersonResult)
                    TempData["msg"] = "Successfully added";
                else
                    TempData["msg"] = "Not added";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Not added";
            }
            return RedirectToAction(nameof(Add));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var person = await _personRepo.GetByIdAsync(id);
            var departments = await _departmentRepo.GetAllAsync();  // ← must return non-null list
            ViewBag.Departments = departments;

            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Person person)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var departments = await _departmentRepo.GetAllAsync();
                    ViewBag.Departments = departments;
                    return View(person);
                }

                var updateResult = await _personRepo.UpdateAsync(person);
                if (updateResult)
                {
                    TempData["msg"] = "Updated successfully";
                    return RedirectToAction(nameof(DisplayAll));
                }
                else
                {
                    TempData["msg"] = "Could not update";
                }
            }
            catch
            {
                TempData["msg"] = "Could not update";
            }

            var departmentsRetry = await _departmentRepo.GetAllAsync();
            ViewBag.Departments = departmentsRetry;
            return View(person);
        }


        public async Task<IActionResult> DisplayAll()
        {
            var people = await _personRepo.GetAllAsync(); // Fetch all people
            var departments = await _departmentRepo.GetAllAsync();  // Fetch all departments

            // Map DeptName to each person based on DeptId
            foreach (var person in people)
            {
                var department = departments.FirstOrDefault(d => d.DeptId == person.DeptId);
                if (department != null)
                {
                    person.DeptName = department.DeptName; // Assign DeptName to the person
                }
            }

            return View(people); // Return updated people list with DeptName
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPeople()
        {
            var people = await _personRepo.GetAllAsync();
            var departments = await _departmentRepo.GetAllAsync();

            foreach (var person in people)
            {
                var department = departments.FirstOrDefault(d => d.DeptId == person.DeptId);
                if (department != null)
                {
                    person.DeptName = department.DeptName;
                }
            }

            return Json(people);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var deleteResult = await _personRepo.DeleteAsync(id);
            return RedirectToAction(nameof(DisplayAll));
        }
        public IActionResult FromApi()
        {
            return View();
        }

    }
}
