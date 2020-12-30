using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;
using RestAPI.Models.Repositories;

namespace RestAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {

        private IServiceRepository repository;

        public PictureController(IServiceRepository repo)
        {
            repository = repo;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult> BusinessImage(long id)
        {
            if(id < 1)
            {
                return BadRequest();
            }
            Business business = await repository.Business.FirstOrDefaultAsync(b => b.BusinessID == id);
            if(business == null)
            {
                return NotFound();
            }
            byte[] b = business.PictureData ?? System.IO.File.ReadAllBytes("Pictures/business-icon.png");
            return File(b, "image/jpeg");
        }

    }
}
