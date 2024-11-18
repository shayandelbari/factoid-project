public struct Array
{
    public string[] array;
    public int length;
}

class Program
{
    //TODO: Change ALL dynamic data types to static. This may require looping through to find the length then iter-ing based on that value.
    //FIXME: main panel still not done!
    static void Main()
    {
        // -------------TESTING CODES------------------------------------------

        // TESTING: DetermineFactoidType
        // string questionType = DetermineFactoidType("Who was the");
        // Console.WriteLine(questionType);

        // TESTING: GetPerson
        // List<string> answer = GetPerson("Apple Inc. was founded by Steve Jobs and Steve Wozniak in CUPERTINO, CALIFORNIA, on 1976-04-01");
        // foreach (string ans in answer)
        // {
        //     Console.WriteLine(ans);
        // }


        // ------------PANEL CODES---------------------------------------------
        // Console.WriteLine("Welcome to Shayan, Edward, and Brett's factoid answering program");
        int mainMenu;
        bool endProgram = false;
        string text = "";
        int badQuestion = 0;
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
                    UpdateTextFn(ref text);
                    break;
                case 2:
                    //run factoid program
                    AskQuestionFn(ref text);
                    break;
                case 3:
                    //explanation on what types of questions the user can use
                    ExplanationFn(ref badQuestion, ref text);
                    break;
                case 4:
                    //end program
                    Console.WriteLine("Thank you for your patronage, come back anytime!");
                    endProgram = true;
                    break;
                default:
                    Console.WriteLine("You have entered an invalid option. You may input the number 1, 2, 3 or 4");
                    break;
            }
        }
        while (endProgram == false);
    }

    static void UpdateTextFn(ref string text)
    //input reference text & split it to arrays of words within arrays of sentences
    {
        Console.WriteLine("Enter the text you would like to use as the reference. Afterwards, you can ask factoid questions based on that text");
        text = Console.ReadLine();
        // TODO: run split on text x2 (once to split sentences once to split words) - end result should be many arrays in an array
        AskQuestionFn(ref text);
    }

    static void AskQuestionFn(ref string text)
    // Fn to take a question get an answer
    {
        if (text == "")
        //if text is blank, ask for text now
        {
            Console.WriteLine("It appears that there is no reference text to review for an answer, let's input that first");
            UpdateTextFn(ref text);
        }

        Console.WriteLine("Ask a factoid question:");
        string question = Console.ReadLine();
        //TODO:
        // run split on question (or later depending on how our other Fns work)
        // run determineFactoidType
        // run removeStopWord on question
        // run CalculateSimilarityModule
        // print the sentence with the highest similarity as the answer
    }

    static void ExplanationFn(ref int badQuestion, ref string text)
    //explanation Fn
    {
        Console.WriteLine("This program is designed to work with factoid questions. We wanted to share what that means so you can get the best out of the program.");
        Console.WriteLine("A factoid question is a closed-ended question based on one of these question words:");
        Console.WriteLine("- WHO");
        Console.WriteLine("- WHERE");
        Console.WriteLine("- WHEN");
        Console.WriteLine("- HOW MANY");
        Console.WriteLine("- HOW MUCH");
        Console.WriteLine();
        Console.WriteLine("Please ensure you phrase your question so it STARTS with one of the previous question words:");
        badQuestion = 0;
        AskQuestionFn(ref text);
    }

    static string DetermineFactoidType(string question, ref int badQuestion)
    // determining the type of question
    {
        string firstWorld = "";
        bool done = false;
        string answerType;
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
                answerType = "The question you have asked is invalid, please rephrase your question and ask again";
                // TODO: this should be better
                badQuestion++;
                if (badQuestion >= 3)
                {
                    return "explanationFn";
                }
                break;
        }

        return answerType;
    }

    static string RemoveStopWords(string question)
    //remove stop words from the question - this will have less iters as it is the shorter string, and will allow us to simply print the sentence that corresponds to the answer w/o any additional formatting
    {
        int i = 0;
        bool isStopWord = false;
        string[] stopWords = { "and", "is", "are", "the", "a", "was", "in", "on" };
        string questionWithoutStopWords = "";
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

    static string[] GetPerson(string sentence)
    // getting the persons name out of the given sentence
    {
        string[] result = new string[10];
        int size = 0;
        bool found = false;
        string word = "";

        for (int i = 0; i < sentence.Length; i++)
        {
            // TODO: come up with a way to ignore none-name words that fit into these conditions
            if (Char.IsUpper(sentence[i]) && Char.IsLower(sentence[i + 1]))
            {
                found = true;
            }
            else if (sentence[i] == ' ' && Char.IsLower(sentence[i + 1]) && found)
            {
                found = false;
                result[size] = word;
                size++;
                word = "";
            }
            if (found)
            {
                word += sentence[i];
            }
        }
        int endIndex = size;

        return result[0..endIndex];
    }

    static int[] CalculateSimilarity(string question, string[] text) // FIXME: change this
    // giving an array of percentage similarity between the question and the sentences in the same order of sentences
    {
        int similarityCounter = 0;
        int[] percentageSimilarityArray = new int[text.Length];
        int lengthOfQuestions = question.Length;
        string[] questionWords = Split(RemoveStopWords(question));
        List<string[]> sentenceWords = [];

        for (int i = 0; i < text.Length; i++)
        {
            sentenceWords.Add(Split(text[i]));
        }

        // Check if each word of the sentence is in the entire array of strings of the question, then increment counter  
        for (int i = 0; i < sentenceWords.Count; i++)
        {
            for (int j = 0; j < sentenceWords[i].Length; j++)
            {
                for (int u = 0; u < questionWords.Length; u++)
                {
                    if (sentenceWords[i][j] == questionWords[u])
                    {
                        similarityCounter++;
                        break;
                    }
                }
            }
            percentageSimilarityArray[i] = similarityCounter / lengthOfQuestions * 100;
            similarityCounter = 0;
        }

        return percentageSimilarityArray;
    }

    static string[] Split(string text, object? character = null)
    {
        //this part is for converting the object type to an array of characters
        int size = 0;
        if (character is char || character is null) size = 1;
        else if (character is string c) size = c.Length;
        else if (character is char[] c1) size = c1.Length;
        else return [];

        char[] separators = new char[size];
        for (int i = 0; i < size; i++)
        {
            if (character is char || character is null) separators[i] = character is null ? ' ' : (char)character;
            else if (character is string c) separators[i] = c[i];
            else if (character is char[] c1) separators[i] = c1[i];
        }

        // TODO: Implementing the possible problem: Ignoring the `.` in `Apple Inc.`
        Array result;
        result.array = new string[50];
        result.length = 0;
        string currentWord = "";
        bool found = false;

        for (int i = 0; i < text.Length; i++)
        {
            for (int j = 0; j < separators.Length; j++)
            {
                if (text[i] == separators[j])
                {
                    found = true;
                    break;
                }
            }
            if (found || i == text.Length - 1)
            {
                result.array[i] = currentWord;
                result.length++;
                found = false;
                currentWord = "";
            }
            else
            {
                currentWord += text[i];
            }
        }
        int endIndex = result.length - 1;

        return result.array[0..endIndex];
    }

    static string Replace(string text, string target, string replacement)
    // this function will be used for replacing the words like `Inc.` with `Inc`
    {
        string result = "";
        bool found = false;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == target[0])
            {
                for (int j = 0; j < target.Length; j++)
                {
                    if (text[i + j] != target[j])
                    {
                        found = false;
                        break;
                    }
                    found = true;
                }
            }
            if (found)
            {
                result += replacement;
                i += target.Length - 1;
                found = false;
            }
            else
            {
                result += text[i];
            }
        }

        return result;
    }
}