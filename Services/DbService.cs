using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using EduCorePro.Models;

namespace EduCorePro.Services;

public class DbService
{
    private string _dbPath = $"Data Source={AppDomain.CurrentDomain.BaseDirectory}educore.db";

    public DbService()
    {
        // Создаем таблицу при запуске
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Questions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Question TEXT,
                CorrectAnswer TEXT,
                OptionA TEXT,
                OptionB TEXT,
                OptionC TEXT,
                OptionD TEXT
            )";
        command.ExecuteNonQuery();
    }

    public void SaveQuestion(QuizQuestion q)
    {
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Questions (Question, CorrectAnswer, OptionA, OptionB, OptionC, OptionD) VALUES ($q, $ca, $a, $b, $c, $d)";
        command.Parameters.AddWithValue("$q", q.Question);
        command.Parameters.AddWithValue("$ca", q.CorrectAnswer);
        command.Parameters.AddWithValue("$a", q.OptionA);
        command.Parameters.AddWithValue("$b", q.OptionB);
        command.Parameters.AddWithValue("$c", q.OptionC);
        command.Parameters.AddWithValue("$d", q.OptionD);
        command.ExecuteNonQuery();
    }

    public List<QuizQuestion> GetAllQuestions()
    {
        var list = new List<QuizQuestion>();
        using var connection = new SqliteConnection(_dbPath);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Questions";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new QuizQuestion {
                Id = reader.GetInt32(0),
                Question = reader.GetString(1),
                CorrectAnswer = reader.GetString(2),
                OptionA = reader.GetString(3),
                OptionB = reader.GetString(4),
                OptionC = reader.GetString(5),
                OptionD = reader.GetString(6)
            });
        }
        return list;
    }
}