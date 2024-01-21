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
// [Authorize]
 public IActionResult GetById(int id)
 {
    var chore = _dbContext
    .Chores
    .Include(c => c.ChoreAssignments)
    .Include(c => c.ChoreCompletions)
    .SingleOrDefault(c => c.Id == id);
    if (chore == null)
    {
        return NotFound();
    }
    var choreDto = new ChoreDTO
    {
        Id = chore.Id,
        Name = chore.Name,
        Difficulty = chore.Difficulty,
        ChoreFrequencyDays = chore.ChoreFrequencyDays,
        ChoreAssignments = chore.ChoreAssignments.Select(ca => new ChoreAssignmentDTO
        {
            Id = ca.Id,
            UserProfileId = ca.UserProfileId,
            ChoreId = ca.ChoreId
        }).ToList(),
        ChoreCompletions = chore.ChoreCompletions.Select(cc => new ChoreCompletionDTO
        {
            Id = cc.Id,
            UserProfileId = cc.UserProfileId,
            ChoreId = cc.ChoreId,
            CompletedOn = cc.CompletedOn
        }).ToList()
    };
    return Ok(choreDto);
 }   

 [HttpPost("{id}/complete")]
 //[Authorize]
 public IActionResult CompleteChore(int id, int choreId)
 {
    UserProfile userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.Id == id);
        if (userProfile == null)
        {
            return NotFound();
        }
    Chore chore = _dbContext.Chores.SingleOrDefault(c => c.Id == choreId);
        if (chore == null)
        {
            return NotFound();
        }
    ChoreCompletion newCompletion = new ChoreCompletion()
    {
        UserProfileId = id,
        ChoreId = choreId,
        CompletedOn = DateTime.Today
    };

    _dbContext.ChoreCompletions.Add(newCompletion);
    _dbContext.SaveChanges();
    return NoContent();
 }

[HttpPost]
[Authorize(Roles = "Admin")]
public IActionResult CreateChore(string name, int difficulty, int choreFrequencyDays)
{
    Chore newChore = new Chore()
    {
        Name = name,
        Difficulty = difficulty,
        ChoreFrequencyDays = choreFrequencyDays
    };
    _dbContext.Chores.Add(newChore);
    _dbContext.SaveChanges();
    return NoContent();
}
}