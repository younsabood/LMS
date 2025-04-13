using System;
using System.Collections.Generic;

namespace LMS
{
    public static class PromptTemplates
    {
        public const string DifficultyPlaceholder = "{difficulty}";
        public const string LanguagePlaceholder = "{language}";
        public const string ContentDomainPlaceholder = "{domain}";
        public const string MinimumQuestionsPlaceholder = "{min_questions}";
        public const string ExampleExam = "{ExampleExam}";
        public const string SourceDocument = "{SourceDocument}";

        public static string YesNoTemplate = @"
            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" HIGH-QUALITY ACADEMIC YES/NO QUESTIONS from " + SourceDocument + @".pdf.
            You are functioning as an advanced academic assessment generator with expertise in " + ContentDomainPlaceholder + @".
            FOCUS EXCLUSIVELY on substantive content in " + SourceDocument + @".pdf - ignore all metadata, front matter, indexes, appendices, and references.

            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core concepts, basic relationships)
            - Level 3-4: Advanced undergraduate (application, initial analysis)
            - Level 5-6: Masters-level graduate (complex analysis, synthesis)
            - Level 7-8: Doctoral/research level (theoretical integration, methodological evaluation)
            - Level 9-10: Expert practitioner/researcher (cutting-edge applications, theoretical contributions)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of concepts.
            For levels 3-4: Require thorough analysis and integration of related concepts.
            For levels 5-6: Demand synthesis across multiple frameworks and evaluation of competing perspectives.
            For levels 7-8: Necessitate critique of methodologies and theoretical reconciliation.
            For levels 9-10: Require expert judgment on emergent applications and theoretical extensions.

            [EXTRACTION IMPERATIVES]
            Extract fundamental frameworks, methodological approaches, empirical findings, and scholarly arguments directly from " + SourceDocument + @".pdf content.
            PRIORITIZE disciplinary intersections, methodological nuances, statistical implications, and theoretical extensions.

            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE YES/NO QUESTIONS with EXACTLY 50% 'Yes' answers and 50% 'No' answers from " + SourceDocument + @".pdf content.
            2. EVERY question MUST necessitate higher-order thinking: inference, concept integration, and critical evaluation.
            3. EACH answer MUST be explicitly supported with direct textual evidence from " + SourceDocument + @".pdf content.
            4. ALWAYS INCLUDE a comprehensive 'source' field with direct references from " + SourceDocument + @".pdf, including page number.
            5. PROVIDE an 'explanation' field that derives its content directly from " + SourceDocument + @".pdf, offering a detailed academic rationale.
            6. ENSURE questions distribute evenly across document sections to cover full content breadth as found in " + SourceDocument + @".pdf.
            7. ALL answers and explanations MUST be derived from " + SourceDocument + @".pdf content.
            8. QUESTIONS MUST HAVE RANDOMLY DISTRIBUTED 'Yes' AND 'No' ANSWERS - do not follow a predictable pattern like alternating Yes/No or grouping similar answers together. However, ensure the FIRST question must have an answer of 'Yes' and the LAST question must have an answer of 'No'.

            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted PRECISELY as:
            [
              {
                ""question"": ""Precise, unambiguous question requiring subject mastery"",
                ""answer"": ""Yes|No"",
                ""source"": ""SourceDocument.pdf (Page X)"",
                ""explanation"": ""Comprehensive academic rationale derived directly from SourceDocument.pdf"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": """ + ContentDomainPlaceholder + @"""
              }
            ]

            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @".
            MAINTAIN field-appropriate academic terminology and conventions consistent with " + ContentDomainPlaceholder + @".
            ";

        public static string OptionsTemplateOnePDF = @"
            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" RIGOROUS MULTIPLE-CHOICE ASSESSMENT ITEMS from " + SourceDocument + @".pdf.
            You are functioning as an advanced academic assessment designer with expertise in " + ContentDomainPlaceholder + @".
            FOCUS EXCLUSIVELY on substantive content in " + SourceDocument + @".pdf - ignore all metadata, front matter, indexes, appendices, and references.

            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core concepts, basic relationships)
            - Level 3-4: Advanced undergraduate (application, initial analysis)
            - Level 5-6: Masters-level graduate (complex analysis, synthesis)
            - Level 7-8: Doctoral/research level (theoretical integration, methodological evaluation)
            - Level 9-10: Expert practitioner/researcher (cutting-edge applications, theoretical contributions)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of concepts.
            For levels 3-4: Require thorough analysis and integration of related concepts.
            For levels 5-6: Demand synthesis across multiple frameworks and evaluation of competing perspectives.
            For levels 7-8: Necessitate critique of methodologies and theoretical reconciliation.
            For levels 9-10: Require expert judgment on emergent applications and theoretical extensions.

            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE MULTIPLE-CHOICE ASSESSMENT ITEMS with content extracted directly from " + SourceDocument + @".pdf.
            2. EACH question MUST include EXACTLY 4 options (A-D) with only ONE correct answer supported by " + SourceDocument + @".pdf content.
            3. ALL distractors MUST be plausible and based on common misconceptions from " + SourceDocument + @".pdf content.
            4. DISTRIBUTE correct answers EVENLY across options A, B, C, and D.
            5. INCLUDE a detailed 'source' field with specific evidence from " + SourceDocument + @".pdf (Page X).
            6. PROVIDE comprehensive 'explanation' that offers a rationale for the correct answer and analysis of distractors.
            7. ENSURE questions distribute evenly across sections in " + SourceDocument + @".pdf.
            8. ALL options MUST be relevant and derived from " + SourceDocument + @".pdf content.

            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted PRECISELY as:
            [
                {
                ""question"": ""Precise, unambiguous question requiring subject mastery"",
                ""answer"": ""Option A|Option B|Option C|Option D"",
                ""options"": [
                    ""Option A: Well-crafted response based on SourceDocument.pdf"",
                    ""Option B: Plausible distractor"",
                    ""Option C: Common misconception"",
                    ""Option D: Partial understanding""
                ],
                ""source"": ""SourceDocument.pdf (Page X)"",
                ""explanation"": ""Comprehensive rationale derived from SourceDocument.pdf"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": """ + ContentDomainPlaceholder + @"""
                }
            ]

            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @".
            MAINTAIN field-appropriate academic terminology and conventions consistent with " + ContentDomainPlaceholder + @".
            ";

        public static string OptionsTemplateTwoPDF = @"
            [DOCUMENT HANDLING]
            TWO PDFs WILL BE PROVIDED:
            1. " + ExampleExam + @".pdf - Contains the REQUIRED QUESTION FORMAT and STYLE to emulate
            2. " + SourceDocument + @".pdf - Contains the CONTENT RESERVOIR for question generation

            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" RIGOROUS MULTIPLE-CHOICE ASSESSMENT ITEMS from " + SourceDocument + @".pdf.
            You are functioning as an advanced academic assessment designer with expertise in " + ContentDomainPlaceholder + @".
            FOCUS EXCLUSIVELY on substantive content in " + SourceDocument + @".pdf - ignore all metadata, front matter, indexes, appendices, and references.

            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core concepts, basic relationships)
            - Level 3-4: Advanced undergraduate (application, initial analysis)
            - Level 5-6: Masters-level graduate (complex analysis, synthesis)
            - Level 7-8: Doctoral/research level (theoretical integration, methodological evaluation)
            - Level 9-10: Expert practitioner/researcher (cutting-edge applications, theoretical contributions)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of concepts.
            For levels 3-4: Require thorough analysis and integration of related concepts.
            For levels 5-6: Demand synthesis across multiple frameworks and evaluation of competing perspectives.
            For levels 7-8: Necessitate critique of methodologies and theoretical reconciliation.
            For levels 9-10: Require expert judgment on emergent applications and theoretical extensions.

            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE MULTIPLE-CHOICE ASSESSMENT ITEMS with content extracted directly from " + SourceDocument + @".pdf.
            2. EACH question MUST include EXACTLY 4 options (A-D) with only ONE correct answer that is directly supported by " + SourceDocument + @".pdf content.
            3. ALL distractors (incorrect options) MUST be plausible and based on common misconceptions or partial understandings relevant to " + SourceDocument + @".pdf content.
            4. DISTRIBUTE correct answers PRECISELY and EVENLY across options A, B, C, and D.
            5. INCLUDE a detailed 'source' field with specific textual evidence from " + SourceDocument + @".pdf (Page X) that supports both the question and its correct answer.
            6. PROVIDE comprehensive 'explanation' that offers a rationale for why the correct answer is right and why each distractor is wrong, all directly sourced from " + SourceDocument + @".pdf.
            7. ENSURE questions distribute evenly across document sections to cover full content breadth as found in " + SourceDocument + @".pdf.
            8. ALL options (correct and distractors) MUST be relevant to the question and derived from " + SourceDocument + @".pdf content.

            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted PRECISELY as shown in " + ExampleExam + @".pdf:
            [
              {
                ""question"": ""Precise, unambiguous question requiring subject mastery from " + SourceDocument + @".pdf"",
                ""answer"": ""Option A|Option B|Option C|Option D"",
                ""options"": [
                  ""Option A: well-crafted response with disciplinary precision based on " + SourceDocument + @".pdf content"",
                  ""Option B: well-crafted response with disciplinary precision based on " + SourceDocument + @".pdf content"",
                  ""Option C: well-crafted response with disciplinary precision based on " + SourceDocument + @".pdf content"",
                  ""Option D: well-crafted response with disciplinary precision based on " + SourceDocument + @".pdf content""
                ],
                ""source"": ""Direct textual evidence extracted from " + SourceDocument + @".pdf (Page X)"",
                ""explanation"": ""Comprehensive academic rationale for the correct answer and analysis of each distractor, all derived from " + SourceDocument + @".pdf content"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": ""Subject classification""
              },
              ...
            ]

            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @".
            MAINTAIN field-appropriate academic terminology, precise language, and scholarly discourse conventions as consistent with " + ContentDomainPlaceholder + @".
            ";

        public static string MathOptionsTemplateTwoPDF = @"
            [DOCUMENT HANDLING]
            TWO PDFs WILL BE PROVIDED:
            1. " + ExampleExam + @".pdf - Contains the REQUIRED MATH QUESTION FORMAT, STYLE, and DIFFICULTY PROGRESSION to mirror
            2. " + SourceDocument + @".pdf - Contains the MATHEMATICAL CONTENT RESERVOIR, EXAMPLE PROBLEMS, and COGNITIVE COMPLEXITY BENCHMARKS for question generation
            [MATHEMATICAL CONTENT HANDLING]
            1. Preserve LaTeX formatting conventions from " + ExampleExam + @".pdf (\$equation\$, \\[display\\], etc.) while mirroring the source document's mathematical syntax
            2. Substitute numerical values to generate new, dissimilar numbers while maintaining equation integrity and adhering to the parameter ranges outlined in the source document
            3. Include step-by-step explanations that align with " + ExampleExam + @".pdf's solution narrative style, with a focus on mathematical reasoning and clarity
            4. Verify all formulas and numerical substitutions against " + SourceDocument + @".pdf content and " + ExampleExam + @".pdf's pedagogical patterns
            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" MULTIPLE-CHOICE ITEMS THAT:
            - Replicate " + ExampleExam + @".pdf's MATH QUESTION STRUCTURE and ANSWER DISTRIBUTION
            - Focus primarily on substantive mathematical content and examples as displayed in the PDF while using dissimilar numbers where appropriate
            - Maintain " + SourceDocument + @".pdf's CONCEPTUAL DENSITY and TECHNICAL PRECISION
            - Mirror the BALANCED DIFFICULTY PROGRESSION between " + ExampleExam + @".pdf and " + SourceDocument + @".pdf
            - Ensure parity in MATHEMATICAL NOTATION STYLES between both documents
            - Preserve the EXAMPLE EXAM's QUESTION-TO-CONTENT DENSITY RATIO for mathematical problems
            - Align with the SOURCE DOCUMENT's AXIOMATIC FRAMEWORKS and THEORETICAL FOUNDATIONS in math
            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core mathematical concepts, basic numerical relationships)
            - Level 3-4: Advanced undergraduate (application and initial numerical analysis)
            - Level 5-6: Masters-level graduate (complex mathematical analysis and synthesis)
            - Level 7-8: Doctoral/research level (theoretical integration and sophisticated methodological evaluation)
            - Level 9-10: Expert practitioner/researcher (cutting-edge mathematical applications and theoretical contributions)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of mathematical concepts.
            For levels 3-4: Require thorough analysis and integration of related mathematical principles.
            For levels 5-6: Demand synthesis across multiple mathematical frameworks and evaluation of competing numerical approaches.
            For levels 7-8: Necessitate critique of mathematical methodologies and theoretical reconciliation.
            For levels 9-10: Require expert judgment on advanced mathematical applications and theoretical extensions.
            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE MULTIPLE-CHOICE ASSESSMENT ITEMS with math content extracted directly from " + SourceDocument + @".pdf.
            2. EACH question MUST include EXACTLY 4 options (A-D) with only ONE correct answer that is directly supported by " + SourceDocument + @".pdf content.
            3. ALL distractors (incorrect options) MUST be plausible and based on common misconceptions or partial understandings relevant to " + SourceDocument + @".pdf mathematical examples.
            4. DISTRIBUTE correct answers PRECISELY and EVENLY across options A, B, C, and D.
            5. INCLUDE a detailed 'source' field with specific textual evidence from " + SourceDocument + @".pdf (Page X) that supports both the question and its correct answer.
            6. PROVIDE comprehensive 'explanation' that offers a rationale for why the correct answer is right and why each distractor is wrong, with particular reference to the mathematical concepts and numerical modifications from " + SourceDocument + @".pdf.
            7. ENSURE questions distribute evenly across document sections to cover the full breadth of mathematical content as found in " + SourceDocument + @".pdf.
            8. ALL options (correct and distractors) MUST be relevant to the mathematical question and derived from " + SourceDocument + @".pdf content.
            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted PRECISELY as shown in " + ExampleExam + @".pdf:
            [
              {
                ""question"": ""Precise, unambiguous math question requiring subject mastery from " + SourceDocument + @".pdf"",
                ""answer"": ""Option A|Option B|Option C|Option D"",
                ""options"": [
                  ""Option A: well-crafted mathematical response with disciplinary precision based on " + SourceDocument + @".pdf content"",
                  ""Option B: well-crafted mathematical response with disciplinary precision based on " + SourceDocument + @".pdf content"",
                  ""Option C: well-crafted mathematical response with disciplinary precision based on " + SourceDocument + @".pdf content"",
                  ""Option D: well-crafted mathematical response with disciplinary precision based on " + SourceDocument + @".pdf content""
                ],
                ""source"": ""Direct textual evidence extracted from " + SourceDocument + @".pdf (Page X)"",
                ""explanation"": ""Comprehensive academic rationale for the correct answer and analysis of each distractor, all derived from " + SourceDocument + @".pdf math content"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": ""Mathematics""
              },
              ...
            ]

            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @".
            MAINTAIN field-appropriate academic terminology, precise mathematical language, and scholarly discourse conventions as consistent with " + ContentDomainPlaceholder + @".
        ";


        public static string MathOptionsTemplateOnePDF = @"
            [DOCUMENT HANDLING]
            ONLY ONE PDF WILL BE PROVIDED:
            1. " + SourceDocument + @".pdf - Contains BOTH MATH CONTENT and FORMAT standards for question generation, with a focus on mathematical examples and problem-solving techniques
            [MATHEMATICAL CONTENT HANDLING]
            1. Preserve LaTeX formatting conventions (\$equation\$, \\[display\\], etc.)
            2. Substitute numerical values to create new examples with dissimilar numbers while ensuring the underlying mathematical principles remain intact
            3. Include step-by-step calculation explanations for solutions, emphasizing mathematical reasoning and clarity in each step
            4. Verify all formulas against " + SourceDocument + @".pdf content
            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" RIGOROUS MULTIPLE-CHOICE ASSESSMENT ITEMS from " + SourceDocument + @".pdf.
            You are functioning as an advanced academic assessment designer with expertise in mathematics.
            FOCUS EXCLUSIVELY on substantive mathematical content and examples in " + SourceDocument + @".pdf - ignore all metadata, front matter, indexes, appendices, and references.
            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core mathematical concepts, basic numerical relationships)
            - Level 3-4: Advanced undergraduate (application and initial analysis of mathematical problems)
            - Level 5-6: Masters-level graduate (complex analysis and synthesis of mathematical frameworks)
            - Level 7-8: Doctoral/research level (theoretical integration and sophisticated mathematical evaluation)
            - Level 9-10: Expert practitioner/researcher (advanced mathematical applications and theoretical contributions)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of mathematical concepts.
            For levels 3-4: Require thorough analysis and integration of related mathematical principles.
            For levels 5-6: Demand synthesis across multiple mathematical frameworks and evaluation of competing numerical strategies.
            For levels 7-8: Necessitate critique of mathematical methodologies and theoretical reconciliation.
            For levels 9-10: Require expert judgment on advanced mathematical applications and emergent theoretical extensions.
            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE MULTIPLE-CHOICE ASSESSMENT ITEMS with mathematical content extracted directly from " + SourceDocument + @".pdf.
            2. EACH question MUST include EXACTLY 4 options (A-D) with only ONE correct answer that is directly supported by " + SourceDocument + @".pdf content.
            3. ALL distractors (incorrect options) MUST be plausible and based on common misconceptions or partial understandings relevant to " + SourceDocument + @".pdf mathematical examples.
            4. DISTRIBUTE correct answers PRECISELY and EVENLY across options A, B, C, and D.
            5. INCLUDE a detailed 'source' field with specific textual evidence from " + SourceDocument + @".pdf (Page X) that supports both the question and its correct answer.
            6. PROVIDE a comprehensive 'explanation' that offers a step-by-step solution with mathematical justification, clearly explaining why the correct answer is right and why each distractor is wrong, based entirely on " + SourceDocument + @".pdf.
            7. ENSURE questions are evenly distributed across the full range of mathematical topics found in " + SourceDocument + @".pdf.
            8. ALL options (correct and distractors) MUST be relevant to the math question and derived from " + SourceDocument + @".pdf content.
            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted as:
            [
              {
                ""question"": ""Precise, unambiguous math question requiring subject mastery"",
                ""answer"": ""Option A|Option B|Option C|Option D"",
                ""options"": [
                  ""Option A: Mathematically precise response"",
                  ""Option B: Alternative plausible mathematical response"",
                  ""Option C: Common mathematical misconception"",
                  ""Option D: Partial mathematical understanding""
                ],
                ""source"": """ + SourceDocument + @".pdf (Page X)"",
                ""explanation"": ""Step-by-step solution with clear mathematical justification"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": ""Mathematics""
              }
            ]
            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @". 
            MAINTAIN field-appropriate academic terminology, precise mathematical notation conventions, and clarity that is expected within rigorous mathematical discourse.
        ";


        public static string ProgrammingOptionsTemplateTwoPDF = @"
            [DOCUMENT HANDLING]
            TWO PDFs WILL BE PROVIDED:
            1. " + ExampleExam + @".pdf - Contains FORMAT STANDARDS, CODE CONVENTIONS, and PROBLEM-SOLVING PATTERNS with emphasis on precise code examples and debugging snippets.
            2. " + SourceDocument + @".pdf - Provides ALGORITHMIC CONTENT and DEBUGGING SCENARIOS with AUTHENTIC COMPLEXITY specifically for coding challenges.
            [PROGRAMMING CONTENT HANDLING]
            1. Preserve code formatting from " + ExampleExam + @".pdf (syntax highlighting, indentation) while ensuring that generated examples closely mimic the code style and structural patterns provided.
            2. Generate programming questions that mirror " + ExampleExam + @".pdf's ERROR PATTERNS, incorporating distinct yet analogous code examples. Ensure these examples are not duplicates but follow a similar logical structure and style.
            3. Create distractors based on " + ExampleExam + @".pdf's COMMON MISTAKES and " + SourceDocument + @".pdf's DEBUGGING CONTEXTS. 
               - In addition, introduce alternative coding implementations that are syntactically correct but logically or optimally flawed, as well as examples with subtle divergence in algorithm choices.
            4. Validate all code constructs against both documents' technical specifications, making sure all code snippets preserve the intended syntax and semantics.
            5. Maintain parity in CODE COMPLEXITY PROGRESSION between the example exam and source material, while allowing for diverse coding scenarios.
            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" MULTIPLE-CHOICE ITEMS THAT:
            - Mirror EXAMPLE EXAM's CODE SNIPPET LENGTH and COMPLEXITY DISTRIBUTION, focusing intensely on programming logic and debugging details.
            - Preserve SOURCE DOCUMENT's ALGORITHMIC PARADIGM EMPHASIS by extracting and adapting key algorithmic challenges.
            - Maintain BALANCED REPRESENTATION of different programming constructs (loops, conditionals, data structures, etc.) with clear debugging contexts.
            - Align with both documents' ERROR TAXONOMY CLASSIFICATIONS while also creating similar but distinct questions using alternative code segments.
            - Reflect SOURCE DOCUMENT's PERFORMANCE ANALYSIS CRITERIA by including questions that require evaluation of efficiency and debugging strategies.
            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core concepts, basic coding relationships)
            - Level 3-4: Advanced undergraduate (code application and initial debugging analysis)
            - Level 5-6: Masters-level graduate (complex algorithm synthesis and debugging across frameworks)
            - Level 7-8: Doctoral/research level (theoretical integration and advanced performance debugging)
            - Level 9-10: Expert practitioner/researcher (cutting-edge applications and nuanced error analysis)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of code examples.
            For levels 3-4: Require thorough analysis of code snippets and integration of multiple programming concepts.
            For levels 5-6: Demand synthesis across multiple coding frameworks and evaluation of alternative implementations.
            For levels 7-8: Necessitate critique of complex methodologies and advanced code debugging.
            For levels 9-10: Require expert judgment on emergent code patterns and theoretical extensions in programming.
            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE MULTIPLE-CHOICE ASSESSMENT ITEMS with content extracted directly from " + SourceDocument + @".pdf.
            2. EACH question MUST include EXACTLY 4 options (A-D) with only ONE correct answer that is directly supported by " + SourceDocument + @".pdf content.
            3. ALL distractors (incorrect options) MUST be plausible and based on common misconceptions, partial understandings, or alternative coding patterns relevant to " + SourceDocument + @".pdf content.
            4. DISTRIBUTE correct answers PRECISELY and EVENLY across options A, B, C, and D.
            5. INCLUDE a detailed 'source' field with specific textual evidence from " + SourceDocument + @".pdf (Page X) that supports both the question and its correct answer.
            6. PROVIDE comprehensive 'explanation' that offers a rationale for why the correct answer is right and why each distractor is wrong, with examples of alternative code interpretations when applicable, all directly sourced from " + SourceDocument + @".pdf.
            7. ENSURE questions distribute evenly across document sections to cover the full content breadth as found in " + SourceDocument + @".pdf.
            8. ALL options (correct and distractors) MUST be relevant to the question and derived from " + SourceDocument + @".pdf content.
            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted as:
            [
              {
                ""question"": ""Precise, unambiguous programming question requiring subject mastery from " + SourceDocument + @".pdf, focusing on coding implementation and debugging nuances"",
                ""answer"": ""Option A|Option B|Option C|Option D"",
                ""options"": [
                  ""Option A: Correct implementation pattern"",
                  ""Option B: Common syntax/logic error"",
                  ""Option C: Suboptimal algorithm choice"",
                  ""Option D: Edge case oversight""
                ],
                ""source"": ""Direct textual evidence extracted from " + SourceDocument + @".pdf (Page X)"",
                ""explanation"": ""Comprehensive code analysis with step-by-step debugging rationale covering why the correct code works and why each distractor fails under specific conditions"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": ""Subject classification""
              },
              ...
            ]
            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @". Maintain focused programming terminology, clear code snippet formatting, and adherence to the content domain's coding conventions as specified in " + ContentDomainPlaceholder + @".
        ";


        public static string ProgrammingOptionsTemplateOnePDF = @"
            [DOCUMENT HANDLING]
            ONLY ONE PDF WILL BE PROVIDED:
            1. " + SourceDocument + @".pdf - Contains BOTH CONTENT and FORMAT standards for question generation with an emphasis on real-life coding examples and debugging scenarios.
            [PROGRAMMING CONTENT HANDLING]
            1. Preserve code formatting conventions (syntax highlighting, indentation) with a special focus on maintaining precise structure in code snippets.
            2. Generate questions about algorithm implementation and debugging, ensuring that each question features a unique code sample demonstrating the intended concept.
            3. Include code snippet-based distractors that reflect common syntax/logic errors, alternative coding approaches, and pitfalls that developers might encounter.
            4. Verify all programming constructs against " + SourceDocument + @".pdf content, with additional attention to subtle differences in implementation style to produce similar, yet distinct questions.
            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" RIGOROUS MULTIPLE-CHOICE ASSESSMENT ITEMS from " + SourceDocument + @".pdf.
            You are functioning as an advanced academic assessment designer with expertise in " + ContentDomainPlaceholder + @". Focus exclusively on substantive programming content in " + SourceDocument + @".pdf.
            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core concepts, basic relationships in code)
            - Level 3-4: Advanced undergraduate (practical application and initial debugging)
            - Level 5-6: Masters-level graduate (synthesis of complex algorithms and debugging strategies)
            - Level 7-8: Doctoral/research level (theoretical integration and detailed code analysis)
            - Level 9-10: Expert practitioner/researcher (advanced coding practices and performance evaluation)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of coding examples.
            For levels 3-4: Require detailed analysis of implemented code and identification of errors.
            For levels 5-6: Demand synthesis across multiple code frameworks and evaluation of different debugging strategies.
            For levels 7-8: Necessitate critique of comprehensive methodologies and identification of nuanced implementation issues.
            For levels 9-10: Require expert judgment on advanced code optimizations and theoretical underpinnings of algorithmic performance.
            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE MULTIPLE-CHOICE ASSESSMENT ITEMS with content extracted directly from " + SourceDocument + @".pdf.
            2. EACH question MUST include EXACTLY 4 options (A-D) with only ONE correct answer that is directly supported by " + SourceDocument + @".pdf content.
            3. ALL distractors (incorrect options) MUST be plausible and based on common misconceptions, overlooked syntax errors, or logical flaws relevant to " + SourceDocument + @".pdf content.
            4. DISTRIBUTE correct answers PRECISELY and EVENLY across options A, B, C, and D.
            5. INCLUDE a detailed 'source' field with specific textual evidence from " + SourceDocument + @".pdf (Page X) that supports both the question and its correct answer.
            6. PROVIDE comprehensive 'explanation' that offers a rationale for why the correct answer is right and why each distractor is wrong, including analysis of alternative code implementations where applicable, all directly sourced from " + SourceDocument + @".pdf.
            7. ENSURE questions distribute evenly across document sections to cover the full breadth of programming content found in " + SourceDocument + @".pdf.
            8. ALL options (correct and distractors) MUST be relevant to the question and derived from " + SourceDocument + @".pdf content.
            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted as:
            [
              {
                ""question"": ""Precise, unambiguous programming question demonstrating core coding concepts and debugging techniques from " + SourceDocument + @".pdf"",
                ""answer"": ""Option A|Option B|Option C|Option D"",
                ""options"": [
                  ""Option A: Correct code implementation"",
                  ""Option B: Common syntax error"",
                  ""Option C: Logic flaw in code execution"",
                  ""Option D: Inefficient or suboptimal approach""
                ],
                ""source"": """ + SourceDocument + @".pdf (Page X)"",
                ""explanation"": ""Step-by-step code analysis and debugging rationale, clarifying why the correct implementation works and how each alternative fails due to specific code issues"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": """ + ContentDomainPlaceholder + @"""
              }
            ]
            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @". 
            Maintain focused, field-appropriate programming terminology, precise code formatting, and adherence to the content domain's style as specified in " + ContentDomainPlaceholder + @", with an increased emphasis on distinct coding examples and debugging scenarios.
        ";

    }
}
