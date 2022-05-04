using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedThings.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace VaccinApi.Controllers
{
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes =
    //    JwtBearerDefaults.AuthenticationScheme)]
    public class VaccinController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public VaccinController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [SwaggerOperation(OperationId = "List")]
        public ActionResult<IEnumerable<Vaccin>> List()
        {
            return Ok(_dbContext.Vacciner.Include(e => e.Supplier));
        }


        [Route("{id}")]
        [HttpGet]
        [SwaggerOperation(OperationId = "GetOne")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Vaccin> GetSingle(int id)
        {
            var user = User;
            var vaccin = _dbContext.Vacciner.Include(e => e.Supplier).FirstOrDefault(e => e.Id == id);
            if (vaccin == null) return NotFound();

            return Ok(vaccin);
        }


        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(OperationId = "Update")]

        public ActionResult<Vaccin> Update(Vaccin vaccin)
        {
            var v = _dbContext.Vacciner.FirstOrDefault(r => r.Id == vaccin.Id);
            v.AntalDoser = vaccin.AntalDoser;
            v.Comment = vaccin.Comment;
            v.EuOkStatus = vaccin.EuOkStatus;
            v.Namn = vaccin.Namn;
            v.Supplier = vaccin.Supplier;
            v.Type = vaccin.Type;
            _dbContext.SaveChanges();

            return Ok(vaccin);
        }


        [HttpPost]

        [SwaggerOperation(OperationId = "Create")]
        public ActionResult<Vaccin> Create(Vaccin vaccin)
        {
            _dbContext.Vacciner.Add(vaccin);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetSingle), new { id = vaccin.Id }, vaccin);
        }
    }
}