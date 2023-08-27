// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Career.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Career.Areas.Identity.Pages.Account.Manage;

public class DeletePersonalDataModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<DeletePersonalDataModel> _logger;
    private readonly ApplicationDbContext _context;

    public DeletePersonalDataModel(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<DeletePersonalDataModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _context = context;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public bool RequirePassword { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        RequirePassword = await _userManager.HasPasswordAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        RequirePassword = await _userManager.HasPasswordAsync(user);
        if (RequirePassword)
        {
            if (!await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                return Page();
            }
        }

        var result = await _userManager.DeleteAsync(user);
        await DeleteUserData(user.Id);
        var userId = await _userManager.GetUserIdAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Unexpected error occurred deleting user.");
        }

        await _signInManager.SignOutAsync();

        _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

        return Redirect("~/");
    }

    private async Task DeleteUserData(string userId)
    {
        var deleteEntities = new List<Task>
        {
            _context.Experiences.Where(e => e.UserId == userId).ExecuteDeleteAsync(),
            _context.Educations.Where(e => e.UserId == userId).ExecuteDeleteAsync(),
            _context.Contacts.Where(e => e.UserId == userId).ExecuteDeleteAsync(),
            _context.Projects.Where(e => e.UserId == userId).ExecuteDeleteAsync(),
            _context.PrivateData.Where(e => e.UserId == userId).ExecuteDeleteAsync(),
            _context.Summaries.Where(e => e.UserId == userId).ExecuteDeleteAsync(),
            _context.Abilities.Where(e => e.UserId == userId).ExecuteDeleteAsync(),
            _context.AppliedJobs.Where(e => e.UserId == userId).ExecuteDeleteAsync(),
            _context.Messages.Where(e => e.UserId == userId).ExecuteDeleteAsync()
        };

        await Task.WhenAll(deleteEntities);
    }
}
