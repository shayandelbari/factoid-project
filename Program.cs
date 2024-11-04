class Program
{
    static void Main(string[] arg)
    {
        Console.WriteLine("Hello");
        // Another();
        string firstWorld = DetermineFactoidType("How many was the");
        Console.WriteLine(firstWorld);
    }

    static void Another()
    {
        Console.WriteLine("Another");
    }

    static string DetermineFactoidType(string question)
    {
        string firstWorld = "";
        string answerType = "";
        bool done = false;
        int i = 0;

        while (!done)
        {
            if (question[i] != ' ' && firstWorld != "How")
            {
                firstWorld += question[i];
            }
            else if (question[i] == ' ' && firstWorld == "How")
            {
                firstWorld += question[i];
                firstWorld += question[i + 1];
                i++;
            }
            else if (question[i] == ' ')
            {
                done = true;
            }
            i++;
        }

        switch (firstWorld)
        {
            case "Who":
                answerType = "Person";
                break;
            case "Where":

            case _:
                answerType = "Invalid Question";
                break;
        }

        return answerType;
    }
}