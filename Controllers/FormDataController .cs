﻿namespace FormsAPI.Controllers;
using FormsAPI.Data;
using FormsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Principal;
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
            string tableName = "newTable" + formData["idForm"];

            // Verifica si la tabla ya existe
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

    //valida la existencia de la tabla
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

    //en caso que no extista, crea la nueva tabla
    private bool CreateTable(Dictionary<string, object> formData, string tableName)
    {
        try
        {
            var createTableQuery = $"CREATE TABLE {tableName} (";

            foreach (var kvp in formData)
            {
                if(kvp.Key == "idForm")
                {
                    createTableQuery += $"{kvp.Key} int,";
                }
                else { 
                    createTableQuery += $"{kvp.Key} NVARCHAR(MAX),";
                }
            }
            createTableQuery = createTableQuery + "FOREIGN KEY(idForm) REFERENCES form(id)";


            createTableQuery = createTableQuery.TrimEnd(',') + ")";
            System.Console.WriteLine(createTableQuery);

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

    //recorre el formdata para insertar los datos
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFormData(int id)
    {
        try
        {
            // Obtener el nombre de la tabla de formData
            string tableName = "newTable" + id;

            // Verificar si la tabla existe
            if (!TableExists(tableName))
            {
                return NotFound("La tabla no existe.");
            }

            // Consultar y devolver los datos de la tabla
            var selectQuery = $"SELECT * FROM {tableName}";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                connection.Open();

                using (var command = new SqlCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var result = new List<Dictionary<string, object>>();

                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetName(i), reader[i]);
                            }

                            result.Add(row);
                        }

                        return Ok(result);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }



}
