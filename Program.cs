class Program
{
    static void Main()
    {
        string tempFolderPath = Path.GetTempPath(); // Replace this with the actual path to your temporary folder

        // Run the cleanup process every 24 hours (adjust the interval as needed)
        TimeSpan cleanupInterval = TimeSpan.FromHours(24);

        // Start a background thread to handle the automatic deletion of temp files
        Thread cleanupThread = new Thread(() =>
        {
            while (true)
            {
                try
                {
                    // Delete files older than a certain threshold (e.g., 1 day)
                    CleanUpOldTempFiles(tempFolderPath, TimeSpan.FromDays(1));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during temp file cleanup: " + ex.Message);
                }

                // Wait for the specified interval before running the cleanup again
                Thread.Sleep(cleanupInterval);
            }
        });

        // Set the thread as a background thread so it doesn't prevent the application from terminating
        cleanupThread.IsBackground = true;

        // Start the thread
        cleanupThread.Start();

        // The rest of your application logic goes here
        // ...

        // Keep the main thread alive to allow the cleanup thread to run in the background
        // This is just an example; in a real application, you may have other logic to keep the app running
        while (true)
        {
            Thread.Sleep(1000);
        }
    }

    static void CleanUpOldTempFiles(string folderPath, TimeSpan maxAge)
    {
        DateTime now = DateTime.Now;
        DirectoryInfo directory = new DirectoryInfo(folderPath);

        // Loop through the files in the directory
        foreach (FileInfo file in directory.GetFiles())
        {
            // Check if the file is older than the specified threshold
            if (now - file.LastWriteTime > maxAge)
            {
                try
                {
                    // Delete the file
                    file.Delete();
                    Console.WriteLine("Deleted: " + file.Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error deleting file " + file.Name + ": " + ex.Message);
                }
            }
        }
    }
}
