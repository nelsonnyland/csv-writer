internal class Program
{
    /// <summary>
    /// This temporarily targets a specific type of CSV file for transitional purposes.
    /// </summary>
    /// <param name="args"></param>
    private static void Main(string[] args)
    {
        // read in csv file
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string fileName = "Export.csv";
        var lines = new List<string>();
        using (var reader = new StreamReader($"{desktopPath}\\{fileName}"))
        {
            // delete first 3 rows
            var line = reader.ReadLine(); // row 1
            for (int i = 0; i < 3; i++)
            {
                line = reader.ReadLine(); // should now have header row line
            }
            
            string[] selCols = line.Split(',');
            // change "Memo"[3] column title to "Payee"
            selCols[3] = "Payee";
            // change "Amount Debit"[4] title to "Amount"
            selCols[4] = "Amount";
            var values = new List<string>();
            foreach (var col in selCols) values.Add(col);
            // add "Tags" column
            values.Add("Tags");
            line = string.Join(",", values);

            // delete columns: column[0], "Description"[2], "Amount Credit"[5],
            // "Balance"[6], "Check Number"[7], and "Fees"[8] columns
            var deleteCols = new int[] { 0, 2, 5, 6, 7, 8 };
            bool headerRow = true; // don't swap header values
            while (line != null)
            {
                values.Clear();
                selCols = line.Split(',');
                // move any values in "Amount Credit" column to Amount column
                if (!headerRow && !string.IsNullOrEmpty(selCols[5])) selCols[4] = selCols[5];
                if (!headerRow && !string.IsNullOrEmpty(selCols[7])) selCols[3] = $"Check #: {selCols[7]}";
                headerRow = false;
                for (int i = 0; i < selCols.Length; i++)
                {
                    if (!deleteCols.Contains(i))
                    {
                        values.Add(selCols[i]);
                    }
                }
                var newLine = string.Join(",", values);
                lines.Add(newLine);
                line = reader.ReadLine();
            }
        }

        // write back to csv file
        using (var writer = new StreamWriter($"{desktopPath}\\Import.csv", false))
        {
            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
        }
    }
}

