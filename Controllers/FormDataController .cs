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

            // Obtener el nombre de la tabla de formData
            string tableName = "newTable";

            // Verificar si la tabla ya existe
            if (!TableExists(tableName))
            {
                // Si no existe, crea la tabla
                if (!CreateTable(formData, tableName))
                {
                    return BadRequest("Error al crear la tabla.");
                }
            }

            // Insertar datos en la tabla
            if (!InsertData(formData, tableName))
            {
                return BadRequest("Error al insertar datos en la tabla.");
            }

            return Ok("Procedimiento almacenado ejecutado con éxito y tabla creada en SQL Server.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    private bool TableExists(string tableName)
    {
        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            connection.Open();

            using (var command = new SqlCommand($"SELECT 1 FROM sys.tables WHERE name = '{tableName}'", connection))
            {
                return command.ExecuteScalar() != null;
            }
        }
    }

    private bool CreateTable(Dictionary<string, object> formData, string tableName)
    {
        try
        {
            var createTableQuery = $"CREATE TABLE {tableName} (";

            foreach (var kvp in formData)
            {
                createTableQuery += $"{kvp.Key} NVARCHAR(MAX),";
            }

            createTableQuery = createTableQuery.TrimEnd(',') + ")";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                connection.Open();

                using (var command = new SqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool InsertData(Dictionary<string, object> formData, string tableName)
    {
        try
        {
            var insertQuery = $"INSERT INTO {tableName} (";

            foreach (var kvp in formData)
            {
                insertQuery += kvp.Key + ",";
            }

            insertQuery = insertQuery.TrimEnd(',') + ") VALUES (";

            foreach (var kvp in formData)
            {
                insertQuery += $"'{kvp.Value}',";
            }

            insertQuery = insertQuery.TrimEnd(',') + ")";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                connection.Open();

                using (var command = new SqlCommand(insertQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }


}
