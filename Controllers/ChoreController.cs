using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HouseRules.Data;
using HouseRules.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using HouseRules.Models;
using Microsoft.AspNetCore.Identity;

namespace HouseRules.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChoreController : ControllerBase
{
    private HouseRulesDbContext _dbContext;

    public ChoreController(HouseRulesDbContext context)
    {
        _dbContext = context;
    }
//endpoints go here
[HttpGet]
// [Authorize]
public IActionResult Get()
{
    return Ok(_dbContext
        .Chores
        .Include(c => c.ChoreAssignments)
        .Include(c => c.ChoreCompletions)
        .Select(c => new ChoreDTO
        {
            Id = c.Id,
            Name = c.Name,
            Difficulty = c.Difficulty,
            ChoreFrequencyDays = c.ChoreFrequencyDays,
            ChoreAssignments = c.ChoreAssignments.Select(ca => new ChoreAssignmentDTO
            {
                Id = ca.Id,
                UserProfileId = ca.UserProfileId,
                ChoreId = ca.ChoreId
            }).ToList(),
            ChoreCompletions = c.ChoreCompletions.Select(cc => new ChoreCompletionDTO
            {
                Id = cc.Id,
                UserProfileId = cc.UserProfileId,
                ChoreId = cc.ChoreId,
                CompletedOn = cc.CompletedOn
            }).ToList()
        }).ToList()
        );
}

//return chore by id with all current assignees and completions
[HttpGet("{id}")]
[Authorize]
 public IActionResult GetById(int id)
 {
    Chore chore = _dbContext
    .Chores
    .Include(c => c.ChoreAssignments)
    .Include(c => c.ChoreCompletions)
    .SingleOrDefault(c => c.Id == id);
    if (chore == null)
    {
        return NotFound();
    }
    return Ok(chore);
 }   

}