using Allurerealstate.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Allurerealstate.Controllers;

public class HouseController : Controller
{
    private readonly AppDbContext _context;

    public HouseController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var houses = _context.Properties.ToList();

        return View(houses);
    }

    public IActionResult Detail(int id)
    {
        var listing = _context.Properties.Find(id);
        if (listing == null)
        {
            return NotFound();
        }

        return View(listing);
    }
}