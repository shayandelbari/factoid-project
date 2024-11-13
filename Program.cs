class Program
{
    int mainMenu;
    bool endProgram = false;
    string text;

    //Console.WriteLine("Welcome to Shayan, Edward, and Brett's factoid answering program");

    do
    {
    Console.WriteLine("Main Menu");
    Console.WriteLine("Please enter the number corresponding with the option you wish to choose:");
    Console.WriteLine("1. Update Reference Text");
    Console.WriteLine("2. Ask a Question");
    Console.WriteLine("3. Find out what type of questions I can ask");
    Console.WriteLine("4. Exit Program");

    mainMenu = Convert.ToInt32(Console.ReadLine());

    switch (mainMenu)
    {
        case 1:
            //update reference text
            Console.WriteLine("Enter the text you would like to use as the reference. Afterwards, you can ask facoid questions based on that text");
            text = Console.ReadLine();
            //goto will allow the user to ask a quesiton right away without going back to the menu, if we are not authorized to use this in the class, the user will need to take the extra step through the main menu.
            goto firstQuestion;
            //break; is redundant here

        case 2:
        //run factoid program
        firstQuestion:
            //firstQuestion label in the destination of the goto statement

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

//this is the end of the main question asking module
break;
case 3:
    //explination on what types of questions the user can use
    Console.WriteLine("This is a placeholder for an explination.");
    goto firstQuestion;
    //user can ask their next question right away
    //break; is redundant here unless we cannot use goto

case 4:
    //end program
    Console.WriteLine("Thank you for your patronage, come back anytime!");
    endProgram = true;
    break;

default:
    Console.WriteLine("You have entered an invalid option. You may input the number 1, 2, or 3");
    break;
    }
}
while (endProgram == false);
}