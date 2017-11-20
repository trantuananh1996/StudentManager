using exam.Models;
using exam.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam.Utils;
using Microsoft.AspNetCore.Authorization;
using exam.Models.Post;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace exam.Controllers
{
    [Route("api/students")]
    public class StudentController : Controller
    {
        StudentRepository studentRepository;
        NationRepository nationRepository;
        ReligionRepository religionRepository;
        public StudentController(StudentRepository user, NationRepository nat,ReligionRepository rel)
        {
            studentRepository = user;
            nationRepository = nat;
            religionRepository = rel;
        }
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var students =await studentRepository.GetAll();
            return View();
        }

        /// <summary>
        /// Get a property info object from Item class filtering by property name.
        /// </summary>
        /// <param name="name">name of the property</param>
        /// <returns>property info object</returns>
        private PropertyInfo getProperty(string name)
        {
            var properties = typeof(Student).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLower().Equals(name.ToLower()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        /// <summary>
        /// Process a list of items according to Form data parameters
        /// </summary>
        /// <param name="lstElements">list of elements</param>
        /// <param name="requestFormData">collection of form data sent from client side</param>
        /// <returns>list of items processed</returns>
        private List<Student> ProcessCollection(List<Student> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = getProperty(columName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                    }
                    else
                        return lstElements;
                }
            }
            return null;
        }

        [HttpGet("show")]
        public async Task<IActionResult> PostAsync()
        {
            //Get form data from client side
            List<Student> lstItems =  await studentRepository.GetAll();


            // Custom response to bind information in client side
            dynamic response = new
            {
                Data = lstItems,
                RecordsFiltered = lstItems.Count,
                RecordsTotal = lstItems.Count
            };
            return Ok(response);
        }

        [HttpGet("find")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<ActionResult> FindStudentByName(string name)
        {
            List<Student> students= await studentRepository.FindStudentByName(name);
            if (students == null || students.Count() == 0)
                return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });
            else return Ok(new { status = ResultStatus.STATUS_OK, data = students });
        }

        [HttpPost("list")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard,Teacher")]
        public async Task<IActionResult> List([FromBody] PostClassId Class)
        {
            if (Class.ClassId==0)
            {
                var users = await studentRepository.GetAll();
                return Ok(new {status = ResultStatus.STATUS_OK, data= users });
            }
            else
            {
                var users = await studentRepository.FindStudentByClass(Class.ClassId);
                return Ok(new { status = ResultStatus.STATUS_OK, data = users });
            }
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard,Teacher")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await studentRepository.Get(id);
            if (user == null) return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });
            return Ok(new { status=ResultStatus.STATUS_OK,data=user });
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Teacher,SchoolBoard")]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] Student u)
        {
            if (String.IsNullOrEmpty(u.FullName)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message="Tên học sinh không được để trống"});

            if (u.Nation!=null)  await nationRepository.Create(u.Nation);
            if (u.Religion!=null) await religionRepository.Create(u.Religion);
            await studentRepository.Create(u);
            return Ok(new
            {  
                status=ResultStatus.STATUS_OK,
                data = u
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard,Teacher")]
        [HttpPost("edit")]
        public async Task<IActionResult> Edit( [FromBody] Student student)
        {
            var user = await studentRepository.Get(student.Id);
            if (user == null) return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });

            if (String.IsNullOrEmpty(student.FullName)) return Ok(new { status = ResultStatus.STATUS_INVALID_INPUT, message = "Tên học sinh không được để trống" });
            
            await studentRepository.Create(student);
            return Ok(new { status=ResultStatus.STATUS_OK,message = "Sửa thông tin học sinh thành công", data = student });
        }

        [HttpDelete("{id}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "SchoolBoard,Teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await studentRepository.Get(id);
            if (user == null) return Ok(new { status = ResultStatus.STATUS_NOT_FOUND, message = "Không tìm thấy học sinh" });
            await studentRepository.Delete(id);
            return Ok(new { status =ResultStatus.STATUS_OK,message = "Xóa thành công!" });
        }




    }
}
