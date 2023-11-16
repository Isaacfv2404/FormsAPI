namespace FormsAPI.Controllers;
using FormsAPI.Data;
using FormsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FormDataController : ControllerBase
{
    private readonly FormsAPIContext _context;

    public FormDataController(FormsAPIContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> ExecuteStoredProcedure([FromBody] Dictionary<string, object> formData)
    {
        try
        {
            if (formData == null || formData.Count == 0)
            {
                return BadRequest("No se proporcionaron datos para ejecutar el procedimiento almacenado.");
            }

            // Crear una tabla en SQL Server basada en los datos recibidos
            var createTableQuery = "CREATE OR ALTER TABLE newTable (";
            var insertQuery = "INSERT INTO newTable (";

            foreach (var kvp in formData)
            {
                createTableQuery += $"{kvp.Key} NVARCHAR(MAX),";
                insertQuery += kvp.Key + ",";
            }

            createTableQuery = createTableQuery.TrimEnd(',') + ")";
            insertQuery = insertQuery.TrimEnd(',') + ") VALUES (";

            foreach (var kvp in formData)
            {
                insertQuery += $"'{kvp.Value}',"; // Aquí se asume que todos los datos son cadenas (strings)
            }

            insertQuery = insertQuery.TrimEnd(',') + ")";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(createTableQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                using (var command = new SqlCommand(insertQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }

            return Ok("Procedimiento almacenado ejecutado con éxito y tabla creada en SQL Server.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

}
