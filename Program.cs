class Program
{
    static void Main()
    {
        // string questionType = DetermineFactoidType("Who was the");
        // Console.WriteLine(questionType);
        List<string> answer = GetPerson("Apple Inc. was founded by Steve Jobs and Steve Wozniak in CUPERTINO, CALIFORNIA, on 1976-04-01");
        foreach (string ans in answer)
        {
            Console.WriteLine(ans);
        }
    }

    // determining the type of question
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
            if (question[i] == ' ')
            {
                done = true;
            }
            i++;
        }

        switch (firstWorld)
        {
            case "Who":
                answerType = "getPerson";
                break;
            case "Where":
                answerType = "getPlace";
                break;
            case "When":
                answerType = "getDateTime";
                break;
            case "How many":
                answerType = "getAmount";
                break;
            case "How much":
                answerType = "getAmount";
                break;
            default:
                answerType = "The question you have asked is invalid, please rephrase your question and ask again.";
                break;
        }

        return answerType;
    }

    //remove stop words from the question
    static string RemoveStopWords(string question)
    {
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
                word = ""; //empty the word for next round
            }

            i++;
        }

        return questionWithoutStopWords;
    }

    static List<string> GetPerson(string sentence)
    {
        List<string> answer = new List<string>();
        bool found = false;
        string word = "";

        for (int i = 0; i < sentence.Length; i++)
        {
            if (Char.IsUpper(sentence[i]) && Char.IsLower(sentence[i + 1]))
            {
                found = true;
            }
            if (sentence[i] == ' ' && Char.IsLower(sentence[i + 1]))
            {
                found = false;
                answer.Add(word);
                word = "";
            }
            if (found)
            {
                word += sentence[i];
            }
        }

        return answer;
    }
}

