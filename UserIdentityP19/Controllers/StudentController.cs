using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserIdentityP19;
using UserIdentityP19.Models;
using UserIdentityP19.Repository.StudentRepo;

namespace UserIdentityP19.Controllers
{
    public class StudentController(IStudentRepository studentRepository, IMapper mapper) : Controller
    {
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index()
        {
            var students = await studentRepository.GetAllAsync();
            var studentVMs = mapper.Map<List<StudentViewModel>>(students);
            return View(studentVMs);
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Details(int id)
        {
            var student = await studentRepository.GetByIdAsync(id);
            if (student == null) return NotFound();
            var viewModel = mapper.Map<StudentViewModel>(student);
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,EnrollmentNo,Email,Course,AdmissionDate")] Student studentData)
        {
            var student = mapper.Map<Student>(studentData);
            await studentRepository.AddAsync(student);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await studentRepository.GetByIdAsync(id);
            if (student == null) return NotFound();
            var viewModel = mapper.Map<StudentViewModel>(student);
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,EnrollmentNo,Email,Course,AdmissionDate")] Student viewModel)
        {
            if (id != viewModel.Id) return BadRequest();
            if (!ModelState.IsValid) return View(viewModel);

            var student = mapper.Map<Student>(viewModel);
            await studentRepository.UpdateAsync(student);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await studentRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
