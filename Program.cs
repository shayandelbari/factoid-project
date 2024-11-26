class Program
{

    // TODO: add landing page
    // add update Txt fn before mainMenu
    // default state is to ask question and get answer UNLESS guide, exit, updateText

    //ASK QUESTION (default loop):
    // get input -> get factoid type (handling bad question here) -> split to sentences -> remove stop words -> get answer

    // ELSES: exit, update, guide

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


        string question;
        string text = "";
        int badQuestion = 0;
        bool endProgram = false;
        //default loop
        do
        {
            Console.WriteLine("ASK QUESTION:");
            question = Console.ReadLine();

            if (question == "Exit")
            {
                endProgram = true;
            }
            else if (question == "Explain")
            {
                ExplanationFn();
                badQuestion = 0;
            }
            else if (question == "Update Text")
            {
                UpdateTextFn(ref text);
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
                // this is where we actually answer the question

                // 
                //replace w Shayan's getAnswerFN

                {

                    text = Replace(text, "Inc.", "inc");
                    string[] textAsSentences = Split(text, "?!.");
                    int[] percentageSimilar = CalculateSimilarity(question, textAsSentences);

                    //TODO: replace w Shayan's getAnswerFN
                    // IF GetAnswer == null, ask to reask question

                }
            }

        } while (endProgram == false);
    }

    static void UpdateTextFn(ref string text)
    //input reference text & split it to arrays of words within arrays of sentences
    {
        Console.Clear();
        Console.WriteLine("Enter the text you would like to use as the reference. Afterwards, you can ask factoid questions based on that text");
        text = Console.ReadLine();
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
            if (question[i] == ' ')
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

    static string[] GetPerson(string sentence)
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
            else if ((sentence[i] == ' ' || sentence[i] == '.') && Char.IsLower(sentence[i + 1]) && found)
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
            if (found)
            {
                word += sentence[i];
            }
            else if ((sentence[i] == ' ' || sentence[i] == '.') && Char.IsLower(sentence[i + 1]) && found)
            {
                found = false;
                result[size] = word;
                size++;
                word = "";
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
            if (Char.IsNumber(sentence[i]) && sentence[i + 4] == '-' && sentence[i + 7] == '-')
            {
                endDateIndex = i + 10;
                output[size] = sentence[i..endDateIndex];
                size++;
                i += 10;
            }
            else if (Char.IsNumber(sentence[i]) && (sentence[i + 3] == ' ' || sentence[i + 4] == '.'))
            {
                endDateIndex = i + 4;
                output[size] = sentence[i..endDateIndex];
                size++;
                i += 4;
            }
        }
        if (size == 0)
        {
            return null;
        }
        return output[0..size];
    }

    // TODO - if sentence[i +4] or sentence[i+7] > is outside the boundries of the array, we will run into issues. This will be a problem in GetAmount too as I borrowed his logic to make sure the number was not a DateTime

    static string[]? GetAmount(string sentence)
    // TODO (review) - My thinking is, if the hasn't found DateTime, anything that is a number will be an amount, please lmk if the logic is bad, I'll fix this. 
    {
        string[] output = new string[10];
        int size = 0;
        bool found = false;
        int start = 0;
        int end = 0;
        int i = 0;

        // find if there is a number
        do
        {
            if (found == false
                    && Char.IsNumber(sentence[i]) == true)
            {
                found = true;
                start = i;
                // log the start location
            }
            // confirm that it is not a DateTime, else make found = false
            if (found == true && Char.IsNumber(sentence[i])
                    && sentence[i + 4] == '-' && sentence[i + 7] == '-')
            {
                found = false;
            }
            i++;
        } while (i < sentence.Length && !found);
        // end loop if a number is found || get to the end of the sentence. 

        // find the ' ' before the word with the number, set start = first char of word with number
        // cant check the number before start if start == 0;
        if (found == true
                && sentence[start - 1] != ' '
                || start == 0)
        {
            do
            {
                start--;
            } while (sentence[start - 1] != ' ');

        }
        end = start;

        // make end == the ' ' after the number (or punctuation) ** when '.' is after a number, if there is another # immediatly after, continue
        if (found == true
                    && sentence[end] != ' '
                    || (sentence[end] != '.' && !char.IsNumber(sentence[end + 1]))
                    || sentence[end] != '!'
                    || sentence[end] != '?')
        {
            while (sentence[end] != ' '
                    && (sentence[end] != '.' && !char.IsNumber(sentence[end + 1]))
                    && sentence[end] != '!'
                    && sentence[end] != '?')
            {
                end++;
            }

        }

        //even if the number is one digit long, end will be the digit after it, therefore if end - start == 0, or the if statements were not entered, Fn will return NULL
        size = end - start;

        if (size == 0)
        {
            return null;
        }
        end--;
        return output[start..end];

        // at end if no result is found (size == 0), return null
        // limitation, what if there are more than 2 words with numbers in the same sentence
    }

    // limitation, the program cannot find the difference between a year and a four digit number. if both exist in the same sentence, it will return the one that shows up first to the factoid type that it is looking for.



    static int[] CalculateSimilarity(string question, string[] text)
    // giving an array of percentage similarity between the question and the sentences in the same order of sentences
    {
        // int similarityCounter = 0;
        // int[] percentageSimilarityArray = new int[text.Length];
        // int lengthOfQuestions = question.Length;
        // string[] questionWords = Split(RemoveStopWords(question));


        // List<string[]> sentenceWords = [];  // FIXME: change this to a static array

        // for (int i = 0; i < text.Length; i++)
        // {
        //     sentenceWords.Add(Split(text[i]));
        // }

        // // Check if each word of the sentence is in the entire array of strings of the question, then increment counter  
        // for (int i = 0; i < sentenceWords.Count; i++)
        // {

        //     for (int j = 0; j < sentenceWords[i].Length; j++)
        //     {
        //         for (int u = 0; u < questionWords.Length; u++)
        //         {
        //             if (sentenceWords[i][j] == questionWords[u])
        //             {
        //                 similarityCounter++;
        //                 break;
        //             }
        //         }
        //     }
        //     percentageSimilarityArray[i] = similarityCounter / lengthOfQuestions * 100;
        //     similarityCounter = 0;
        // }

        // return percentageSimilarityArray;

        int[] result = new int[text.Length];
        string[] questionWords = Split(question);

        for (int i = 0; i < text.Length; i++)
        {
            int similarityCounter = 0;
            string[] wordText = Split(text[i]);
            for (int j = 0; j < wordText[i].Length; j++)
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
            result[i] = similarityCounter / question.Length * 100;
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
                if (separators[j] == '.' && Char.IsNumber(text[i - 1]) && Char.IsNumber(text[i + 1])) continue;
                if (text[i] == separators[j])
                {
                    found = true;
                    break;
                }
            }
            if (found || i == text.Length - 1)
            {
                array[i] = currentWord;
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

    string ToLower(ref string text)
    {
        int i;
        char[] listUpper = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                            'U', 'V', 'W', 'X', 'Y', 'Z' };

        char[] listLower = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
                            'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
                            'u', 'v', 'w', 'x', 'y', 'z' };

        for (i = 0; i < listUpper.Length; i++)
        {
            if (text[0] == listUpper[i])
            {
                break;
            }
        }

        return Replace(text, Convert.ToString(text[0]), Convert.ToString(listLower[i]));
    }

    // ** TODO :
    static string[]? GetAnswer(string questionType, string[] text, int[] similarity)
    {
        string[]? result = [];

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
                // result = GetAmount(text[maxIndex]);

                // if (result != null)
                // {
                //     return result;
                // }
                // else
                // {
                //     similarity[maxIndex] = 0;
                //     maxIndex = HighestIndex(similarity);
                //     continue;
                // }
            }

        }

        return null;
    }

    static int HighestIndex(int[] array)
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

// TODO - do not remove the '.' if there the char IsNumber on both sides of the '.'