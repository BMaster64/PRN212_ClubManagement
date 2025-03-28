using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;
using PRN212_Project.Models;

public static class ExcelExportHelper
{
    public static void ExportMembersToExcel(IEnumerable<User> members, string filePath)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Club Members");

            // Add headers
            worksheet.Cell(1, 1).Value = "Student ID";
            worksheet.Cell(1, 2).Value = "Full Name";
            worksheet.Cell(1, 3).Value = "Username";
            worksheet.Cell(1, 4).Value = "Role";
            worksheet.Cell(1, 5).Value = "Date Joined";
            worksheet.Cell(1, 6).Value = "Status";

            // Style the header row
            var headerRange = worksheet.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Add data rows
            int row = 2;
            foreach (var member in members)
            {
                worksheet.Cell(row, 1).Value = member.StudentId;
                worksheet.Cell(row, 2).Value = member.FullName;
                worksheet.Cell(row, 3).Value = member.Username;
                worksheet.Cell(row, 4).Value = GetRoleName(member.RoleId);
                worksheet.Cell(row, 5).Value = member.CreatedAt?.ToString("MM/dd/yyyy");
                worksheet.Cell(row, 6).Value = member.Status ? "Active" : "Inactive";
                row++;
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            // Save the workbook
            workbook.SaveAs(filePath);
        }
    }

    public static void ExportClubsToExcel(IEnumerable<Club> clubs, string filePath)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Clubs");

            // Add headers
            worksheet.Cell(1, 1).Value = "Club ID";
            worksheet.Cell(1, 2).Value = "Club Name";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "Number of Members";

            // Style the header row
            var headerRange = worksheet.Range(1, 1, 1, 4);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Add data rows
            int row = 2;
            foreach (var club in clubs)
            {
                worksheet.Cell(row, 1).Value = club.ClubId;
                worksheet.Cell(row, 2).Value = club.ClubName;
                worksheet.Cell(row, 3).Value = club.Description ?? "N/A";
                worksheet.Cell(row, 4).Value = club.Users.Count;
                row++;
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            // Save the workbook
            workbook.SaveAs(filePath);
        }
    }

    private static string GetRoleName(int roleId)
    {
        return roleId switch
        {
            1 => "Chủ nhiệm",
            2 => "Phó chủ nhiệm",
            3 => "Trưởng ban",
            4 => "Thành viên",
            _ => "Unknown"
        };
    }
}