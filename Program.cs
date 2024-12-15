class Program
{
    public static readonly bool DEVELOPMENT = false;
    static void Main()
    {
        LandingPage();
        string? question;
        string[] textArray = UpdateTextFn();
        int badQuestion = 0;
        bool endProgram = false;
        //default loop (ie the main program itself, this will run on loop until )
        do
        {
            OtherKeywords();
            Console.WriteLine("You can ask a factoid question, or you can enter one of the following keywords:");
            question = Console.ReadLine();
            while (question is null || question == "")
            {
                Console.WriteLine("Please ask a question, or enter one of the above keywords:");
                question = Console.ReadLine();
            }

            if (question == "Exit" || question == "q" || question == "exit" || question == "quit" || question == "Quit")
            {
                endProgram = true;
            }
            else if (question == "Explain" || question == "explain" || question == "Explanation" || question == "explanation" || question == "e")
            {
                ExplanationFn();
                badQuestion = 0;
                //Reset the bad question counter every time they read the explanation
            }
            else if (question == "Update Text" || question == "update text" || question == "Update" || question == "update" || question == "u")
            {
                textArray = UpdateTextFn();
            }
            else
            {
                string answerType = DetermineFactoidType(question);

                if (answerType == "bad")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The question you have asked is invalid, please rephrase your question and ask again");
                    Console.ResetColor();
                    Console.Write("\nContinue... ");
                    if (!DEVELOPMENT) Console.ReadKey(false);
                    else Console.ReadLine();
                    badQuestion++;
                    if (badQuestion >= 3)
                    {
                        ExplanationFn();
                        badQuestion = 0;
                    }
                }
                else
                {
                    question = RemoveStopWords(question);
                    double[] percentageSimilar = CalculateSimilarity(question, textArray);
                    string[]? answers = GetAnswer(answerType, textArray, percentageSimilar);

                    if (answers is null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Your question didn't have any answers could you rephrase it?");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine();
                        for (int i = 0; i < answers.Length; i++)
                        {
                            Console.WriteLine(answers[i]);
                        }
                    }
                    Console.Write("Continue... ");
                    if (!DEVELOPMENT) Console.ReadKey(false);
                    else Console.ReadLine();
                }
            }

        } while (endProgram == false);
        if (!DEVELOPMENT) Console.Clear();
        Console.WriteLine("Thank you for your patronage, come back anytime!");
        int milliseconds = 3000;
        Thread.Sleep(milliseconds);
        if (!DEVELOPMENT) Console.Clear();
    }

    static void LandingPage()
    {
        string msg = @"
███████╗ █████╗  ██████╗████████╗ ██████╗ ██╗██████╗ 
██╔════╝██╔══██╗██╔════╝╚══██╔══╝██╔═══██╗██║██╔══██╗
█████╗  ███████║██║        ██║   ██║   ██║██║██║  ██║
██╔══╝  ██╔══██║██║        ██║   ██║   ██║██║██║  ██║
██║     ██║  ██║╚██████╗   ██║   ╚██████╔╝██║██████╔╝
╚═╝     ╚═╝  ╚═╝ ╚═════╝   ╚═╝    ╚═════╝ ╚═╝╚═════╝ 
                                                     
               ██████╗     ██╗ █████╗ 
              ██╔═══██╗   ██╔╝██╔══██╗ 
              ██║   ██║  ██╔╝ ███████║ 
              ██║▄▄ ██║ ██╔╝  ██╔══██║ 
              ╚██████╔╝██╔╝   ██║  ██║ 
               ╚══▀▀═╝ ╚═╝    ╚═╝  ╚═╝

";
        if (!DEVELOPMENT) Console.Clear();
        Console.WriteLine("Shayan Delbari, Edward Angeles, and Brett Trudel are proud to present:");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(msg);
        Console.ResetColor();
        Console.WriteLine("");
        Console.WriteLine("Factoid questions start with who, when, where, how many, or how much.");
        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");
        if (!DEVELOPMENT) Console.ReadKey(false);
        else Console.ReadLine();
    }

    static void OtherKeywords()
    {
        string msg = @"
  ___                  _   _                 
 / _ \ _   _  ___  ___| |_(_) ___  _ __  ___ 
| | | | | | |/ _ \/ __| __| |/ _ \| '_ \/ __|
| |_| | |_| |  __/\__ \ |_| | (_) | | | \__ \
 \__\_\\__,_|\___||___/\__|_|\___/|_| |_|___/

";
        if (!DEVELOPMENT) Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(msg);
        Console.ResetColor();
        Console.WriteLine("Update ---- changes the reference text");
        Console.WriteLine("Explain --- provides a short explanation on what type of questions you can ask");
        Console.WriteLine("Exit ------ ends the program");
        Console.WriteLine("");
    }

    static string[] UpdateTextFn()
    //input reference text & split it to arrays of words within arrays of sentences
    {
        string msg = @"
 _   _           _       _         _____         _   
| | | |_ __   __| | __ _| |_ ___  |_   _|____  _| |_ 
| | | | '_ \ / _` |/ _` | __/ _ \   | |/ _ \ \/ / __|
| |_| | |_) | (_| | (_| | ||  __/   | |  __/>  <| |_ 
 \___/| .__/ \__,_|\__,_|\__\___|   |_|\___/_/\_\\__|
      |_|                                            
";
        if (!DEVELOPMENT) Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(msg);
        Console.ResetColor();
        Console.WriteLine("Enter the text you would like to use as the reference. Afterwards, you can ask factoid questions based on that text");

        string? text;
        text = Console.ReadLine();
        while (text is null || !CheckText(text))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nIt seems you haven't entered any text or it doesn't fit the guide lines. Please try another text again.");
            Console.ResetColor();
            text = Console.ReadLine();
        }

        text = Replace(text, "Inc.", "inc");
        string[] textArray = Split(text, "?!.");

        for (int i = 0; i < textArray.Length; i++)
        {
            textArray[i] = ToLower(Trim(textArray[i]));
        }
        return textArray;
    }

    static bool CheckText(string text)
    {
        if (text == "") return false;

        bool noSpace = true;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == ' ')
            {
                noSpace = false;
                break;
            }
        }
        if (noSpace) return false;

        return true;
    }

    static void ExplanationFn()
    //explanation Fn
    {
        string msg = @"
 _____            _                   _   _             
| ____|_  ___ __ | | __ _ _ __   __ _| |_(_) ___  _ __  
|  _| \ \/ / '_ \| |/ _` | '_ \ / _` | __| |/ _ \| '_ \ 
| |___ >  <| |_) | | (_| | | | | (_| | |_| | (_) | | | |
|_____/_/\_\ .__/|_|\__,_|_| |_|\__,_|\__|_|\___/|_| |_|
           |_|                                          
";
        string guide = @"This program is designed to work with factoid questions. We wanted to share what that means so you can get the best out of the program.
A factoid question is a closed-ended question based on one of these question words:
- WHO
- WHERE
- WHEN
- HOW MANY
- HOW MUCH

Please ensure you phrase your question so it STARTS with one of the previous question words:";

        if (!DEVELOPMENT) Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(msg);
        Console.ResetColor();
        Console.WriteLine(guide);
        if (!DEVELOPMENT) Console.ReadKey(false);
        else Console.ReadLine();
    }

    // static void WriteLine(string msg, )

    static string DetermineFactoidType(string question)
    // determining the type of question
    {
        string firstWorld = "";
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
            if (question[i] == ' ' || i == question.Length - 1)
            {
                done = true;
            }
            i++;
        }

        string answerType = firstWorld switch
        {
            "Who" => "getPerson",
            "Where" => "getPlace",
            "When" => "getDateTime",
            "How many" => "getAmount",
            "How much" => "getAmount",
            _ => "bad",
        };
        return answerType;
    }

    static string RemoveStopWords(string question)
    //remove stop words from the question - this will have less iters as it is the shorter string, and will allow us to simply print the sentence that corresponds to the answer w/o any additional formatting
    {
        string[] stopWords = { "and", "is", "are", "the", "a", "was", "in", "on" };
        string questionWithoutStopWords = "";
        string word = "";

        for (int i = 0; i < question.Length; i++)
        {
            if (question[i] != ' ' && question[i] != '?')
            {
                word += question[i];
            }
            else
            {
                bool isStopWord = false;
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
                    questionWithoutStopWords += word + " ";
                }
                word = ""; //empty the word for next round
            }
        }

        return Trim(questionWithoutStopWords);
    }

    static string[]? GetPerson(string sentence)
    // getting the persons name out of the given sentence
    {
        string[] result = new string[10];
        int size = 0;
        bool found = false;
        string word = "";

        for (int i = 0; i < sentence.Length; i++)
        {
            if (i < sentence.Length - 1 && Char.IsUpper(sentence[i]) && Char.IsLower(sentence[i + 1]))
            {
                found = true;
            }
            else if (i < sentence.Length - 1 && sentence[i] == ' ' && Char.IsLower(sentence[i + 1]) && found)
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

        if (size == 0)
        {
            return null;
        }
        return result[0..size];
    }

    static string[]? GetLocation(string sentence)
    {
        string[] result = new string[10];
        int size = 0;
        bool found = false;
        string word = "";

        for (int i = 0; i < sentence.Length; i++)
        {
            if (i < sentence.Length - 1 && !found && Char.IsUpper(sentence[i]) && Char.IsUpper(sentence[i + 1]))
            {
                found = true;
            }
            if (i < sentence.Length - 1 && ((sentence[i] == ' ' && Char.IsLower(sentence[i + 1])) || (sentence[i] == ',')) && found)
            {
                found = false;
                result[size] = word;
                size++;
                word = "";
            }
            else if (found)
            {
                word += sentence[i];
            }
        }

        if (size == 0)
        {
            return null;
        }
        return result[0..size];
    }

    static string[]? GetDateTime(string sentence)
    {
        string[] output = new string[10];
        int size = 0;
        int endDateIndex = 0;

        for (int i = 0; i < sentence.Length; i++)
        {
            if (i < sentence.Length - 9 && Char.IsNumber(sentence[i]) && (sentence[i + 4] == '-' || sentence[i + 4] == '/') && (sentence[i + 7] == '-' || sentence[i + 7] == '/'))
            {
                endDateIndex = i + 10;
                output[size] = sentence[i..endDateIndex];
                size++;
                i += 10;
            }
            else if (i < sentence.Length - 3 && Char.IsNumber(sentence[i]) && (sentence[i + 4] == ' ' || sentence[i + 4] == ','))
            {
                bool allNumbers = true;
                for (int j = 0; j < 4; j++)
                {
                    if (!Char.IsNumber(sentence[i + j]))
                    {
                        allNumbers = false;
                        break;
                    }
                }
                if (allNumbers)
                {
                    endDateIndex = i + 4;
                    output[size] = sentence[i..endDateIndex];
                    size++;
                    i += 4;
                }
            }
        }
        if (size == 0)
        {
            return null;
        }
        return output[0..size];
    }

    static string[]? GetAmount(string sentence)
    {
        string[] result = new string[10];
        int size = 0;
        string[] words = Split(sentence);
        bool found = false;
        char[] chars = { '%', '$' };

        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 1 && Char.IsNumber(words[i][0]))
            {
                for (int j = 0; j < chars.Length; j++)
                {
                    if (words[i][^1] == chars[j])
                    {
                        found = true;
                        break;
                    }
                }
            }
            else if (words[i].Length > 1 && Char.IsNumber(words[i][1]))
            {
                for (int j = 0; j < chars.Length; j++)
                {
                    if (words[i][0] == chars[j])
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (found)
            {
                result[size] += words[i];
                size++;
                found = false;
            }
        }

        if (size == 0)
        {
            return null;
        }
        return result[0..size];
    }

    static double[] CalculateSimilarity(string question, string[] text)
    // giving an array of percentage similarity between the question and the sentences in the same order of sentences
    {
        double[] result = new double[text.Length];
        string[] questionWords = Split(question);

        for (int i = 0; i < text.Length; i++)
        {
            double similarityCounter = 0;
            // double ratio = 0;
            string[] wordText = Split(text[i]);
            for (int j = 0; j < wordText.Length; j++)
            {
                for (int u = 0; u < questionWords.Length; u++)
                {
                    if (wordText[j].ToLower() == questionWords[u].ToLower()) // FIXME: don't use these
                    {
                        similarityCounter++;
                        break;
                    }
                }
            }
            result[i] = similarityCounter / questionWords.Length * 100;
        }

        return result;
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

        string[] array = new string[50];
        int length = 0;
        string currentWord = "";
        bool found = false;

        for (int i = 0; i < text.Length; i++)
        {
            for (int j = 0; j < separators.Length; j++)
            {
                if (separators[j] == '.' && i < text.Length - 2 && i > 1) if (Char.IsNumber(text[i - 1]) && Char.IsNumber(text[i + 1])) continue;
                if (text[i] == separators[j])
                {
                    found = true;
                    break;
                }
            }
            if (found || i == text.Length - 1)
            {
                array[length] = currentWord;
                length++;
                found = false;
                currentWord = "";
            }
            else
            {
                currentWord += text[i];
            }
        }

        return array[0..length];
    }

    static string Replace(string text, string target, string replacement)
    // this function will be used for replacing the words like `Inc.` with `inc`
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

    static string ToLower(string text)
    {
        if (text.Length == 0) return text;
        bool found = false;
        int i;
        char[] listUpper = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                            'U', 'V', 'W', 'X', 'Y', 'Z' };

        char[] listLower = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
                            'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
                            'u', 'v', 'w', 'x', 'y', 'z' };

        if (Char.IsUpper(Split(text)[1][0])) return text;

        for (i = 0; i < listUpper.Length; i++)
        {
            if (text[0] == listUpper[i])
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            return text;
        }

        return listLower[i] + text[1..];
    }

    static string Trim(string sentence)
    {
        int start = 0;
        int end = sentence.Length;

        for (int i = 0; i < sentence.Length; i++)
        {
            if (sentence[i] != ' ')
            {
                start += i;
                break;
            }
        }

        int u = 0;
        for (int j = sentence.Length - 1; j >= 0; j--)
        {
            if (sentence[j] != ' ')
            {
                end -= u;
                break;
            }
            u++;
        }

        return sentence[start..end];
    }

    static string[]? GetAnswer(string questionType, string[] text, double[] similarity)
    {
        string[]? result = null;

        int maxIndex = HighestIndex(similarity);
        int sentenceTimes = 3;

        for (int i = 0; i < sentenceTimes; i++)
        {
            if (questionType == "getPerson")
            {
                result = GetPerson(text[maxIndex]);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    similarity[maxIndex] = 0;
                    maxIndex = HighestIndex(similarity);
                    continue;
                }
            }
            else if (questionType == "getPlace")
            {
                result = GetLocation(text[maxIndex]);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    similarity[maxIndex] = 0;
                    maxIndex = HighestIndex(similarity);
                    continue;
                }
            }
            else if (questionType == "getDateTime")
            {
                result = GetDateTime(text[maxIndex]);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    similarity[maxIndex] = 0;
                    maxIndex = HighestIndex(similarity);
                    continue;
                }
            }
            else if (questionType == "getAmount")
            {
                result = GetAmount(text[maxIndex]);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    similarity[maxIndex] = 0;
                    maxIndex = HighestIndex(similarity);
                    continue;
                }
            }

        }

        return result;
    }

    static int HighestIndex(double[] array)
    {
        int maxIndex = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] > array[maxIndex])
            {
                maxIndex = i;
            }
        }

        return maxIndex;
    }
}
