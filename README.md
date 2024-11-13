# Factoid-Algorithm-Project

---

## Project management

- [x] figure out the main breakdown :)
- [x] breakdown the steps suggested
- [ ] breakdown the tasks
- [ ] assign names for each task

---

## Some suggestions

- Adding a one or two line comment for the functions so we know what the function is supposed to do
- Renaming the value that is gonna be returned inside the function to `result` for debugging and understanding better
- Splitting the `test` and the `panel` codes
  - possible solution can be to have separate function for handling panel

---

## Steps

### 1. Question Analysis Module

- [x] Provide an analysis of the question analysis module.
- [x] Write an algorithm (pseudo-code) that generates the expected answer type (e.g., person’s name, date, location).
- [x] Write a C# function that takes a question as input and returns the **expected answer type**.

> [!NOTE]
> We have to create a function that gets the question and returns whether it is about `Person`, `Location`, `Date`, and `Number` based on the first (or the first two) words.

### Sentence Similarity Module

- [ ] Write a pseudo-code algorithm for removing stop words.
- [x] Write a C# function for removing stop words.
- [ ] Provide an analysis of how to calculate sentence similarity.
- [ ] Write a pseudo-code algorithm for calculating sentence similarity.
- [ ] Write a C# function for calculating sentence similarity.
- [ ] Write a C# function that finds the most similar sentence from a text.
- [ ] Test the Sentence Similarity Module with various inputs

### Answer Extraction Module

- [ ] Provide an analysis of the answer extraction module.
- [ ] Person’s Name Extraction: Implement extraction of person names based on the rule of uppercase words followed by lowercase letters.
  - [ ] Write a pseudo-code algorithm: Create pseudo-code that scans a sentence and identifies person names by looking for terms that begin with a capital letter followed by lowercase letters.
  - [ ] Write a C# function: Implement the getPersonName function in C# using the rule defined in the pseudo-code. The function should take a sentence as input and return any identified person names.
- [ ] Location Extraction: Extract locations based on uppercase words in the sentence.
  - [ ] Write a pseudo-code algorithm: Create pseudo-code to identify words in all uppercase (e.g., "CUPERTINO") and extract them as locations.
  - [ ] Write a C# function: Implement the getLocation function in C# that scans a sentence and returns the locations in uppercase letters.
- [ ] Date/Time Extraction: Extract specific date or time formats (e.g., "YYYY-MM-DD") from sentences.
  - [ ] Write a pseudo-code algorithm: Create pseudo-code for identifying dates and times using specific formats like "YYYY-MM-DD" or common time expressions.
  - [ ] Write a C# function: Implement the getDateTime function in C# to extract valid date/time formats from a sentence. The function should scan the text and return any identified dates or times.
- [ ] Number Extraction: Extract numeric values, including monetary amounts, percentages, and other numbers.
  - [ ] Write a pseudo-code algorithm: Create pseudo-code to identify numbers (e.g., "$1,000", "15%") and return them as numeric entities.
  - [ ] Write a C# function: Implement the getNumber function in C# that detects and returns monetary values, percentages, or other numeric entities from a sentence.
- [ ] Testing the Answer Extraction Module: Perform testing of the entire module to ensure that each entity (person name, location, date, number) is correctly extracted.
  - [ ] Prepare a variety of sentences with names, locations, dates, times, and numbers to validate the correct extraction of entities.
  - [ ] Test edge cases, such as names with multiple capitalized words, locations with mixedcase letters, and unusual date formats.
  - [ ] Analyze the performance of the rule-based system compared to real-world text to ensure robustness and accuracy.
- [ ] Write a C# function that applies the rules for all entity types: Create a function that combines all the entity extraction functions (getPersonName, getLocation, getDateTime, getNumber) and applies them to the most relevant sentence to extract the correct answer

### Testing

- [ ] Your program should allow the user to ask multiple factoid questions, such as: - `When was Konrad Zuse born?` - `Where was Konrad Zuse born?` - `Who is the creator of the first high-level programming language?`
- [ ] Report Enrichment: - Discuss three limitations of your system and potential solutions. - Identify and elaborate on the module that posed the most challenges during this project.

---

### Tasks to finish later

- [ ] Update the explanation section from case 3 in mainMenu (it is a placeholder for now).

## Functions suggested by the prof

- [ ] **determineFactoidType**

  - Analyzes the question to identify the type of expected answer (e.g., name, date, location).

- [ ] **removeStopWords**

  - Removes common, irrelevant words from the text to streamline question and sentence processing.

- [ ] **calculateSimilarity**

  - Computes lexical similarity between the question and sentences in the text by measuring word overlap.

- [ ] **getPersonName**

  - Extracts names based on the rule that names typically start with an uppercase letter followed by lowercase letters.

- [ ] **getLocation**

  - Identifies locations by scanning for words in all uppercase letters, indicating possible place names.

- [ ] **getDateTime**

  - Extracts date and time information by identifying specific formats (e.g., YYYY-MM-DD) within the text.

- [ ] **getNumber**

  - Identifies numeric values, such as monetary amounts and percentages, and returns them for answer extraction.

- [ ] **extractAnswer**
  - Combines all entity extraction functions (getPersonName, getLocation, getDateTime, getNumber) to retrieve the most relevant answer from a sentence.
