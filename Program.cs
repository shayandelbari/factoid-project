class Program
{
    static void Main(string[] arg)
    {
        Console.WriteLine("Hello");
        // Another();
        string questionType = DetermineFactoidType("Who was the");
        Console.WriteLine(questionType);
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
                answerType = "Location";
                break;
            case "When":
                answerType = "Date Time";
                break;
            case "How many":
                answerType = "Number";
                break;
            case "How much":
                answerType = "Number";
                break;
            default:
                answerType = "Invalid Question";
                break;
        }

        return answerType;
    }

    static string removeStopWords(string question)
    { //remove stop words from the question?

        int i = 0;
        bool isStopWord = false;
        string[] stopWords = { "and", "is", "are", "the", "a", "was", "in", "on" };
        string questionWithoutStopWords = " ";
        string word = "";

        while (question[i] != '?')
        {

            if (question[i] != ' ')
            {
                word += question[i];
            }

            else if (question[i] == ' ')
            {
                word = ""; //empty the word for nextround
                for (int w = 0; w < stopWords.Length; w++)
                {
                    if (word == stopWords[w])
                    {
                        isStopWord = true;
                        break;
                    }
                }
                if (!isStopWord)
                {
                    questionWithoutStopWords += word;
                }
            }

            i++;
        }

    }
}