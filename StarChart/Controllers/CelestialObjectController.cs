using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.Find(id);
            
            if (result == null) return NotFound();

            result.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();

            return Ok(result);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(x => x.Name == name);

            if (!result.Any()) return NotFound();

            foreach (var celestialObject in result)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();

            }

            return Ok(result.ToList());
        }        
        
        [HttpGet()]
        public IActionResult GetAll()
        {
            var result = _context.CelestialObjects.ToList();


            if (!result.Any()) return NotFound();

            foreach (var celestialObject in result)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();

            }

            return Ok(result.ToList());
        }
    }
}
