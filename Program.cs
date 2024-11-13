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

    static int[] CalculateSimilarity(string question, string[] text)
    {
        int similarityCounter = 0;
        int[] percentageSimilarityArray = new int[100];
        int lengthOfQuestions = question.Length; // OR LENGTH OF THE TEXT???????
        string[] questionWords = Split(RemoveStopWords(question));
        string[] sentenceWords = Split(text); // TODO:Problem with the data type of text, the parameter in Split only accepts strings.

        // Split(text[1]);
        // Split(text[2]);
        // Check if each word of the sentence is in the entire array of strings of the question, then increment counter  


        for (int i = 0; i < text.Length; i++)
        {
            for (int u = 0; u < lengthOfQuestions; u++)
            {
                if (sentenceWords[i] == sentenceWords[u])
                {
                    similarityCounter++;
                }
            }
        }

        for (int i = 0; i < text.Length; i++)
        {
            percentageSimilarityArray[i] = (similarityCounter / lengthOfQuestions) * 100; // POSSIBLE CORRECTION???????
        }

        return percentageSimilarityArray;
    }

    static string[] Split(string text, char character = ' ')
    {
        string[] words = new string[51];
        string currentWord = "";
        int iterWords = 0;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == character)
            {
                words[iterWords] = currentWord;
                iterWords++;
                currentWord = "";
            }
            else if (i == text.Length - 1)
            {
                words[iterWords] = currentWord;
            }
            else
            {
                currentWord += text[i];
            }
        }

        return words; // We might return empty words if we have a fixed size array
    }
}
