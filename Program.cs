public struct Array
{
    public string[] array;
    public int length;
}

class Program
{

    // add landing page
    // add update Txt fn before mainMenu
    // default state is to ask question and get answer UNLESS guide, exit, updateText

    //ASK QUESTION (default loop):
    // get input -> get factoid type (handling bad question here) -> split to sentences -> remove stop words -> get answer

    // ELSES: exit, update, guide



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
                {
                    string questionWithoutStop = RemoveStopWords(question);
                    text = Replace(text, "Inc.", "inc");
                    int[] percentageSimilar = CalculateSimilarity(questionWithoutStop, Split(text, "?!."));


                    //finding the most similar sentence
                    int iterOfMostSimilarSentence = 0;
                    for (int i = 1; i >= percentageSimilar.Length - 1; i++)
                    {
                        if (percentageSimilar[i] > percentageSimilar[iterOfMostSimilarSentence])
                        {
                            iterOfMostSimilarSentence = i;
                        }
                    }

                    // TODO get answer with percentageSimilar[iterOfMostSimilarSentence]                 
                    // Print answer
                }
            }

        }
        while (endProgram == false);

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
                // TODO: come up with a way to ignore none-name words that fit into these conditions
                // If a none-name word comes up, do we need to worry about it? The similarityCheckingModule will let us know what is the most simular and we can print that whole sentence as the asnwer to the factoid question. 
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
            int endIndex = size - 1;

            return result[0..endIndex];
        }

        static string[] GetLocation(string sentence)
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
                else if (sentence[i] == ' ')
                {
                    found = false;
                    result[size] = word;
                    size++;
                    word = "";
                }
            }

            return result;
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

        string ToLower(ref string text)
        {
            char[] listUpper = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                            'U', 'V', 'W', 'X', 'Y', 'Z' };

            char[] listLower = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
                            'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
                            'u', 'v', 'w', 'x', 'y', 'z' };

            for (int j = 0; j < listUpper.Length; j++)
            {
                if (text[0] == listUpper[j])
                {
                    text[0] = listLower[j]; //FIXME: this is not working
                    break;
                }
            }


            return text;
        }

        // ** TODO :
        static void printAnswerFn()
        {
            //Take the sentence with the highest simularity
            //Ensure that it has the right answer type in the sentence
            //if not, error message (ask user to rephrase question), or maybe check next highest % sentence
            //  else Print answer
        }

    }
}