class Program
{
    public static readonly string[] DATA = {
        "Apple Inc. was founded by Steve Jobs and Steve Wozniak in CUPERTINO, CALIFORNIA, on 1976-04-01. The company initially raised $1,000 to develop their first product. In 2023, Apple reported a 15% revenue increase, reaching a total of $387.53 billion.",
        "The history of programming languages spans from documentation of early mechanical computers to modern tools for software development. Early programming languages were highly specialized, relying on mathematical notation and similarly obscure syntax. Throughout the 20th century, research in compiler theory led to the creation of high-level programming languages, which use a more accessible syntax to communicate instructions. The first high-level programming language was created by Konrad Zuse in 1943. The first highlevel language to have an associated compiler was created by Corrado Böhm in 1951. Konrad Zuse was born on 1910/06/22, in GERMANY, and was a notable civil engineer, pioneering computer scientist, inventor, and businessman."
    };
    // add update Txt fn before mainMenu
    // default state is to ask question and get answer UNLESS guide, exit, updateText
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
            else if (question == "Explain" || question == "explain" || question == "Explanation" || question == "explanation")
            {
                ExplanationFn();
                badQuestion = 0;
                //Reset the bad question counter every time they read the explanation
            }
            else if (question == "Update Text" || question == "update text" || question == "Update" || question == "update")
            {
                textArray = UpdateTextFn();
            }
            else
            {
                string answerType = DetermineFactoidType(question);

                if (answerType == "bad")
                {
                    Console.WriteLine("The question you have asked is invalid, please rephrase your question and ask again");
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

                    if (answers is null) Console.WriteLine("Your question didn't have any answers could you rephrase it?");
                    else
                    {
                        Console.WriteLine();
                        for (int i = 0; i >= answers.Length; i++)
                        {
                            Console.WriteLine(answers[i]);
                        }
                    }
                    Console.Write("Continue... ");
                    Console.ReadKey();
                }
            }

        } while (endProgram == false);
        Console.Clear();
        Console.WriteLine("Thank you for your patronage, come back anytime!");
        int milliseconds = 3000;
        Thread.Sleep(milliseconds);
        Console.Clear();
    }

    static void OtherKeywords()
    {
        Console.Clear();
        Console.WriteLine("");
        Console.WriteLine("Update ---- changes the reference text");
        Console.WriteLine("Explain --- provides a short explanation on what type of questions you can ask");
        Console.WriteLine("Exit ------ ends the program");
        Console.WriteLine("");
    }

    static void LandingPage()
    {
        Console.Clear();
        Console.WriteLine("Shayan Delbari, Edward Angeles, and Brett Trudel are proud to present:");
        Console.WriteLine("🅣🅗🅔 🅕🅐🅒🅣🅞🅘🅓 🅐🅝🅢🅦🅔🅡🅘🅝🅖 🅟🅡🅞🅖🅡🅐🅜");
        Console.WriteLine("");
        Console.WriteLine("Factoid questions start with who, when, where, how many, or how much.");
        Console.WriteLine("");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    static string[] UpdateTextFn()
    //input reference text & split it to arrays of words within arrays of sentences
    {
        Console.Clear();
        Console.WriteLine("🅤🅟🅓🅐🅣🅔 🅣🅔🅧🅣");
        Console.WriteLine("Enter the text you would like to use as the reference. Afterwards, you can ask factoid questions based on that text");

        // TODO: null text before shipping :|
        string? text;
        text = Console.ReadLine();
        while (text is null || text == "")
        {
            Console.WriteLine("It seems you haven't entered any text. Please try that again.");
            text = Console.ReadLine();
        }

        text = Replace(text, "Inc.", "inc"); // FIXME: come up with a better way
        string[] textArray = Split(text, "?!.");

        for (int i = 0; i < textArray.Length; i++)
        {
            textArray[i] = ToLower(Trim(textArray[i]));
        }
        return textArray;
    }

    static void ExplanationFn()
    //explanation Fn
    {
        string guide = @"This program is designed to work with factoid questions. We wanted to share what that means so you can get the best out of the program.
A factoid question is a closed-ended question based on one of these question words:
- WHO
- WHERE
- WHEN
- HOW MANY
- HOW MUCH

Please ensure you phrase your question so it STARTS with one of the previous question words:";

        Console.Clear();
        Console.WriteLine("🅔🅧🅟🅛🅐🅝🅐🅣🅘🅞🅝");
        Console.WriteLine(guide);
    }

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

    static string[]? GetPerson(string sentence)
    // getting the persons name out of the given sentence
    {
        string[] result = new string[10];
        int size = 0;
        bool found = false;
        string word = "";

        for (int i = 0; i < sentence.Length; i++)
        {
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
            if (!found && Char.IsUpper(sentence[i]) && Char.IsUpper(sentence[i + 1]))
            {
                found = true;
            }
            if (((sentence[i] == ' ' && Char.IsLower(sentence[i + 1])) || (sentence[i] == ',')) && found)
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
            if (i < sentence.Length - 10 && Char.IsNumber(sentence[i]) && (sentence[i + 4] == '-' || sentence[i + 4] == '/') && (sentence[i + 7] == '-' || sentence[i + 7] == '/'))
            {
                endDateIndex = i + 10;
                output[size] = sentence[i..endDateIndex];
                size++;
                i += 10;
            }
            else if (i < sentence.Length - 4 && Char.IsNumber(sentence[i]) && (sentence[i + 4] == ' ' || sentence[i + 4] == ','))
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
                    if (wordText[j] == questionWords[u])
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
        if (text == "") return [""];
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
                break;
            }
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

// TODO - Replace Brett's bad characters as titles