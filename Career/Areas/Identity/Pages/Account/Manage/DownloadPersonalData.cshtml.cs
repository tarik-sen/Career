// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Career.Data;
using Career.Models;
using Career.Models.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Career.Areas.Identity.Pages.Account.Manage;

public class DownloadPersonalDataModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DownloadPersonalDataModel> _logger;

    public DownloadPersonalDataModel(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        ILogger<DownloadPersonalDataModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
        _context = context;
    }

    public IActionResult OnGet()
    {
        return NotFound();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

        List<Dictionary<string, string>> userData = new();

        var privateData = await _context.PrivateData.Include(p => p.UserNationality).FirstOrDefaultAsync(p => p.UserId == user.Id);
        userData.Add(GetUserTableData(privateData));

        var contactData = await _context.Contacts.Include(p => p.UserCountry).Include(p => p.UserCity).FirstOrDefaultAsync(p => p.UserId == user.Id);
        userData.Add(GetUserTableData(contactData));


        Dictionary<string, string> personalData = new();

        var personalDataProps = typeof(IdentityUser).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
        foreach (var p in personalDataProps)
        {
            personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");

        }

        personalData.Add($"Authenticator Key", await _userManager.GetAuthenticatorKeyAsync(user));
        userData.Add(personalData);

        Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
        return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(userData), "application/json");
    }

    private static Dictionary<string, string> GetUserTableData<T>(T tableData)
    {
        Dictionary<string, string> userData = new();

        if (tableData == null)
            return userData;

        var tableDataProp = typeof(T).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

        foreach (var p in tableDataProp)
        {
            if (p.Name.StartsWith("User") && p.GetValue(tableData) != null)
                userData.Add(p.Name, ((IDatasetBaseEntityModel)p.GetValue(tableData)).Title);
            else
                userData.Add(p.Name, p.GetValue(tableData)?.ToString() ?? "null");

        }

        return userData;
    }
}
